using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLife.Map
{
    [Serializable]
    public record MapData
    {
        public readonly Dictionary<Vector2Int, TileDefinition> Tiles = new();
    }
}