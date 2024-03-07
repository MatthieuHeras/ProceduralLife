using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class MoveSimulationCommand : ASimulationCommand
    {
        public MoveSimulationCommand(SimulationEntity entity, Vector2Int newPosition)
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
            
            Debug.Log("Do");
            this.entity.Move(this.newPosition);
        }

        public override void Undo()
        {
            Debug.Log("Undo");
            this.entity.Move(this.oldPosition);
        }

        public override void Redo()
        {
            Debug.Log("Redo");
            this.entity.Move(this.newPosition);
        }
    }
}