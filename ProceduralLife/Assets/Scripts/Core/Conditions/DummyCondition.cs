namespace ProceduralLife.Conditions
{
    public sealed class DummyCondition : AGameCondition
    {
        public DummyCondition(object parameter, ConditionContext context) : base(parameter, context) { }
        
        protected override bool Check() => true;
        protected override void HookToContext() { }
        protected override void UnhookToContext() { }
    }
}