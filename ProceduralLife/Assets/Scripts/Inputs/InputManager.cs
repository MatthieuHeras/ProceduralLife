using ProceduralLife.Simulation;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ProceduralLife.Inputs
{
    public class InputManager : MonoBehaviour
    {
        // [TODO] Proper spawn/place
        [SerializeField]
        private SimulationEntityDefinition testEntity;

        public static event Action SimulationChanged = delegate { };
        public static event Action<SimulationEntityDefinition, Vector2Int> SpawnEntityEvent = delegate { };
        
        [Button]
        public void AddSheepButton()
        {
            // [TODO] Proper spawn/place
            SpawnEntityEvent.Invoke(this.testEntity, new Vector2Int(0, 0));
            
            SimulationChanged.Invoke();
        }
        
        [Button]
        public void Add100SheepButton()
        {
            for (int i = 0; i < 100; i++)
                SpawnEntityEvent.Invoke(this.testEntity, new Vector2Int(0, 0));
            
            SimulationChanged.Invoke();
        }
    }
}