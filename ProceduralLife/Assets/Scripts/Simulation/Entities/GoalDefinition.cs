using ProceduralLife.Conditions;
using System;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    [Serializable]
    public class GoalDefinition
    {
        [field: SerializeField]
        public GameConditionField Condition { get; private set; }
        // [TODO] Implement behaviours
        [field: SerializeField]
        public BehaviourDefinition Behaviour { get; private set; }
    }
}