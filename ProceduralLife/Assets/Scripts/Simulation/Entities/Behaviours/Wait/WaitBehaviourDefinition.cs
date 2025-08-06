using System;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    [CreateAssetMenu(fileName = "Wait", menuName = Constants.Editor.PATH_BEHAVIOURS + "Wait")]
    public class WaitBehaviourDefinition : ABehaviourDefinition
    {
        public override Type ParameterType => typeof(ulong);

        public override ABehaviour GetBehaviour(BehaviourContext context, object parameter) => new WaitBehaviour(context, (ulong)parameter);
    }
}