namespace ProceduralLife.Simulation
{
    public abstract record AStateData
    {
        protected AStateData(SimulationMoment executionMoment)
        {
            this.ExecutionMoment = executionMoment;
        }
        
        public readonly SimulationMoment ExecutionMoment;
    }
}