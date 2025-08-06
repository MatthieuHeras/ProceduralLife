using MHLib.CommandPattern;

namespace ProceduralLife.Simulation
{
    public abstract class ASimulationElementBase : ACommand
    {
        public delegate void TimeEvent(bool timeIsForward);
        
        public event TimeEvent BirthEvent = delegate { };
        public event TimeEvent DeathEvent = delegate { };
        
        protected SimulationMoment birthMoment;
        public SimulationMoment PreviousExecutionMoment { get; protected set; }
        public SimulationMoment NextExecutionMoment { get; protected set; }
        
        public void InitBirthMoment(SimulationMoment newBirthMoment)
        {
            this.birthMoment = newBirthMoment;
            this.NextExecutionMoment = newBirthMoment;
            this.PreviousExecutionMoment = null;
        }
        
        public virtual void ReachBirthMoment(bool timeIsForward)
        {
            this.BirthEvent.Invoke(timeIsForward);
        }
        
        public virtual void ReachDeathMoment(bool timeIsForward)
        {
            this.DeathEvent.Invoke(timeIsForward);
        }
    }
}