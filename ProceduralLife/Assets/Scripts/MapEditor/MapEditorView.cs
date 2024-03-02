using System.Collections.Generic;
using MHLib.Hexagon;
using ProceduralLife.Map;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralLife.MapEditor
{
    public class MapEditorView : MonoBehaviour
    {
        private readonly Dictionary<Vector2Int, GameObject> tilesView = new();
        
        private void OnTileAdded(Vector2Int tilePosition, TileDefinition tileDefinition)
        {
            Assert.IsTrue(!this.tilesView.ContainsKey(tilePosition));
            
            Vector3 worldPosition = HexagonHelper.TileToWorld(tilePosition, Constants.TILE_SIZE);
            GameObject newTileView = Instantiate(tileDefinition.View, worldPosition, tileDefinition.View.transform.rotation);
            
            this.tilesView.Add(tilePosition, newTileView);
        }

        private void OnTileRemoved(Vector2Int tilePosition)
        {
            Assert.IsTrue(this.tilesView.ContainsKey(tilePosition));
            
            Destroy(this.tilesView[tilePosition]);
            this.tilesView.Remove(tilePosition);
        }

        private void OnEnable()
        {
            AMapEditorCommand.AddedTileEvent += this.OnTileAdded;
            AMapEditorCommand.RemovedTileEvent += this.OnTileRemoved;
        }

        private void OnDisable()
        {
            AMapEditorCommand.AddedTileEvent -= this.OnTileAdded;
            AMapEditorCommand.RemovedTileEvent -= this.OnTileRemoved;
        }
    }
}