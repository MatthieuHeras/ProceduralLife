namespace ProceduralLife.Simulation
{
    using UnityEngine;

    public record TestStateData : AStateData
    {
        public TestStateData(Vector2Int position, Vector2Int target)
            : base()
        {
            this.Position = position;
            this.Target = target;
        }

        public readonly Vector2Int Position;
        public readonly Vector2Int Target;
    }
}