using ProceduralLife.Map;

namespace ProceduralLife.MapEditor
{
    public class ChangePaintedTileCommand : AMapEditorCommand
    {
        public ChangePaintedTileCommand(MapData mapData, MapEditorData mapEditorData, TileDefinition tileDefinition) : base(mapData, mapEditorData)
        {
            this.tileDefinition = tileDefinition;
        }

        private readonly TileDefinition tileDefinition;
        private TileDefinition oldTileDefinition;
        
        public override void Do()
        {
            this.oldTileDefinition = this.mapEditorData.PaintedTileDefinition;
            this.ChangePaintedTile(this.tileDefinition);
        }

        public override void Undo()
        {
            this.ChangePaintedTile(this.oldTileDefinition);
        }
    }
}