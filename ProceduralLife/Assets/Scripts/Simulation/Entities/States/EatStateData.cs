namespace ProceduralLife.Simulation
{
    public record EatStateData : AStateData
    {
        public EatStateData(long foodTaken)
        {
            this.FoodTaken = foodTaken;
        }

        public readonly long FoodTaken;
    }
}