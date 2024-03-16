namespace ProceduralLife.Simulation
{
    public record SimulationMoment
    {
        public SimulationMoment(ulong time, int order)
        {
            this.Time = time;
            this.Order = order;
        }

        public SimulationMoment(SimulationMoment other)
        {
            this.Time = other.Time;
            this.Order = other.Order;
        }
        
        public ulong Time;
        // This is used to ensure an ordering amongst moments that share the same time.
        public int Order = 0;
        
        public bool IsBefore(ulong moment) => this.Time < moment;
        public bool IsBefore(SimulationMoment moment) => this.Time < moment.Time || (this.Time == moment.Time && this.Order < moment.Order);
        public bool IsBeforeOrEqual(ulong moment) => this.Time <= moment;
        
        public bool IsAfter(ulong moment) => this.Time > moment;
        public bool IsAfter(SimulationMoment moment) => this.Time > moment.Time || (this.Time == moment.Time && this.Order > moment.Order);
        public bool IsAfterOrEqual(ulong moment) => this.Time >= moment;
    }
}