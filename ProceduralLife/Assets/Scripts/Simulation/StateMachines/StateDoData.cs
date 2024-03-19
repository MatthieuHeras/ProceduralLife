namespace ProceduralLife.Simulation
{
    public record StateDoData
    {
        public StateDoData(AStateData stateData, ulong delay, AState nextState)
        {
            this.StateData = stateData;
            this.Delay = delay;
            this.NextState = nextState;
        }

        public readonly AStateData StateData;
        public readonly ulong Delay;
        public readonly AState NextState;
    }
}