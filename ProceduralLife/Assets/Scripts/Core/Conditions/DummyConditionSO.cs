using MHLib.Conditions;
using System;
using UnityEngine;

namespace ProceduralLife.Conditions
{
    [CreateAssetMenu(fileName = "Dummy", menuName = Constants.Editor.PATH_CONDITIONS + "Dummy")]
    public class DummyConditionSO : AGameConditionSO
    {
        public override ACondition<ConditionContext> CreateCondition(object parameter, ConditionContext context) => new DummyCondition(parameter, context);

        public override Type ParameterType => typeof(int);
    }
}