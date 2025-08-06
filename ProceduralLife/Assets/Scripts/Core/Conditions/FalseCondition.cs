namespace ProceduralLife.Conditions
{
    public class FalseCondition : AGameCondition
    {
        public FalseCondition(object parameter, ConditionContext context) : base(parameter, context) { }
        
        protected override bool Check() => false;
        protected override void HookToContext() { }
        protected override void UnhookToContext() { }
    }
}