namespace ProceduralLife.Simulation
{
    public class WaitBehaviour : ABehaviour
    {
        public WaitBehaviour(BehaviourContext context, WaitBehaviourParameter parameter) : base(context)
        {
            this.parameter = parameter;
        }
        
        private readonly WaitBehaviourParameter parameter;

        protected override AState GetNewState() => new WaitState(this.entity, this.parameter.Duration);
    }
}