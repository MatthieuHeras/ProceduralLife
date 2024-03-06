namespace ProceduralLife.Simulation
{
    public abstract class ASimulationElement
    {
        public ulong ExecutionMoment;

        public abstract ASimulationCommand Apply(SimulationContext context);
    }
}