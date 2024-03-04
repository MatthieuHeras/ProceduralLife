using System;
using MHLib.CommandPattern;
using ProceduralLife.Map;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralLife.MapEditor
{
    /// <summary>
    /// <para> This is a sandbox command pattern. All features are in this file, and children commands will pick from those.</para>
    /// <para> A few reasons why we do it this way :<br/>
    /// - it enables us to add static event Action here, used to update the view with a layer of abstraction, while restraining access to the call of these events<br/>
    /// - it's likely that commands will have similar behaviours, and this will keep us from duplicating code </para>
    /// </summary>
    public abstract class AMapEditorCommand : ACommand
    {
        protected AMapEditorCommand(MapData mapData, MapEditorData mapEditorData)
        {
            this.mapData = mapData;
            this.mapEditorData = mapEditorData;
        }

        protected readonly MapData mapData;
        protected readonly MapEditorData mapEditorData;

        public static event Action<Vector2Int, TileDefinition> AddedTileEvent = delegate {  };
        public static event Action<Vector2Int> RemovedTileEvent = delegate {  };
        public static event Action<TileDefinition> ChangedPaintedTileEvent = delegate {  };
        
        protected void AddTile(Vector2Int tilePosition, TileDefinition tile)
        {
            Assert.IsTrue(!this.mapData.Tiles.ContainsKey(tilePosition));
            
            this.mapData.Tiles.Add(tilePosition, tile);
            
            AddedTileEvent.Invoke(tilePosition, tile);
        }
        
        protected void RemoveTile(Vector2Int tilePosition)
        {
            Assert.IsTrue(this.mapData.Tiles.ContainsKey(tilePosition));

            this.mapData.Tiles.Remove(tilePosition);

            RemovedTileEvent.Invoke(tilePosition);
        }
        
        protected void ChangePaintedTile(TileDefinition newPaintedTile)
        {
            Assert.IsTrue(this.mapEditorData.PaintedTileDefinition != newPaintedTile);

            this.mapEditorData.PaintedTileDefinition = newPaintedTile;
            
            ChangedPaintedTileEvent.Invoke(newPaintedTile);
        }
    }
}