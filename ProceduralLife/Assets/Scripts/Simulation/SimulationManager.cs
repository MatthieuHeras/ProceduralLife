using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class SimulationManager : MonoBehaviour
    {
        [SerializeField]
        private float timeScale = 1f;
        
        private SimulationTime simulationTime;
        
        public void ChangeTimeScale(float newTimeScale)
        {
            this.timeScale = newTimeScale;
        }
        
        [Button]
        public void GoBackward()
        {
            this.simulationTime.Backward();
        }
        
        [Button]
        public void GoForward()
        {
            this.simulationTime.Forward();
        }
    
        private void Start()
        {
            this.simulationTime = new SimulationTime();
        }
        
        private void Update()
        {
            ulong deltaTime = (ulong)(Time.deltaTime * this.timeScale * 1000f);
            
            this.simulationTime.Iterate(deltaTime);
        }
    }
}