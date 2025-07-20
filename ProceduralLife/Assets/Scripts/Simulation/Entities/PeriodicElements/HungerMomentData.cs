using System.Collections.Generic;

namespace ProceduralLife.Simulation.PeriodicElements
{
    public record HungerMomentData(SimulationMoment NextSimulationMoment, List<ASimulationElementBase> KilledElements) : MomentData(NextSimulationMoment)
    {
        public List<ASimulationElementBase> KilledElements { get; } = KilledElements;
    }
}