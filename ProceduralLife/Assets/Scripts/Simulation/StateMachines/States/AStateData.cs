namespace ProceduralLife.Simulation
{
    public abstract record AStateData
    {
        public AStateData InitExecutionMoments(SimulationMoment executionMoment, SimulationMoment nextExecutionMoment)
        {
            this.ExecutionMoment = executionMoment;
            this.NextExecutionMoment = nextExecutionMoment;
            
            return this;
        }
        
        public AStateData InitState(AState state)
        {
            this.State = state;
            
            return this;
        }
        
        public SimulationMoment ExecutionMoment { get; private set; }
        public SimulationMoment NextExecutionMoment { get; private set; }
        
        public AState State { get; private set; }
    }
}