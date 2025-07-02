using System;
using System.Collections.Generic;
using ProceduralLife.Map;
using UnityEngine.Assertions;

namespace ProceduralLife.Simulation
{
    public class SimulationTime
    {
        public SimulationTime(MapData mapData)
        {
            this.SimulationContext = new SimulationContext(this, mapData);
            ASimulationElement.SetupContext(this.SimulationContext);
            
            this.iterateMethod = this.IterateForward;
        }
        
        public readonly SimulationContext SimulationContext;
        
        // Sorted lists
        public readonly List<ASimulationElement> DeadElements = new();
        public readonly List<ASimulationElement> AliveElements = new();
        public readonly List<ASimulationElement> ToBeBornElements = new();
        
        public ulong CurrentTime { get; private set; }
        public ulong PresentTime { get; private set; }
        
        private Action<ulong> iterateMethod;
        
        public static event Action<ulong, ulong> CurrentTimeChanged = delegate { };
        public static event Action<E_IterationMethodType> IterationMethodChanged = delegate { }; 
        
        #region TIME WAY
        public void ForwardStrict()
        {
            if (this.iterateMethod == this.IterateForward)
                return;
            
            this.AliveElements.Sort((element1, element2) => element1.NextExecutionMoment.IsBefore(element2.NextExecutionMoment) ? -1 : 1);
            IterationMethodChanged.Invoke(E_IterationMethodType.PLAY);
            this.iterateMethod = this.IterateForward;
        }
        
        public void ForwardReplay()
        {
            if (this.iterateMethod == this.IterateForward || this.iterateMethod == this.IterateReplay)
                return;
            
            this.AliveElements.Sort((element1, element2) => element1.NextExecutionMoment.IsBefore(element2.NextExecutionMoment) ? -1 : 1);
            IterationMethodChanged.Invoke(E_IterationMethodType.REPLAY);
            this.iterateMethod = this.IterateReplay;
        }
        
        public void Backward()
        {
            if (this.iterateMethod == this.IterateBackward)
                return;
            
            this.AliveElements.Sort((element1, element2) => element1.PreviousExecutionMoment.IsBefore(element2.PreviousExecutionMoment) ? -1 : 1);
            IterationMethodChanged.Invoke(E_IterationMethodType.BACKWARD);
            this.iterateMethod = this.IterateBackward;
        }
        #endregion TIME WAY

        #region ELEMENTS LIFE
        
        /// <summary> Use for player inputs. Entities should use their own execution moment, not current time. </summary>
        public void InsertElement(ASimulationElement element)
        {
            this.InsertElement(element, this.CurrentTime);
        }
        
        public void InsertElement(ASimulationElement element, ulong birthTime)
        {
            // Inserting in the future is not supported by Undo/Redo
            Assert.IsTrue(birthTime <= this.CurrentTime);

            int elementIndex = 0;
            while (elementIndex < this.AliveElements.Count && this.AliveElements[elementIndex].NextExecutionMoment.IsBefore(birthTime))
                elementIndex++;

            bool isSimultaneous = false;
            while (elementIndex < this.AliveElements.Count && this.AliveElements[elementIndex].NextExecutionMoment.Time == birthTime)
            {
                elementIndex++;
                isSimultaneous = true;
            }

            int birthOrder = isSimultaneous ? this.AliveElements[elementIndex - 1].NextExecutionMoment.Order + 1 : 0;
            SimulationMoment birthMoment = new(birthTime, birthOrder);
            
            element.InitBirth(birthMoment);
            this.AliveElements.Insert(elementIndex, element);
        }

        public void DelayElement(ASimulationElement element, ulong delay)
        {
            Assert.IsTrue(this.AliveElements.Contains(element));
            Assert.IsTrue(this.AliveElements.Count > 0 && this.AliveElements[0] == element);
            
            this.AliveElements.RemoveAt(0);
            
            ulong nextExecutionTime = element.NextExecutionMoment.Time + delay;
            
            int elementIndex = 0;
            while (elementIndex < this.AliveElements.Count && this.AliveElements[elementIndex].NextExecutionMoment.IsBefore(nextExecutionTime))
                elementIndex++;
            
            bool isSimultaneous = false;
            while (elementIndex < this.AliveElements.Count && this.AliveElements[elementIndex].NextExecutionMoment.Time == nextExecutionTime)
            {
                elementIndex++;
                isSimultaneous = true;
            }
            
            int order = isSimultaneous ? this.AliveElements[elementIndex - 1].NextExecutionMoment.Order + 1 : 0;
            
            SimulationMoment nextExecutionMoment = new(nextExecutionTime, order);
            element.NextExecutionMoment = nextExecutionMoment;
            
            this.AliveElements.Insert(elementIndex, element);
        }
        
