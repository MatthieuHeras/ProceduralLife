namespace ProceduralLife.Simulation
{
    public class WaitBehaviour : ABehaviour
    {
        public WaitBehaviour(BehaviourContext context, ulong parameter) : base(context)
        {
            this.parameter = parameter;
        }
        
        private readonly ulong parameter;

        protected override AState GetNewState() => new WaitState(this.entity, this.parameter);
    }
}