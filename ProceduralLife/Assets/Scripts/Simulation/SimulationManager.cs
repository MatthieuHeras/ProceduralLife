using ProceduralLife.MapEditor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class SimulationManager : MonoBehaviour
    {
        // [TODO] Tmp way to access MapData, will need proper way later on.
        [SerializeField, Required]
        private MapEditorCommandGenerator commandGenerator;

        [SerializeField]
        private float timeScale = 1f;
        
        private SimulationTime simulationTime;
        private bool startedSimulation = false;

        private bool forward = true;
        
        [Button]
        private void StartSimulation()
        {
            this.startedSimulation = true;
            this.simulationTime = new SimulationTime(this.commandGenerator.MapData);
            
            this.simulationTime.InsertUpcomingEntity(new SimulationEntity(Vector2Int.zero));
        }

        [Button]
        private void GoBackward()
        {
            Debug.LogWarning("Backward");
            this.forward = false;
        }

        [Button]
        private void GoForward()
        {
            Debug.LogWarning("Forward");
            this.forward = true;
        }

        private void Update()
        {
            if (!this.startedSimulation)
                return;

            ulong deltaTime = (ulong)(Time.deltaTime * this.timeScale * 1000f);
            
            if (this.forward)
                this.simulationTime.IterateForward(deltaTime);
            else
                this.simulationTime.IterateBackward(deltaTime);
        }
    }
}