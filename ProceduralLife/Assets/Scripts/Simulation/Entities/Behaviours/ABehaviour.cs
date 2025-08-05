namespace ProceduralLife.Simulation
{
    public abstract class ABehaviour : AStateMachine
    {
        protected ABehaviour(BehaviourContext context) : base(context.Entity, true) { }
    }
}