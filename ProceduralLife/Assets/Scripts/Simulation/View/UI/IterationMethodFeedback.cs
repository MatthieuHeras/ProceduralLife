namespace ProceduralLife.Simulation.View
{
    using Sirenix.OdinInspector;
    using System;
    using TMPro;
    using UnityEngine;

    public class IterationMethodFeedback : MonoBehaviour
    {
        [SerializeField, Required]
        private TextMeshProUGUI text;
        
        private void OnIterationMethodChanged(E_IterationMethodType newIterationMethod)
        {
            string feedback = newIterationMethod switch
            {
                E_IterationMethodType.PLAY => "Playing ...",
                E_IterationMethodType.BACKWARD => "Backward ...",
                E_IterationMethodType.REPLAY => "Replaying ...",
                _ => throw new ArgumentOutOfRangeException(nameof(newIterationMethod), newIterationMethod, null)
            };

            this.text.text = feedback;
        }
        
        private void OnEnable()
        {
            SimulationTime.IterationMethodChanged += this.OnIterationMethodChanged;
        }
        
        private void OnDisable()
        {
            SimulationTime.IterationMethodChanged -= this.OnIterationMethodChanged;
        }
    }
}