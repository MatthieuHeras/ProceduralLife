namespace ProceduralLife.Simulation
{
    public record MoveStateData : AStateData
    {
        public MoveStateData(bool isMoving)
        {
            this.IsMoving = isMoving;
        }

        public readonly bool IsMoving;
    }
}