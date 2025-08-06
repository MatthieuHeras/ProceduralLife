using UnityEngine.Assertions;

namespace ProceduralLife.Simulation
{
    public class EatState : AState
    {
        public EatState(SimulationEntity entity, SimulationEntity target) : base(entity)
        {
            this.target = target;
        }
        
        private readonly SimulationEntity target;

        public override StateDoData Do()
        {
            // Must eat on self tile.
            Assert.IsTrue(this.target.Position == this.entity.Position);
            
            // [TODO] Move killing to its own state, will work for now.
            SimulationContext.SimulationTime.KillElement(target);

            long foodTaken = System.Math.Min(this.target.Definition.FoodValue, this.entity.Definition.MaxHunger - this.entity.Hunger);
            this.entity.ChangeHunger(foodTaken);
            
            // [TODO] Implement proper eating duration (depending on food taken, self stat, etc.)
            return new StateDoData(new EatStateData(foodTaken), 1000ul, null);
        }

        public override void Undo(AStateData stateData)
        {
            Assert.IsTrue(stateData is EatStateData);
            EatStateData eatStateData = (EatStateData)stateData;
            
            this.entity.ChangeHunger(-eatStateData.FoodTaken);
            
            SimulationContext.SimulationTime.ResurrectElement(this.target);
        }

        public override void Redo(AStateData stateData)
        {
            Assert.IsTrue(stateData is EatStateData);
            EatStateData eatStateData = (EatStateData)stateData;
            
            this.entity.ChangeHunger(eatStateData.FoodTaken);
            
            SimulationContext.SimulationTime.ReKillElement(this.target);
        }
    }
}