using ProceduralLife.MapEditor;
using ProceduralLife.Simulation.View;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class SimulationManager : MonoBehaviour
    {
        // [TODO] Tmp way to access MapData, will need proper way later on.
        [SerializeField, Required]
        private MapEditorCommandGenerator commandGenerator;
        
        [SerializeField, Required]
        private SimulationEntityView entityView;
        
        [SerializeField]
        private float timeScale = 1f;
        
        private SimulationTime simulationTime;
        
        public void ChangeTimeScale(float newTimeScale)
        {
            this.timeScale = newTimeScale;
        }
        
        [Button]
        public void AddSheep()
        {
            SimulationEntity entity = new(this.commandGenerator.MapData);
            SimulationEntityView firstEntityView = Instantiate(this.entityView);
            firstEntityView.Init(entity);
            
            this.simulationTime.InsertElement(entity);
        }
        
        [Button]
        public void Add100Sheep()
        {
            for (int i = 0; i < 100; i++)
                this.AddSheep();
        }
        
        [Button]
        public void GoBackward()
        {
            Debug.LogWarning("Backward");
            this.simulationTime.Backward();
        }
        
        [Button]
        public void GoForward()
        {
            Debug.LogWarning("Forward");
            this.simulationTime.Forward();
        }
    
        private void Start()
        {
            this.simulationTime = new SimulationTime(this.commandGenerator.MapData);
        }
        
        private void Update()
        {
            ulong deltaTime = (ulong)(Time.deltaTime * this.timeScale * 1000f);
            
            this.simulationTime.Iterate(deltaTime);
        }
    }
}