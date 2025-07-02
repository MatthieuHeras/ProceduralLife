namespace ProceduralLife.Simulation
{
    public record MoveStateData : AStateData
    {
        public MoveStateData(bool isMoving, ulong previousDuration, ulong duration)
        {
            this.IsMoving = isMoving;
            this.PreviousDuration = previousDuration;
            this.Duration = duration;
        }
        
        public readonly bool IsMoving;
        public readonly ulong PreviousDuration;
        public readonly ulong Duration;
    }
}