using ProceduralLife.Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.MapEditor
{
    public class MapEditorCommandGenerator : MonoBehaviour
    {
        [SerializeField, Required]
        private MapEditorCommandHandler commandHandler;
        
        public readonly MapData MapData = new();
        private readonly MapEditorData mapEditorData = new();
        
        private void GenerateCommand(AMapEditorCommand command) => this.commandHandler.DoCommand(command);
        
        public void GenerateAddTileCommand(Vector2Int tilePosition)
        {
            if (this.mapEditorData.PaintedTileDefinition != null && !this.MapData.Tiles.ContainsKey(tilePosition))
                this.GenerateCommand(new AddTileCommand(this.MapData, this.mapEditorData, tilePosition, this.mapEditorData.PaintedTileDefinition));
        }
        
        public void GenerateRemoveTileCommand(Vector2Int tilePosition)
        {
            if (this.MapData.Tiles.ContainsKey(tilePosition))
                this.GenerateCommand(new RemoveTileCommand(this.MapData, this.mapEditorData, tilePosition));
        }
        
        public void GenerateChangePaintedTileCommand(TileDefinition newPaintedTile)
        {
            if (this.mapEditorData.PaintedTileDefinition != newPaintedTile)
                this.GenerateCommand(new ChangePaintedTileCommand(this.MapData, this.mapEditorData, newPaintedTile));
        }
        
        private void OnEnable()
        {
            TilePicker.TilePickedEvent += this.OnTilePicked;
        }

        private void OnTilePicked(TileDefinition newTileDefinition)
        {
            this.GenerateChangePaintedTileCommand(newTileDefinition);
        }
    }
}