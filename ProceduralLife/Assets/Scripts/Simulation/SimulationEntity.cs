using System.Collections.Generic;
using MHLib.Hexagon;
using ProceduralLife.Map;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class SimulationEntity : ASimulationElement
    {
        public SimulationEntity(Vector2Int position)
        {
            this.Position = position;
        }
        
        public Vector2Int Position { get; private set; }
        
        public override ASimulationCommand Apply(SimulationContext context)
        {
            this.ExecutionMoment = context.SimulationTime.CurrentTime + 2000;
            context.SimulationTime.InsertUpcomingEntity(this);
            
            Vector2Int[] offsets = HexagonHelper.GetTileOffsets(this.Position.y);
            
            List<Vector2Int> neighbours = new(HexagonHelper.DIRECTIONS_COUNT);
            
            for (int i = 0, offsetsLength = offsets.Length; i < offsetsLength; i++)
            {
                Vector2Int neighbour = this.Position + offsets[i];
                if (context.MapData.Tiles.TryGetValue(neighbour, out TileDefinition tileDefinition) && !tileDefinition.HasWater)
                    neighbours.Add(neighbour);
            }
            
            int randomIndex = Random.Range(0, neighbours.Count);
            Vector2Int newPosition = neighbours[randomIndex];
            
            return new MoveSimulationCommand(context.SimulationTime.CurrentTime, this, newPosition);
        }
        
        public void Move(Vector2Int newPosition)
        {
            this.Position = newPosition;
            Debug.Log($"Move to {newPosition}");
        }
    }
}