using MHLib.CommandPattern;

namespace ProceduralLife.Simulation
{
    public abstract class ASimulationElement : ACommand
    {
        public delegate void TimeEvent(bool timeIsForward);
        
        public event TimeEvent BirthEvent = delegate { };
        public event TimeEvent DeathEvent = delegate { };

        public void InitBirthMoment(SimulationMoment birthMoment)
        {
            this.NextExecutionMoment = birthMoment;
            this.PreviousExecutionMoment = null;
        }
        
        public void ReachBirthMoment(bool timeIsForward)
        {
            this.BirthEvent.Invoke(timeIsForward);
        }
        
        public void ReachDeathMoment(bool timeIsForward)
        {
            this.DeathEvent.Invoke(timeIsForward);
        }
        
        public void UnBirth() => this.BirthEvent.Invoke(false);
        public void UnKill() => this.DeathEvent.Invoke(false);
        
        public SimulationMoment PreviousExecutionMoment;
        public SimulationMoment NextExecutionMoment;
    }
}