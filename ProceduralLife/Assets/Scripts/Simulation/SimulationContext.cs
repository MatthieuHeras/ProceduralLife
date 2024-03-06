using ProceduralLife.Map;

namespace ProceduralLife.Simulation
{
    public record SimulationContext
    {
        public SimulationContext(SimulationTime simulationTime, MapData mapData)
        {
            this.SimulationTime = simulationTime;
            this.MapData = mapData;
        }

        public readonly SimulationTime SimulationTime;
        public readonly MapData MapData;
    }
}