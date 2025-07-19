using ProceduralLife.Inputs;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class InputSimulationElement : ASimulationElement<InputMomentData>
    {
        public InputSimulationElement()
        {
            InputManager.SpawnEntityEvent += this.OnEntitySpawned;
        }
        
        ~InputSimulationElement()
        {
            InputManager.SpawnEntityEvent -= this.OnEntitySpawned;
        }

        private void OnEntitySpawned(SimulationEntityDefinition entityDefinition, Vector2Int spawnPosition) => this.waitingInputs.Add(new SpawnEntityInput(this, entityDefinition, spawnPosition));
        
        private readonly List<AInput> waitingInputs = new();
        
        protected override InputMomentData ApplyDo()
        {
            // We delay the inputs to the next frame
            SimulationMoment nextMoment = SimulationContext.SimulationTime.DelayElement(this, (SimulationContext.SimulationTime.CurrentTime + 1ul) - this.NextExecutionMoment.Time);
            
            List<AInput> inputs = new(this.waitingInputs);
            this.waitingInputs.Clear();
            
            foreach (AInput input in inputs)
                input.Do();
            
            return new InputMomentData(nextMoment, inputs);
        }
        
        protected override void ApplyUndo(InputMomentData momentData)
        {
            foreach (AInput input in momentData.Input)
                input.Undo();
        }

        protected override void ApplyRedo(InputMomentData momentData)
        {
            foreach (AInput input in momentData.Input)
                input.Redo();
        }
    }
}