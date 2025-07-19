using MHLib.CommandPattern;

namespace ProceduralLife.Simulation
{
    public abstract class AInput : ACommand
    {
        protected AInput(InputSimulationElement inputElement)
        {
            this.inputElement = inputElement;
        }

        protected readonly InputSimulationElement inputElement;
    }
}