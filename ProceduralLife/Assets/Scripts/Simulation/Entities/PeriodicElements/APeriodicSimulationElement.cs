namespace ProceduralLife.Simulation.PeriodicElements
{
    public abstract class APeriodicSimulationElement<TMomentData> : ASimulationElement<TMomentData>
        where TMomentData : MomentData
    {
        protected APeriodicSimulationElement(ulong period)
        {
            this.period = period;
        }

        protected readonly ulong period;
    }
}