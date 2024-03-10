using MHLib.CommandPattern;

namespace ProceduralLife.Simulation
{
    public abstract class ASimulationCommand : ACommand
    {
        protected ASimulationCommand(ASimulationElement responsibleElement)
        {
            this.ResponsibleElement = responsibleElement;
        }
        
        public readonly ASimulationElement ResponsibleElement;
        
        // In milliseconds
        public ulong ExecutionMoment { get; private set; }
        
        public void SetExecutionMoment(ulong executionMoment)
        {
            this.ExecutionMoment = executionMoment;
        }
    }
}