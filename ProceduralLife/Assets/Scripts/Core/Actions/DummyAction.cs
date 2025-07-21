namespace ProceduralLife.Actions
{
    public sealed class DummyAction : AGameAction
    {
        public override ActionResult Trigger(ActionContext context, ActionTarget target) => new();
    }
}