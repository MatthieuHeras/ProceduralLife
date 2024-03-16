namespace ProceduralLife.Simulation
{
    public abstract record AStateData
    {
        protected AStateData(SimulationMoment executionMoment, SimulationMoment nextExecutionMoment)
        {
            this.ExecutionMoment = executionMoment;
            this.NextExecutionMoment = nextExecutionMoment;
        }
        
        public readonly SimulationMoment ExecutionMoment;
        public readonly SimulationMoment NextExecutionMoment;
    }
}