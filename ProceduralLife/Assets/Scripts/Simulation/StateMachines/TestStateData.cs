namespace ProceduralLife.Simulation
{
    using UnityEngine;

    public record TestStateData : AStateData
    {
        public TestStateData(SimulationMoment executionMoment, Vector2Int position, Vector2Int target)
            : base(executionMoment)
        {
            this.Position = position;
            this.Target = target;
        }

        public readonly Vector2Int Position;
        public readonly Vector2Int Target;
    }
}