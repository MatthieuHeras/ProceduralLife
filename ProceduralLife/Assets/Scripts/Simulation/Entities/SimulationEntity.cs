using System;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class SimulationEntity : ASimulationElement<StateMomentData>
    {
        public SimulationEntity(SimulationEntityDefinition definition)
        {
            this.Definition = definition;
            this.state = new SimulationEntityBrain(definition.BrainDefinition, this);
        }
        
        public readonly SimulationEntityDefinition Definition;
        public Vector2Int Position { get; private set; } = Vector2Int.zero;
        
        public event Action<Vector2Int, ulong, ulong, bool> MoveStartEvent = delegate { };
        public event Action<Vector2Int> MoveEndEvent = delegate { };
        
        private AState state;

        protected override StateMomentData ApplyDo()
        {
            StateDoData stateDoData = this.state.Do();
            SimulationMoment executionMoment = this.NextExecutionMoment;
            SimulationMoment nextMoment = SimulationContext.SimulationTime.DelayElement(this, stateDoData.Delay);
            
            stateDoData.StateData.InitState(this.state)
                                 .InitExecutionMoments(executionMoment, this.NextExecutionMoment);
            
            this.state = stateDoData.NextState;
            
            return new StateMomentData(nextMoment, stateDoData.StateData);
        }

        protected override void ApplyUndo(StateMomentData stateMomentData)
        {
            this.state = stateMomentData.StateData.State;
            this.state.Undo(stateMomentData.StateData);
        }

        protected override void ApplyRedo(StateMomentData stateMomentData)
        {
            this.state = stateMomentData.StateData.State;
            this.state.Redo(stateMomentData.StateData);
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