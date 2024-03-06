using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class MoveSimulationCommand : ASimulationCommand
    {
        public MoveSimulationCommand(ulong executionMoment, SimulationEntity entity, Vector2Int newPosition) : base(executionMoment)
        {
            this.entity = entity;
            this.newPosition = newPosition;
        }

        private readonly SimulationEntity entity;
        private readonly Vector2Int newPosition;
        
        private Vector2Int oldPosition;
        
        public override void Do()
        {
            this.oldPosition = this.entity.Position;

            this.entity.Move(this.newPosition);
        }

        public override void Undo()
        {
            throw new System.NotImplementedException();
        }

        public override void Redo()
        {
            base.Redo();
        }
    }
}