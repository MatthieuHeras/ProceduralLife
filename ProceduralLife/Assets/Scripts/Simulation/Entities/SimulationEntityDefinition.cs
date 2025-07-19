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
        
        // [TODO] Make a stat out of this, we might not want all entities to have speed
        // A speed of 1 means it takes 1 second to move 1 tile.
        [field: SerializeField]
        public float Speed { get; private set; } = 1f;
    }
}