using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class MoveStartSimulationCommand : ASimulationCommand
    {
        public MoveStartSimulationCommand(ASimulationElement responsibleElement, SimulationEntity targetEntity, Vector2Int newPosition, ulong duration)
            : base(responsibleElement)
        {
            this.targetEntity = targetEntity;
            this.newPosition = newPosition;
            this.duration = duration;
        }
        
        private readonly SimulationEntity targetEntity;
        private readonly Vector2Int newPosition;
        private readonly ulong duration;
        private Vector2Int oldPosition;
        
        public override void Do()
        {
            this.oldPosition = this.targetEntity.Position;
            this.targetEntity.MoveStart(this.newPosition, this.ExecutionMoment, this.duration, true);
        }
        
        public override void Undo()
        {
            this.targetEntity.MoveEnd(this.oldPosition);
        }
        
        public override void Redo()
        {
            this.targetEntity.MoveStart(this.newPosition, this.ExecutionMoment, this.duration, true);
        }
    }
}