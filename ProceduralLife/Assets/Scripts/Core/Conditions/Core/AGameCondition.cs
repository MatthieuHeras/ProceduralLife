using MHLib.Conditions;

namespace ProceduralLife.Conditions
{
    public abstract class AGameCondition : ACondition<ConditionContext>
    {
        protected AGameCondition(object parameter, ConditionContext context) : base(parameter, context)
        {
        }
    }
}