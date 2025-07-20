namespace ProceduralLife.Conditions
{
    public class DummyCondition : AGameCondition
    {
        public DummyCondition(object parameter, ConditionContext context) : base(parameter, context)
        {
        }

        protected override bool Check()
        {
            throw new System.NotImplementedException();
        }

        protected override void HookToContext()
        {
            throw new System.NotImplementedException();
        }

        protected override void UnhookToContext()
        {
            throw new System.NotImplementedException();
        }
    }
}