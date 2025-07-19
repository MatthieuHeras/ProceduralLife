using ProceduralLife.Simulation.View;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    [CreateAssetMenu(fileName = "NewEntity", menuName = "PL/Entity/Entity")]
    public class SimulationEntityDefinition : ScriptableObject
    {
        [field: SerializeField]
        public SimulationEntityView View { get; private set; }
        
        [field: SerializeField]
        public SimulationEntityBrainDefinition BrainDefinition { get; private set; }
        
        [field: SerializeField]
        public ulong Speed { get; private set; }
    }
}