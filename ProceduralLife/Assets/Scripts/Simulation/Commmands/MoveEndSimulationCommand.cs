using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class MoveEndSimulationCommand : ASimulationCommand
    {
        public MoveEndSimulationCommand(SimulationEntity entity, Vector2Int newPosition, ulong duration)
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
            this.entity.MoveEnd(this.newPosition);
        }
        
        public override void Undo()
        {
            this.entity.MoveStart(this.oldPosition, this.ExecutionMoment, this.duration, false);
        }
        
        public override void Redo()
        {
            this.entity.MoveEnd(this.newPosition);
        }
    }
}