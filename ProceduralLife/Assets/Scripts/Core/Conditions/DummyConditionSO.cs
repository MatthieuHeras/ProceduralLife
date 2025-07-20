using System;
using UnityEngine;

namespace ProceduralLife.Conditions
{
    [CreateAssetMenu(fileName = "DummyCondition", menuName = Constants.Editor.PATH_CONDITION + "DummyCondition")]
    public class DummyConditionSO : AGameConditionSO<DummyCondition>
    {
        public override DummyCondition CreateCondition(object parameter, ConditionContext context) => new(parameter, context);
        
        public override Type ParameterType { get; }
    }
}