namespace ProceduralLife.Simulation
{
    public record MomentData(SimulationMoment NextSimulationMoment)
    {
        public SimulationMoment NextSimulationMoment { get; } = NextSimulationMoment;
    }
}