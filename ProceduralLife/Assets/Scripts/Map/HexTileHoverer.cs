using System;
using MHLib.Hexagon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.Map
{
    public class HexTileHoverer : MonoBehaviour
    {
        private static readonly Plane GROUND_PLANE = new(Vector3.up, Vector3.zero);
        
        [SerializeField, Required]
        private Camera mainCamera;

        public Vector2Int? CurrentTilePosition { get; private set; }= null;
        
        public static event Action<Vector2Int?> HoverTileEvent = delegate {  };

        private void Update()
        {
            Vector2Int? newTilePosition = HexagonHelper.ScreenToTile(Input.mousePosition, this.mainCamera, GROUND_PLANE, Constants.TILE_SIZE);

            if (newTilePosition != this.CurrentTilePosition)
            {
                this.CurrentTilePosition = newTilePosition;
                HoverTileEvent.Invoke(newTilePosition);
            }
        }
    }
}