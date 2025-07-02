using ProceduralLife.MapEditor;
using ProceduralLife.Simulation.View;
using Sirenix.OdinInspector;
using System;
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

        // [TODO] Proper spawn/place
        [SerializeField]
        private SimulationEntityDefinition testEntity;
        
        private SimulationTime simulationTime;
        
        public static event Action SimulationChanged = delegate { };
        
        public void ChangeTimeScale(float newTimeScale)
        {
            this.timeScale = newTimeScale;
        }

        private void AddSheep()
        {
            SimulationEntity entity = new(this.testEntity, this.commandGenerator.MapData);
            SimulationEntityView firstEntityView = Instantiate(this.entityView);
            firstEntityView.Init(entity);
            
            this.simulationTime.InsertElement(entity);
        }
        
        [Button]
        public void AddSheepButton()
        {
            this.AddSheep();
            SimulationChanged.Invoke();
        }
        
        [Button]
        public void Add100SheepButton()
        {
            for (int i = 0; i < 100; i++)
                this.AddSheepButton();
            
            SimulationChanged.Invoke();
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