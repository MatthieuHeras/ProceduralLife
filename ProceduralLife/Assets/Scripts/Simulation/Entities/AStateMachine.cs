using UnityEngine.Assertions;

namespace ProceduralLife.Simulation
{
    public abstract class AStateMachine : AState
    {
        /// <param name="canEnd"> Set to false if you don't want to ever get out of the state machine. Useful for core state machines.</param>
        protected AStateMachine(SimulationEntity entity, bool canEnd) : base(entity)
        {
            this.canEnd = canEnd;
        }
        
        protected AState state = null;
        private readonly bool canEnd;
        
        public override StateDoData Do()
        {
            this.state = this.GetNewState();
            StateDoData doData = this.state.Do();
            doData.StateData.InitState(this.state);
            
            this.state = doData.NextState;
            
            bool shouldStop = this.canEnd && this.state == null;
            AState nextState = shouldStop ? null : this;
            
            return new StateDoData(new StateMachineData(doData.StateData), doData.Delay, nextState);
        }
        
        public override void Undo(AStateData stateData)
        {
            Assert.IsTrue(stateData is StateMachineData);
            StateMachineData stateMachineData = (StateMachineData)stateData;

            this.state = stateMachineData.ChildStateData.State;
            this.state.Undo(stateMachineData.ChildStateData);
        }

        public override void Redo(AStateData stateData)
        {
            Assert.IsTrue(stateData is StateMachineData);
            StateMachineData stateMachineData = (StateMachineData)stateData;
            
            this.state = stateMachineData.ChildStateData.State;
            this.state.Redo(stateMachineData.ChildStateData);
        }
        
        protected abstract AState GetNewState();
    }
}