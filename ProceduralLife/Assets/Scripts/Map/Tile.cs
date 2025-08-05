using ProceduralLife.Simulation;
using System.Collections.Generic;

namespace ProceduralLife.Map
{
    public class Tile
    {
        public Tile(TileDefinition definition)
        {
            this.Definition = definition;
        }

        public readonly TileDefinition Definition;
        public readonly HashSet<SimulationEntity> Entities = new();
    }
}