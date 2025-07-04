using UnityEngine;

namespace ProceduralLife.Simulation
{
    [CreateAssetMenu(fileName = "NewEntity", menuName = "PL/Entity/Entity")]
    public class SimulationEntityDefinition : ScriptableObject
    {
        [field: SerializeField]
        public SimulationEntityBrainDefinition BrainDefinition;
    }
}