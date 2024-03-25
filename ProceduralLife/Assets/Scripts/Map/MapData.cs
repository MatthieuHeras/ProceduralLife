using MHLib.Hexagon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLife.Map
{
    [Serializable]
    public record MapData
    {
        public readonly Dictionary<Vector2Int, TileDefinition> Tiles = new();
        
        public Vector2Int[] GetTileNeighbours(Vector2Int tilePosition)
        {
            List<Vector2Int> neighbours = new();
            
            void Action(Vector2Int neighbourPosition)
            {
                if (this.Tiles.ContainsKey(neighbourPosition))
                    neighbours.Add(neighbourPosition);
            }
            
            HexagonHelper.ApplyOnNeighbours(tilePosition, Action);
            
            return neighbours.ToArray();
        }
    }
}