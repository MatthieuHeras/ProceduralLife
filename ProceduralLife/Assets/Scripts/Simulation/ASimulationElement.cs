using MHLib.CommandPattern;

namespace ProceduralLife.Simulation
{
    public abstract class ASimulationElement : ACommand
    {
        public void InitBirth(SimulationMoment birthMoment)
        {
            this.BirthMoment = birthMoment;
            this.NextExecutionMoment = birthMoment;

            this.DeathMoment = null;
            this.PreviousExecutionMoment = null;
        }
        
        public SimulationMoment BirthMoment;
        public SimulationMoment DeathMoment;
        
        public SimulationMoment PreviousExecutionMoment;
        public SimulationMoment NextExecutionMoment;
    }
}