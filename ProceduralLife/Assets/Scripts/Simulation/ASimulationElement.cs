using System.Collections.Generic;

namespace ProceduralLife.Simulation
{
    public abstract class ASimulationElement<TMomentData> : ASimulationElementBase
        where TMomentData : MomentData
    {
        private readonly List<TMomentData> momentsData = new();
        private int currentIndex = -1;
        
        public override void Do()
        {
            // Break future data, should happen when we break the replay to start a new timeline
            if (this.currentIndex < this.momentsData.Count - 1)
            {
                int nextIndex = this.currentIndex + 1;
                this.momentsData.RemoveRange(nextIndex, this.momentsData.Count - nextIndex);
            }
            
            SimulationMoment currentMoment = this.currentIndex == -1 ? this.birthMoment : this.momentsData[this.currentIndex].NextSimulationMoment;
            
            TMomentData newMomentData = this.ApplyDo();
            this.momentsData.Add(newMomentData);
            this.currentIndex++;
            
            this.PreviousExecutionMoment = currentMoment;
            this.NextExecutionMoment = newMomentData.NextSimulationMoment; 
        }
        
        public override void Undo()
        {
            this.ApplyUndo(this.momentsData[this.currentIndex]);
            
            this.currentIndex--;
            
            this.PreviousExecutionMoment = this.currentIndex switch
            {
                -1 => new SimulationMoment(0, 0),
                0 => this.birthMoment,
                _ => this.momentsData[this.currentIndex - 1].NextSimulationMoment
            };
            
            this.NextExecutionMoment = this.currentIndex == -1 ? this.birthMoment : this.momentsData[this.currentIndex].NextSimulationMoment;
        }
        
        public override void Redo()
        {
            SimulationMoment currentMoment = this.currentIndex == -1 ? this.birthMoment : this.momentsData[this.currentIndex].NextSimulationMoment;
            
            this.currentIndex++;
            
            this.ApplyRedo(this.momentsData[this.currentIndex]);

            this.PreviousExecutionMoment = currentMoment;
            this.NextExecutionMoment = this.momentsData[this.currentIndex].NextSimulationMoment;
        }
        
        protected abstract TMomentData ApplyDo();
        protected abstract void ApplyUndo(TMomentData momentData);
        protected abstract void ApplyRedo(TMomentData momentData);
    }
}