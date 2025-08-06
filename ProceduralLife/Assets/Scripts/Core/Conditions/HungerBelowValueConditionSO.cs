using MHLib.Conditions;
using MHLib.ConfigurableSO;
using System;
using UnityEngine;

namespace ProceduralLife.Conditions
{
    [CreateAssetMenu(fileName = "HungerBelowValue", menuName = Constants.Editor.PATH_CONDITIONS + "HungerBelowValue")]
    public class HungerBelowValueConditionSO : AGameConditionSO
    {
        public override ACondition<ConditionContext> CreateCondition(object parameter, ConditionContext context) => new HungerBelowValueCondition(((LongWrapper)parameter).Value, context);
        
        public override Type ParameterType => typeof(LongWrapper);
    }
}