using System.Linq;
using UnityEngine.Assertions;

namespace ProceduralLife.Simulation
{
    public class StateMachine : AState
    {
        public StateMachine(StateMachineDefinition definition, SimulationEntity entity) : base(entity)
        {
            this.Definition = definition;
            this.state = this.GetNewState();
        }

        public readonly StateMachineDefinition Definition;
        private AState state;
        
        public override StateDoData Do()
        {
            StateDoData doData = this.state.Do();
            doData.StateData.InitState(this.state);
            
            this.state = doData.NextState ?? this.GetNewState();
            return new StateDoData(new StateMachineData(doData.StateData), doData.Delay, this.state != null ? this : null);
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

        private AState GetNewState()
        {
            // [TODO] Implement get new state from definition
            return new MoveState(this.entity, SimulationContext.MapData.Tiles.ElementAt(UnityEngine.Random.Range(0, SimulationContext.MapData.Tiles.Count)).Key);
        }
    }
}