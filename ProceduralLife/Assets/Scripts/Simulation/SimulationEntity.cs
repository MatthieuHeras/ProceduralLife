using ProceduralLife.Map;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralLife.Simulation
{
    public class SimulationEntity : ASimulationElement
    {
        public SimulationEntity(SimulationEntityDefinition definition)
        {
            this.Definition = definition;
            this.state = new StateMachine(definition.StateMachineDefinition, this);
        }
        
        public readonly SimulationEntityDefinition Definition;
        public Vector2Int Position { get; private set; } = Vector2Int.zero;
        
        public event Action<Vector2Int, ulong, ulong, bool> MoveStartEvent = delegate { };
        public event Action<Vector2Int> MoveEndEvent = delegate { };
        
        private readonly List<AStateData> stateData = new();
        
        private int currentIndex = -1;
        private AState state;
        
        public override void Do()
        {
            Assert.IsTrue(this.currentIndex < this.stateData.Count);
            
            // Break future data, should happen when we break the replay to start a new timeline
            if (this.currentIndex < this.stateData.Count - 1)
                this.stateData.RemoveRange(this.currentIndex + 1, this.stateData.Count - 1 - this.currentIndex);
            
            StateDoData stateDoData = this.state.Do();
            
            SimulationMoment executionMoment = this.NextExecutionMoment;
            SimulationContext.SimulationTime.DelayElement(this, stateDoData.Delay);
            
            stateDoData.StateData.InitState(this.state)
                                 .InitExecutionMoments(executionMoment, this.NextExecutionMoment);
            
            this.stateData.Add(stateDoData.StateData);
            this.currentIndex++;
            
            this.state = stateDoData.NextState;
        }
        
        public override void Undo()
        {
            AStateData currentStateData = this.stateData[this.currentIndex];
            this.state = currentStateData.State;
            
            this.state.Undo(currentStateData);
            this.currentIndex--;

            if (this.currentIndex >= 0)
                this.PreviousExecutionMoment = this.stateData[this.currentIndex].ExecutionMoment;
        }
        
        public override void Redo()
        {
            Assert.IsTrue(this.currentIndex < this.stateData.Count - 1);
            this.currentIndex++;
            
            this.state = this.stateData[this.currentIndex].State;
            this.state.Redo(this.stateData[this.currentIndex]);
            this.NextExecutionMoment = this.stateData[this.currentIndex].NextExecutionMoment;
        }
        
        public void MoveStart(Vector2Int newTarget, ulong startMoment, ulong duration, bool forward)
        {
            this.MoveStartEvent.Invoke(newTarget, startMoment, duration, forward);
        }
        
        public void MoveEnd(Vector2Int newPosition)
        {
            this.Position = newPosition;
            this.MoveEndEvent.Invoke(newPosition);
        }
    }
}