        public void KillElement(ASimulationElement element)
        {
            Assert.IsTrue(this.AliveElements.Contains(element));
            this.AliveElements.Remove(element);
            this.DeadElements.Add(element);
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
            this.PresentTime = this.CurrentTime;
            
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
            Assert.IsTrue(this.PresentTime > this.CurrentTime);
            
            if (this.CurrentTime + deltaTime < this.PresentTime)
            {
                this.CurrentTime += deltaTime;
                this.ApplyElementsReplay();
                return;
            }
            
            ulong replayTime = this.PresentTime - this.CurrentTime;
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
            while (this.AliveElements.Count > 0 && this.AliveElements[0].NextExecutionMoment.IsBeforeOrEqual(this.CurrentTime))
            {
                ASimulationElement element = this.AliveElements[0];
                SimulationMoment previousMoment = new(element.NextExecutionMoment);
                element.Do();
                element.PreviousExecutionMoment = previousMoment;
            }
        }
        
        private void ApplyElementsBackward()
        {
            while (this.AliveElements.Count > 0 && this.AliveElements[^1].PreviousExecutionMoment.IsAfterOrEqual(this.CurrentTime)
                   || this.DeadElements.Count > 0 && this.DeadElements[^1].DeathMoment.IsAfterOrEqual(this.CurrentTime))
            {
                bool applyAlive = this.AliveElements.Count > 0 && (this.DeadElements.Count == 0 || this.AliveElements[^1].PreviousExecutionMoment.IsAfter(this.DeadElements[^1].DeathMoment));
                
                if (applyAlive)
                {
                    ASimulationElement element = this.AliveElements[^1];
                    
                    SimulationMoment nextMoment = new(element.PreviousExecutionMoment);
                    element.Undo();
                    element.NextExecutionMoment = nextMoment;
                    
                    if (element.BirthMoment == nextMoment)
                    {
                        this.ToBeBornElements.Insert(0, element);
                        this.AliveElements.RemoveAt(this.AliveElements.Count - 1);
                    }
                    else
                    {
                        int elementIndex = this.AliveElements.Count - 1;
                        while (elementIndex > 0 && this.AliveElements[elementIndex - 1].PreviousExecutionMoment.IsAfter(element.PreviousExecutionMoment))
                            elementIndex--;

                        this.AliveElements.RemoveAt(this.AliveElements.Count - 1);
                        this.AliveElements.Insert(elementIndex, element);
                    }
                }
                else
                {
                    ASimulationElement element = this.DeadElements[^1];
                    this.DeadElements.RemoveAt(this.DeadElements.Count - 1);
                    
                    this.ResurrectElement(element);
                }
            }
        }

        private void ResurrectElement(ASimulationElement element)
        {
            int elementIndex = this.AliveElements.Count;

            while (elementIndex > 0 && element.PreviousExecutionMoment.IsBefore(this.AliveElements[elementIndex].PreviousExecutionMoment))
                elementIndex--;
            
            this.AliveElements.Insert(elementIndex, element);
        }

        private void ApplyElementsReplay()
        {
            while (this.AliveElements.Count > 0 && this.AliveElements[0].NextExecutionMoment.IsBeforeOrEqual(this.CurrentTime)
                   || this.ToBeBornElements.Count > 0 && this.ToBeBornElements[0].BirthMoment.IsBeforeOrEqual(this.CurrentTime))
            {
                bool applyAlive = this.AliveElements.Count > 0 && (this.ToBeBornElements.Count == 0 || this.AliveElements[0].NextExecutionMoment.IsBefore(this.ToBeBornElements[0].BirthMoment));
                
                if (applyAlive)
                {
                    ASimulationElement element = this.AliveElements[0];
                    
                    if (element.NextExecutionMoment == element.DeathMoment)
                    {
                        this.DeadElements.Insert(this.DeadElements.Count - 1, element);
                        this.AliveElements.RemoveAt(0);
                    }
                    else
                    {
                        SimulationMoment previousMoment = new(element.NextExecutionMoment);
                        element.Redo();
                        element.PreviousExecutionMoment = previousMoment;

                        int elementIndex = 0;
                        while (elementIndex < this.AliveElements.Count - 1 && this.AliveElements[elementIndex + 1].NextExecutionMoment.IsBefore(element.NextExecutionMoment))
                            elementIndex++;
                        
                        this.AliveElements.RemoveAt(0);
                        this.AliveElements.Insert(elementIndex, element);
                    }
                }
                else
                {
                    ASimulationElement element = this.ToBeBornElements[0];
                    this.ToBeBornElements.RemoveAt(0);

                    this.RespawnElement(element);
                }
            }
        }
        
        private void RespawnElement(ASimulationElement element)
        {
            int elementIndex = 0;

            while (elementIndex < this.AliveElements.Count && element.NextExecutionMoment.IsAfter(this.AliveElements[elementIndex].NextExecutionMoment))
                elementIndex++;
            
            this.AliveElements.Insert(elementIndex, element);
        }
        #endregion APPLY ELEMENTS
    }
}