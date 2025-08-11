namespace ProceduralLife.Simulation
{
    public class HuntBehaviour : ASearchBehaviour
    {
        public HuntBehaviour(BehaviourContext context, HuntBehaviourParameter parameter) : base(context, true)
        {
            this.parameter = parameter;
        }

        private readonly HuntBehaviourParameter parameter;
        
        protected override AState GetInteractionState() => new EatState(this.entity, this.target);
        protected override bool IsTargetValid(SimulationEntity targetEntity) => targetEntity.Definition.Type == this.parameter.EntityType;
    }
}