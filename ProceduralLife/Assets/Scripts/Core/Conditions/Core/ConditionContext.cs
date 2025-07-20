using ProceduralLife.Simulation;

namespace ProceduralLife.Conditions
{
    public record ConditionContext(SimulationEntity Entity)
    {
        public SimulationEntity Entity { get; } = Entity;
    }
}