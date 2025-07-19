using ProceduralLife.Inputs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralLife.Simulation
{
    public class SimulationTime
    {
        public SimulationTime()
        {
            SimulationContext.InitTime(this);
            
            this.iterateMethod = this.IterateForward;

            InputManager.SimulationChanged += this.OnSimulationChanged;
            
            this.SpawnElement(new InputSimulationElement(), 0ul);
        }
        
        ~SimulationTime()
        {
            InputManager.SimulationChanged -= this.OnSimulationChanged;
        }

        // Sorted lists
        private readonly List<ASimulationElementBase> aliveElements = new();

        public ulong CurrentTime { get; private set; }
        private ulong presentTime;
        
        private Action<ulong> iterateMethod;
        
        public static event Action<ulong, ulong> CurrentTimeChanged = delegate { };
        public static event Action<E_IterationMethodType> IterationMethodChanged = delegate { }; 
        
        #region TIME DIRECTION
        public void Forward()
        {
            if (this.iterateMethod == this.IterateForward || this.iterateMethod == this.IterateReplay)
                return;
            
            this.ChangeIterationMethod(this.presentTime == this.CurrentTime ? E_IterationMethodType.PLAY : E_IterationMethodType.REPLAY);
        }
        
        public void Backward()
        {
            if (this.iterateMethod == this.IterateBackward)
                return;
            
            this.ChangeIterationMethod(E_IterationMethodType.BACKWARD);
        }

        private void OnSimulationChanged()
        {
            if (this.iterateMethod == this.IterateForward)
                return;

            if (this.iterateMethod != this.IterateReplay)
                Debug.LogError("Trying to impact the simulation while it's not going forward.");
            
            this.ChangeIterationMethod(E_IterationMethodType.PLAY);
        }

        private void ChangeIterationMethod(E_IterationMethodType iterationMethodType)
        {
            // Sort alive elements depending on the time direction.
            switch (iterationMethodType)
            {
                case E_IterationMethodType.PLAY:
                case E_IterationMethodType.REPLAY:
                    this.aliveElements.Sort((element1, element2) => element1.NextExecutionMoment.IsBefore(element2.NextExecutionMoment) ? -1 : 1);
                    break;
                case E_IterationMethodType.BACKWARD:
                    this.aliveElements.Sort((element1, element2) => element1.PreviousExecutionMoment.IsBefore(element2.PreviousExecutionMoment) ? -1 : 1);
                    break;
            }
            
            this.iterateMethod = iterationMethodType switch
            {
                E_IterationMethodType.PLAY => this.IterateForward,
                E_IterationMethodType.REPLAY => this.IterateReplay,
                E_IterationMethodType.BACKWARD => this.IterateBackward
            };
            
            IterationMethodChanged.Invoke(iterationMethodType);
        }
        #endregion TIME DIRECTION

        #region ELEMENTS LIFE
        public SimulationMoment DelayElement(ASimulationElementBase element, ulong delay)
        {
            Assert.IsTrue(this.aliveElements.Contains(element));
            Assert.IsTrue(this.aliveElements[0] == element);
            
            this.aliveElements.RemoveAt(0);
            
            ulong nextExecutionTime = element.NextExecutionMoment.Time + delay;
            
            int elementIndex = 0;
            while (elementIndex < this.aliveElements.Count && this.aliveElements[elementIndex].NextExecutionMoment.IsBefore(nextExecutionTime))
                elementIndex++;
            
            bool isSimultaneous = false;
            while (elementIndex < this.aliveElements.Count && this.aliveElements[elementIndex].NextExecutionMoment.Time == nextExecutionTime)
            {
                elementIndex++;
                isSimultaneous = true;
            }
            
            int order = isSimultaneous ? this.aliveElements[elementIndex - 1].NextExecutionMoment.Order + 1 : 0;
            
            SimulationMoment nextExecutionMoment = new(nextExecutionTime, order);
            
            this.aliveElements.Insert(elementIndex, element);

            return nextExecutionMoment;
        }
        
        /// <summary> Spawn a new element in the simulation while it plays FORWARD. </summary>
        public void SpawnElement(ASimulationElementBase element, ulong birthTime)
        {
            // Inserting in the future is not supported by Undo/Redo
            Assert.IsTrue(birthTime <= this.CurrentTime);

            int elementIndex = 0;
            while (elementIndex < this.aliveElements.Count && this.aliveElements[elementIndex].NextExecutionMoment.IsBefore(birthTime))
                elementIndex++;

            bool isSimultaneous = false;
            while (elementIndex < this.aliveElements.Count && this.aliveElements[elementIndex].NextExecutionMoment.Time == birthTime)
            {
                elementIndex++;
                isSimultaneous = true;
            }

            int birthOrder = isSimultaneous ? this.aliveElements[elementIndex - 1].NextExecutionMoment.Order + 1 : 0;
            SimulationMoment birthMoment = new(birthTime, birthOrder);
            
            element.InitBirthMoment(birthMoment);
            
            this.aliveElements.Insert(elementIndex, element);
            
            element.ReachBirthMoment(true);
        }
        
        /// <summary> Undo the spawn a new element in the simulation while it plays BACKWARD. </summary>
        public void DespawnElement(ASimulationElementBase element)
        {
            this.aliveElements.Remove(element);
            element.ReachBirthMoment(false);
        }
        
        /// <summary> Redo the spawn a new element in the simulation while it REPLAYS. </summary>
        public void RespawnElement(ASimulationElementBase element)
        {
            Assert.IsTrue(!this.aliveElements.Contains(element));
            int elementIndex = 0;
            
            while (elementIndex < this.aliveElements.Count && element.NextExecutionMoment.IsAfter(this.aliveElements[elementIndex].NextExecutionMoment))
                elementIndex++;
            
            this.aliveElements.Insert(elementIndex, element);
            element.ReachBirthMoment(true);
        }
        
        /// <summary> Remove an element from the simulation while it plays FORWARD. </summary>
        public void KillElement(ASimulationElementBase element)
        {
            this.aliveElements.Remove(element);
            element.ReachDeathMoment(true);
        }
        
        /// <summary> Undo the removal of an element from the simulation while it plays BACKWARD. </summary>
        public void ResurrectElement(ASimulationElementBase element)
        {
            int elementIndex = this.aliveElements.Count;
            
            while (elementIndex > 0 && element.PreviousExecutionMoment.IsBefore(this.aliveElements[elementIndex].PreviousExecutionMoment))
                elementIndex--;
            
            this.aliveElements.Insert(elementIndex, element);
            element.ReachDeathMoment(false);
        }
        
        /// <summary> Redo the removal of an element from the simulation while it REPLAYS. </summary>
        public void ReKillElement(ASimulationElementBase element)
        {
            // For now, it's the same as KillElement, but it might change later on.
            this.KillElement(element);
        }
        #endregion ELEMENTS LIFE
        
        #region ITERATE
        public void Iterate(ulong deltaTime)
        {
            ulong previousTime = this.CurrentTime;
            this.iterateMethod(deltaTime);
            
            CurrentTimeChanged.Invoke(previousTime, this.CurrentTime);
        }
        
        private void IterateForward(ulong deltaTime)
        {
            this.CurrentTime += deltaTime;
            this.presentTime = this.CurrentTime;
            
            this.ApplyElementsForward();
        }
        
        private void IterateBackward(ulong deltaTime)
        {
            if (deltaTime > this.CurrentTime)
                this.CurrentTime = 0;
            else
                this.CurrentTime -= deltaTime;
            
            this.ApplyElementsBackward();
        }
        
        private void IterateReplay(ulong deltaTime)
        {
            Assert.IsTrue(this.presentTime > this.CurrentTime);
            
            if (this.CurrentTime + deltaTime < this.presentTime)
            {
                this.CurrentTime += deltaTime;
                this.ApplyElementsReplay();
                return;
            }
            
            ulong replayTime = this.presentTime - this.CurrentTime;
            ulong forwardDeltaTime = deltaTime - replayTime;
            
            this.CurrentTime += replayTime;
            this.ApplyElementsReplay();
            
            IterationMethodChanged.Invoke(E_IterationMethodType.PLAY);
            this.iterateMethod = this.IterateForward;
            this.IterateForward(forwardDeltaTime);
        }
        #endregion ITERATE
        
        #region APPLY ELEMENTS
        private void ApplyElementsForward()
        {
            while (this.aliveElements.Count > 0 && this.aliveElements[0].NextExecutionMoment.IsBeforeOrEqual(this.CurrentTime))
            {
                ASimulationElementBase element = this.aliveElements[0];
                element.Do();
            }
        }
        
        private void ApplyElementsBackward()
        {
            while (this.aliveElements.Count > 0 && this.aliveElements[^1].PreviousExecutionMoment.IsAfterOrEqual(this.CurrentTime))
            {
                ASimulationElementBase element = this.aliveElements[^1];
                element.Undo();
                
                int elementIndex = this.aliveElements.Count - 1;
                while (elementIndex > 0 && this.aliveElements[elementIndex - 1].PreviousExecutionMoment.IsAfter(element.PreviousExecutionMoment))
                    elementIndex--;
                
                this.aliveElements.RemoveAt(this.aliveElements.Count - 1);
                this.aliveElements.Insert(elementIndex, element);
            }
        }
        
        private void ApplyElementsReplay()
        {
            while (this.aliveElements.Count > 0 && this.aliveElements[0].NextExecutionMoment.IsBeforeOrEqual(this.CurrentTime))
            {
                ASimulationElementBase element = this.aliveElements[0];
                
                element.Redo();
                
                int elementIndex = 0;
                while (elementIndex < this.aliveElements.Count - 1 && this.aliveElements[elementIndex + 1].NextExecutionMoment.IsBefore(element.NextExecutionMoment))
                    elementIndex++;
                
                this.aliveElements.RemoveAt(0);
                this.aliveElements.Insert(elementIndex, element);
            }
        }
        #endregion APPLY ELEMENTS
    }
}