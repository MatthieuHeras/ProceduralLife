using ProceduralLife.Simulation.View;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class SpawnEntityInput : AInput
    {
        public SpawnEntityInput(InputSimulationElement inputElement, SimulationEntityDefinition entityDefinition, Vector2Int spawnPosition)
            : base(inputElement)
        {
            this.entityDefinition = entityDefinition;
            this.spawnPosition = spawnPosition;
        }

        private readonly SimulationEntityDefinition entityDefinition;
        private readonly Vector2Int spawnPosition;
        
        private SimulationEntity entity;
        
        public override void Do()
        {
            this.entity = new SimulationEntity(this.entityDefinition);
            SimulationEntityView entityView = Object.Instantiate(this.entityDefinition.View);
            entityView.Init(entity);
            
            this.entity.MoveEnd(spawnPosition);
            SimulationContext.SimulationTime.SpawnElement(entity, this.inputElement.NextExecutionMoment.Time);
        }

        public override void Undo()
        {
            SimulationContext.SimulationTime.DespawnElement(this.entity);
        }

        public override void Redo()
        {
            SimulationContext.SimulationTime.RespawnElement(this.entity);
        }
    }
}