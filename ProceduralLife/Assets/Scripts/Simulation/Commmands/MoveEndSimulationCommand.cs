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
            
            Debug.Log("Do");
            this.entity.MoveEnd(this.newPosition);
        }
        
        public override void Undo()
        {
            Debug.Log("Undo");
            this.entity.MoveStartBackward(this.oldPosition, this.ExecutionMoment, this.duration);
        }
        
        public override void Redo()
        {
            Debug.Log("Redo");
            this.entity.MoveEnd(this.newPosition);
        }
    }
}