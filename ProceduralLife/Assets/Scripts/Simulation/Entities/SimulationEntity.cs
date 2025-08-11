using System;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ProceduralLife.Simulation
{
    public class SimulationEntity : ASimulationElement<StateMomentData>
    {
        public SimulationEntity(SimulationEntityDefinition definition)
        {
            this.Definition = definition;
            this.brain = new SimulationEntityBrain(definition.BrainDefinition, this);
            
            // [TODO] Implement proper stat system
            this.Speed = definition.Speed * Random.Range(0.5f, 2f);
            this.Hunger = definition.MaxHunger;
            this.SightRange = definition.SightRange;
        }
        
        public readonly SimulationEntityDefinition Definition;
        public Vector2Int Position { get; private set; } = Vector2Int.zero;
        
        // [TODO] Make those stats
        public float Speed { get; private set; }
        public long Hunger { get; private set; }
        public uint SightRange { get; private set; }
        
        public event Action<Vector2Int, ulong, ulong, bool> MoveStartEvent = delegate { };
        public event Action<Vector2Int> MoveEndEvent = delegate { };
        public event Action HungerChanged = delegate { }; 
        
        private readonly SimulationEntityBrain brain;

        protected override StateMomentData ApplyDo()
        {
            StateDoData stateDoData = this.brain.Do();
            SimulationMoment executionMoment = this.NextExecutionMoment;
            SimulationMoment nextMoment = SimulationContext.SimulationTime.DelayElement(this, stateDoData.Delay);
            
            stateDoData.StateData.InitState(this.brain)
                                 .InitExecutionMoments(executionMoment, this.NextExecutionMoment);
            
            Assert.IsTrue(stateDoData.NextState == this.brain);
            
            return new StateMomentData(nextMoment, stateDoData.StateData);
        }

        protected override void ApplyUndo(StateMomentData stateMomentData)
        {
            Assert.IsTrue(stateMomentData.StateData.State == this.brain);
            this.brain.Undo(stateMomentData.StateData);
        }

        protected override void ApplyRedo(StateMomentData stateMomentData)
        {
            Assert.IsTrue(stateMomentData.StateData.State == this.brain);
            this.brain.Redo(stateMomentData.StateData);
        }
        
        public override void ReachBirthMoment(bool timeIsForward)
        {
            base.ReachBirthMoment(timeIsForward);

            if (timeIsForward)
                SimulationContext.MapData.Tiles[this.Position].Entities.Add(this);
            else
                SimulationContext.MapData.Tiles[this.Position].Entities.Remove(this);
        }
        
        public override void ReachDeathMoment(bool timeIsForward)
        {
            base.ReachDeathMoment(timeIsForward);

            if (timeIsForward)
                SimulationContext.MapData.Tiles[this.Position].Entities.Remove(this);
            else
                SimulationContext.MapData.Tiles[this.Position].Entities.Add(this);
        }
        
        public void MoveStart(Vector2Int newTarget, ulong startMoment, ulong duration, bool forward)
        {
            this.MoveStartEvent.Invoke(newTarget, startMoment, duration, forward);
        }
        
        public void MoveEnd(Vector2Int newPosition)
        {
            SimulationContext.MapData.Tiles[this.Position].Entities.Remove(this);
            this.Position = newPosition;
            SimulationContext.MapData.Tiles[this.Position].Entities.Add(this);
            this.MoveEndEvent.Invoke(newPosition);
        }

        public void ChangeHunger(long hungerOffset)
        {
            Hunger += hungerOffset;
            this.HungerChanged.Invoke();
        }
    }
}