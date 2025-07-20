using MHLib.Conditions;

namespace ProceduralLife.Conditions
{
    public abstract class AGameConditionSO<TGameCondition> : AConditionSO<TGameCondition, ConditionContext>
        where TGameCondition : AGameCondition
    {
        
    }
}