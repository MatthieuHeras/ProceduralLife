namespace ProceduralLife.Simulation
{
    public abstract class ASimulationElement
    {
        protected ASimulationElement(ulong insertMoment)
        {
            this.ExecutionMoment = insertMoment;
        }
        
        public ulong ExecutionMoment;

        public abstract ASimulationCommand Apply(SimulationContext context);
    }
}