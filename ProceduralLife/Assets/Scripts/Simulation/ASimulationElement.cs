using MHLib.CommandPattern;

namespace ProceduralLife.Simulation
{
    public abstract class ASimulationElement : ACommand
    {
        public delegate void TimeEvent(bool timeIsForward);
        
        public event TimeEvent BirthEvent = delegate { };
        public event TimeEvent DeathEvent = delegate { };
        
        public void InitBirth(SimulationMoment birthMoment)
        {
            this.BirthMoment = birthMoment;
            this.NextExecutionMoment = birthMoment;

            this.DeathMoment = null;
            this.PreviousExecutionMoment = null;

            this.BirthEvent.Invoke(true);
        }
        
        public void InitDeath(SimulationMoment deathMoment)
        {
            this.DeathMoment = deathMoment;
            this.DeathEvent.Invoke(true);
        }
        
        public SimulationMoment BirthMoment;
        public SimulationMoment DeathMoment;
        
        public SimulationMoment PreviousExecutionMoment;
        public SimulationMoment NextExecutionMoment;
    }
}