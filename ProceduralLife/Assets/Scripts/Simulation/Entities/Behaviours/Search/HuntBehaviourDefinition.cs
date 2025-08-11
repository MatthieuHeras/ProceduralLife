using System;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    [CreateAssetMenu(fileName = "Hunt", menuName = Constants.Editor.PATH_BEHAVIOURS + "Hunt")]
    public class HuntBehaviourDefinition : ABehaviourDefinition
    {
        public override Type ParameterType => typeof(HuntBehaviourParameter);
        
        public override ABehaviour GetBehaviour(BehaviourContext context, object parameter) => new HuntBehaviour(context, (HuntBehaviourParameter)parameter);
    }
}