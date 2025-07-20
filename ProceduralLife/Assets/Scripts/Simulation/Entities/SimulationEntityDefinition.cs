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
        
        // [TODO] Make it a stat
        // A speed of 1 means it takes 1 second to move 1 tile.
        [field: SerializeField]
        public float Speed { get; private set; } = 1f;
        
        // [TODO] Make it a stat
        // If it reaches 0, the entity dies.
        [field: SerializeField]
        public ulong MaxHunger { get; private set; } = 1000;
        
        // [TODO] Make it a stat
        // How much is deducted from the hunger each second.
        [field: SerializeField]
        public ulong HungerRate { get; private set; } = 10;
    }
}