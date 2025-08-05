using ProceduralLife.Map;
using UnityEngine;

namespace ProceduralLife.MapEditor
{
    public class RemoveTileCommand : AMapEditorCommand
    {
        public RemoveTileCommand(MapData mapData, MapEditorData mapEditorData, Vector2Int tilePosition) : base(mapData, mapEditorData)
        {
            this.tilePosition = tilePosition;
        }

        private readonly Vector2Int tilePosition;
        private TileDefinition oldTileDefinition;
        
        public override void Do()
        {
            this.oldTileDefinition = this.mapData.Tiles[this.tilePosition].Definition;
            this.RemoveTile(this.tilePosition);
        }

        public override void Undo()
        {
            this.AddTile(this.tilePosition, this.oldTileDefinition);
        }
    }
}