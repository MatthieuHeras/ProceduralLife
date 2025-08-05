using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    [CreateAssetMenu(fileName = "NewBrain", menuName = Constants.Editor.PATH_ENTITY + "Brain")]
    public class SimulationEntityBrainDefinition : StateMachineDefinition
    {
        [field: SerializeField]
        public List<GoalDefinition> Dangers { get; private set; } = new();
        [field: SerializeField]
        public List<GoalDefinition> Needs { get; private set; } = new();
        [field: SerializeField]
        public List<GoalDefinition> Wants { get; private set; } = new();
    }
}