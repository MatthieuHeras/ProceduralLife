using System.Collections.Generic;
using UnityEngine.Assertions;

namespace ProceduralLife.Simulation.PeriodicElements
{
    /// <summary> Deduces hunger periodically for each element and kills them if necessary. </summary>
    public class HungerSimulationElement : APeriodicSimulationElement<HungerMomentData>
    {
        public HungerSimulationElement(ulong period) : base(period)
        {
            time = SimulationContext.SimulationTime;
        }
        
        private readonly SimulationTime time; 
        
        protected override HungerMomentData ApplyDo()
        {
            List<ASimulationElementBase> killedElements = new();
            
            // Reverse loop because we may remove elements while looping
            for (int i = this.time.AliveElements.Count - 1; i >= 0; i--)
            {
                ASimulationElementBase element = this.time.AliveElements[i];
                if (element is SimulationEntity entity)
                {
                    // Do not clamp at 0, easier to revive them with the proper hunger value when undoing 
                    entity.Hunger -= entity.Definition.HungerRate;
                    
                    if (entity.Hunger <= 0)
                        killedElements.Add(this.time.KillElement(entity));
                }
            }
            
            return new HungerMomentData(this.time.DelayElement(this, this.period), killedElements);
        }

        protected override void ApplyUndo(HungerMomentData momentData)
        {
            foreach (ASimulationElementBase killedElement in momentData.KilledElements)
                this.time.ResurrectElement(killedElement);

            foreach (ASimulationElementBase element in this.time.AliveElements)
            {
                if (element is SimulationEntity entity)
                {
                    entity.Hunger += entity.Definition.HungerRate;
                    Assert.IsTrue(entity.Hunger <= entity.Definition.MaxHunger);
                }
            }
        }

        protected override void ApplyRedo(HungerMomentData momentData)
        {
            foreach (ASimulationElementBase element in this.time.AliveElements)
            {
                if (element is SimulationEntity entity)
                {
                    entity.Hunger -= entity.Definition.HungerRate;
                    Assert.IsTrue(entity.Hunger > 0 || momentData.KilledElements.Contains(element));
                }
            }

            foreach (ASimulationElementBase killedElement in momentData.KilledElements)
                this.time.ReKillElement(killedElement);
        }
    }
}