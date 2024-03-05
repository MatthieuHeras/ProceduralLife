using MHLib.CommandPattern;

namespace ProceduralLife.Simulation
{
    public abstract class ASimulationCommand : ACommand
    {
        protected ASimulationCommand(ulong executionMoment)
        {
            this.ExecutionMoment = executionMoment;
        }

        // In milliseconds
        public readonly ulong ExecutionMoment;
    }
}