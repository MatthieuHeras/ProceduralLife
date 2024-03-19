namespace ProceduralLife.Simulation
{
    public abstract class AState
    {
        public abstract StateDoData Do();
        public abstract void Undo(AStateData stateData);
        public abstract void Redo(AStateData stateData);
    }
}