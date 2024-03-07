using MHLib.Hexagon;
using UnityEngine;

namespace ProceduralLife.Simulation.View
{
    public class SimulationEntityView : MonoBehaviour
    {
        public void Init(SimulationEntity entity)
        {
            entity.PositionChangedEvent += this.OnPositionChanged;
        }
        
        private void OnPositionChanged(Vector2Int newPosition)
        {
            Vector3 worldPosition = HexagonHelper.TileToWorld(newPosition, Constants.TILE_SIZE);
            
            this.transform.position = worldPosition;
        }
    }
}