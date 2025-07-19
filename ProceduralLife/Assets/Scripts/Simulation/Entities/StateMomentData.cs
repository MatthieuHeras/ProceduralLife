namespace ProceduralLife.Simulation
{
    public record StateMomentData(SimulationMoment NextSimulationMoment, AStateData StateData) : MomentData(NextSimulationMoment)
    {
        public AStateData StateData { get; } = StateData;
    }
}