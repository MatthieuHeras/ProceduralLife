using System;
using UnityEngine;

namespace ProceduralLife.Conditions
{
    [CreateAssetMenu(fileName = "Dummy", menuName = Constants.Editor.PATH_CONDITIONS + "Dummy")]
    public class DummyConditionSO : AGameConditionSO<DummyCondition>
    {
        public override DummyCondition CreateCondition(object parameter, ConditionContext context) => new(parameter, context);

        public override Type ParameterType => typeof(int);
    }
}