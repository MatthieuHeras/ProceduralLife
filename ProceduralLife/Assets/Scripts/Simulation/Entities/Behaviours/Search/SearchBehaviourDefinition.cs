using System;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    [CreateAssetMenu(fileName = "Search", menuName = Constants.Editor.PATH_BEHAVIOURS + "Search")]
    public class SearchBehaviourDefinition : ABehaviourDefinition
    {
        public override Type ParameterType => typeof(SearchBehaviourParameter);

        public override ABehaviour GetBehaviour(BehaviourContext context, object parameter) => new SearchBehaviour(context, parameter as SearchBehaviourParameter);
    }
}