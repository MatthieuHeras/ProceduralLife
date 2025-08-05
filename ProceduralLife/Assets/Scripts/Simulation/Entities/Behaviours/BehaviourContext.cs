namespace ProceduralLife.Simulation
{
    public record BehaviourContext(SimulationEntity Entity)
    {
        public SimulationEntity Entity { get; } = Entity;
    }
}