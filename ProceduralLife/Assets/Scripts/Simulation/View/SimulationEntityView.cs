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
            
            Transform selfTransform = this.transform;
            
            Vector3 positionDiff = worldPosition - selfTransform.position;
            float rotation = Mathf.Atan2(positionDiff.z, positionDiff.x) / Mathf.PI * 180f;
            selfTransform.position = worldPosition;
            selfTransform.rotation = Quaternion.Euler(0f, -rotation, 0f);
        }
    }
}