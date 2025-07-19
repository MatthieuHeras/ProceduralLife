using System.Collections.Generic;

namespace ProceduralLife.Simulation
{
    public record InputMomentData(SimulationMoment NextSimulationMoment, List<AInput> Input) : MomentData(NextSimulationMoment)
    {
        public List<AInput> Input { get; } = Input;
    }
}