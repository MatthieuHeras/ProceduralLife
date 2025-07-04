using System;

namespace ProceduralLife.Simulation
{
    [Serializable]
    public class GoalDefinition
    {
        // [TODO] Implement conditions
        public bool Condition;
        public BehaviourDefinition Behaviour;
    }
}