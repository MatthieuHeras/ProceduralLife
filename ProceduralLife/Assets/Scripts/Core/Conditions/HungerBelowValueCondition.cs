namespace ProceduralLife.Conditions
{
    public class HungerBelowValueCondition : AGameCondition
    {
        public HungerBelowValueCondition(long  parameter, ConditionContext context) : base(parameter, context) { }
        
        protected override bool Check()
        {
            return this.context.Entity.Hunger < (long)this.parameter;
        }

        protected override void HookToContext()
        {
            this.context.Entity.HungerChanged += this.UpdateValue;
        }

        protected override void UnhookToContext()
        {
            this.context.Entity.HungerChanged -= this.UpdateValue;
        }
    }
}