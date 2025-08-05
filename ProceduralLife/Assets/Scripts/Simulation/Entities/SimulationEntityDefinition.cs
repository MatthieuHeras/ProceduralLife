using ProceduralLife.Simulation.View;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    [CreateAssetMenu(fileName = "NewEntity", menuName = Constants.Editor.PATH_ENTITY + "Entity")]
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
        [field: SerializeField, MinValue(1)]
        public long MaxHunger { get; private set; } = 1000;
        
        // [TODO] Make it a stat
        // How much is deducted from the hunger each second.
        [field: SerializeField, MinValue(0)]
        public long HungerRate { get; private set; } = 10;
        
        // [TODO] Make it a stat
        // How much is deducted from the hunger each second.
        [field: SerializeField, MinValue(1)]
        public uint SightRange { get; private set; } = 3;
    }
}