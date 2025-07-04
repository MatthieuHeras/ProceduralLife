namespace ProceduralLife.Simulation
{
    public class WaitState : AState
    {
        public WaitState(SimulationEntity entity, ulong duration)
            : base(entity)
        {
            this.duration = duration;
        }

        private readonly ulong duration;
        
        public override StateDoData Do() => new(new WaitStateData(), this.duration, null);
        public override void Undo(AStateData stateData) { }
        public override void Redo(AStateData stateData) { }
    }
}