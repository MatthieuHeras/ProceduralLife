using ProceduralLife.Map;

namespace ProceduralLife.Simulation
{
    public static class SimulationContext
    {
        public static void InitTime(SimulationTime simulationTime) => SimulationTime = simulationTime;
        public static void InitMapData(MapData mapData) => MapData = mapData;

        public static SimulationTime SimulationTime { get; private set; }
        public static MapData MapData { get; private set; }
    }
}