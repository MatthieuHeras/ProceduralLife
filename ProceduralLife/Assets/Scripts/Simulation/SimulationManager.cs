using System.Collections.Generic;
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
        private bool startedSimulation = false;

        private bool forward = true;

        private readonly List<SimulationEntity> entities = new();

        public void ChangeTimeScale(float newTimeScale)
        {
            this.timeScale = newTimeScale;
        }
        
        [Button]
        public void AddSheep()
        {
            SimulationEntity entity = new(this.simulationTime.CurrentTime, Vector2Int.zero);
            SimulationEntityView firstEntityView = Instantiate(this.entityView);
            firstEntityView.Init(entity);
            
            this.entities.Add(entity);
            this.simulationTime.InsertUpcomingEntity(entity);
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
            this.forward = false;
        }
        
        [Button]
        public void GoForward()
        {
            Debug.LogWarning("Forward");
            this.forward = true;
        }
    
        private void Start()
        {
            this.startedSimulation = true;
            this.simulationTime = new SimulationTime(this.commandGenerator.MapData);
        }
        
        private void Update()
        {
            if (this.entities.Count == 0)
                return;
            
            ulong deltaTime = (ulong)(Time.deltaTime * this.timeScale * 1000f);
            
            if (this.forward)
                this.simulationTime.IterateForward(deltaTime);
            else
                this.simulationTime.IterateBackward(deltaTime);
        }
    }
}