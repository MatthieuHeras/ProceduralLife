using MHLib.Hexagon;
using ProceduralLife.Map;
using ProceduralLife.MapEditor;
using UnityEngine;

namespace ProceduralLife
{
    public class HexTilePlacer : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        
        [SerializeField]
        private TileDefinition hexTile;
        
        private readonly Plane groundPlane = new(Vector3.up, Vector3.zero);
        
        private readonly float tileSize = 2f / Mathf.Sqrt(3f);

        private MapData mapData;

        private void Awake()
        {
            this.mapData = new MapData();
        }

        private void Update()
        {
            // WIP, waiting for proper input management and command handling. This is only here for testing
            if (Input.GetMouseButton(0))
            {
                Vector2Int? tilePosition = HexagonHelper.ScreenToTile(Input.mousePosition, this.mainCamera, this.groundPlane, this.tileSize);
                if (tilePosition == null || this.mapData.Tiles.ContainsKey(tilePosition.Value))
                    return;

                AddTileCommand newCommand = new(this.mapData, tilePosition.Value, this.hexTile);
                newCommand.Do();
            }
            else if (Input.GetMouseButton(1))
            {
                Vector2Int? tilePosition = HexagonHelper.ScreenToTile(Input.mousePosition, this.mainCamera, this.groundPlane, this.tileSize);
                if (tilePosition == null || !this.mapData.Tiles.ContainsKey(tilePosition.Value))
                    return;
                
                RemoveTileCommand newCommand = new(this.mapData, tilePosition.Value);
                newCommand.Do();
            }
        }
    }
}
