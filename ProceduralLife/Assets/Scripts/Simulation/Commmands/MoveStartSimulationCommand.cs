using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class MoveStartSimulationCommand : ASimulationCommand
    {
        public MoveStartSimulationCommand(SimulationEntity entity, Vector2Int newPosition, ulong duration)
        {
            this.entity = entity;
            this.newPosition = newPosition;
            this.duration = duration;
        }
        
        private readonly SimulationEntity entity;
        private readonly Vector2Int newPosition;
        private readonly ulong duration;
        private Vector2Int oldPosition;
        
        public override void Do()
        {
            this.oldPosition = this.entity.Position;
            this.entity.MoveStart(this.newPosition, this.ExecutionMoment, this.duration, true);
        }
        
        public override void Undo()
        {
            this.entity.MoveEnd(this.oldPosition);
        }
        
        public override void Redo()
        {
            this.entity.MoveStart(this.newPosition, this.ExecutionMoment, this.duration, true);
        }
    }
}