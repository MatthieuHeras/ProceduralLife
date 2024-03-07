using MHLib.CommandPattern;

namespace ProceduralLife.Simulation
{
    public abstract class ASimulationCommand : ACommand
    {
        // In milliseconds
        public ulong ExecutionMoment { get; private set; }
        
        public void SetExecutionMoment(ulong executionMoment)
        {
            this.ExecutionMoment = executionMoment;
        }
    }
}