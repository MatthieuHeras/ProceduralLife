namespace ProceduralLife.Simulation
{
    public abstract class AState
    {
        protected AState(SimulationEntity entity)
        {
            this.entity = entity;
        }
        
        protected readonly SimulationEntity entity;
        
        public abstract StateDoData Do();
        public abstract void Undo(AStateData stateData);
        public abstract void Redo(AStateData stateData);
    }
}