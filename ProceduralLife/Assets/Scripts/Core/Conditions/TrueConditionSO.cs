using MHLib.Conditions;
using System;
using UnityEngine;

namespace ProceduralLife.Conditions
{
    [CreateAssetMenu(fileName = "True", menuName = Constants.Editor.PATH_CONDITIONS + "True")]
    public class TrueConditionSO : AGameConditionSO
    {
        public override ACondition<ConditionContext> CreateCondition(object parameter, ConditionContext context) => new TrueCondition(parameter, context);
        
        public override Type ParameterType => null;
    }
}