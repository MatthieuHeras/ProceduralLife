using ProceduralLife.Map;
using UnityEngine;

namespace ProceduralLife.MapEditor
{
    public class RemoveTileCommand : AMapEditorCommand
    {
        public RemoveTileCommand(MapData mapData, Vector2Int tilePosition) : base(mapData)
        {
            this.tilePosition = tilePosition;
        }

        private readonly Vector2Int tilePosition;
        private TileDefinition oldTileDefinition;
        
        public override void Do()
        {
            this.oldTileDefinition = this.mapData.Tiles[this.tilePosition];
            this.RemoveTile(this.tilePosition);
        }

        public override void Undo()
        {
            this.AddTile(this.tilePosition, this.oldTileDefinition);
        }
    }
}