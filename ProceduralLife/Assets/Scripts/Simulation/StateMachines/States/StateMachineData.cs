namespace ProceduralLife.Simulation
{
    public record StateMachineData : AStateData
    {
        public StateMachineData(AStateData childStateData)
        {
            this.ChildStateData = childStateData;
        }

        public readonly AStateData ChildStateData;
    }
}