using ProceduralLife.Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.MapEditor
{
    public class MapEditorCommandGenerator : MonoBehaviour
    {
        [SerializeField, Required]
        private MapEditorCommandHandler commandHandler;
        
        // [TODO] Remove this and handle changing tile definition
        [SerializeField, Required]
        private TileDefinition defaultTileDefinition;

        private readonly MapData mapData = new();
        private readonly MapEditorData mapEditorData = new();
        
        private void GenerateCommand(AMapEditorCommand command) => this.commandHandler.DoCommand(command);
        
        public void GenerateAddTileCommand(Vector2Int tilePosition)
        {
            if (this.mapEditorData.PaintedTileDefinition != null && !this.mapData.Tiles.ContainsKey(tilePosition))
                this.GenerateCommand(new AddTileCommand(this.mapData, tilePosition, this.mapEditorData.PaintedTileDefinition));
        }
        
        public void GenerateRemoveTileCommand(Vector2Int tilePosition)
        {
            if (this.mapData.Tiles.ContainsKey(tilePosition))
                this.GenerateCommand(new RemoveTileCommand(this.mapData, tilePosition));
        }
        
        // [TODO] Remove this and handle changing tile definition
        private void Awake()
        {
            this.mapEditorData.PaintedTileDefinition = this.defaultTileDefinition;
        }
    }
}