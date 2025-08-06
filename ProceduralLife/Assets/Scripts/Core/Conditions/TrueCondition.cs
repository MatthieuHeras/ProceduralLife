namespace ProceduralLife.Conditions
{
    public class TrueCondition : AGameCondition
    {
        public TrueCondition(object parameter, ConditionContext context) : base(parameter, context) { }
        
        protected override bool Check() => true;
        protected override void HookToContext() { }
        protected override void UnhookToContext() { }
    }
}