using ProceduralLife.Map;
using UnityEngine;

namespace ProceduralLife.MapEditor
{
    public class AddTileCommand : AMapEditorCommand
    {
        public AddTileCommand(MapData mapData, MapEditorData mapEditorData, Vector2Int tilePosition, TileDefinition tileDefinition) : base(mapData, mapEditorData)
        {
            this.tilePosition = tilePosition;
            this.tileDefinition = tileDefinition;
        }

        private readonly Vector2Int tilePosition;
        private readonly TileDefinition tileDefinition;
        
        public override void Do()
        {
            this.AddTile(this.tilePosition, this.tileDefinition);
        }

        public override void Undo()
        {
            this.RemoveTile(this.tilePosition);
        }
    }
}