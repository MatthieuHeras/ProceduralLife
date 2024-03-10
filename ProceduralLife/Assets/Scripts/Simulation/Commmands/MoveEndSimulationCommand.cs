using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class MoveEndSimulationCommand : ASimulationCommand
    {
        public MoveEndSimulationCommand(ASimulationElement responsibleElement, SimulationEntity targetEntity, Vector2Int newPosition, ulong duration)
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
            this.targetEntity.MoveEnd(this.newPosition);
        }
        
        public override void Undo()
        {
            this.targetEntity.MoveStart(this.oldPosition, this.ExecutionMoment, this.duration, false);
        }
        
        public override void Redo()
        {
            this.targetEntity.MoveEnd(this.newPosition);
        }
    }
}