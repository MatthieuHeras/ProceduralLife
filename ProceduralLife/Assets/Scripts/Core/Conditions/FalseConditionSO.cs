using MHLib.Conditions;
using System;
using UnityEngine;

namespace ProceduralLife.Conditions
{
    [CreateAssetMenu(fileName = "False", menuName = Constants.Editor.PATH_CONDITIONS + "False")]
    public class FalseConditionSO : AGameConditionSO
    {
        public override ACondition<ConditionContext> CreateCondition(object parameter, ConditionContext context) => new FalseCondition(parameter, context);
        
        public override Type ParameterType => null;
    }
}