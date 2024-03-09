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
        
        public override void Do()
        {
            Debug.Log("Do");
            this.entity.MoveStart(this.newPosition, this.ExecutionMoment, this.duration);
        }
        
        public override void Undo()
        {
            Debug.Log("Undo");
            this.entity.MoveEndBackward();
        }
        
        public override void Redo()
        {
            Debug.Log("Redo");
            this.entity.MoveStart(this.newPosition, this.ExecutionMoment, this.duration);
        }
    }
}