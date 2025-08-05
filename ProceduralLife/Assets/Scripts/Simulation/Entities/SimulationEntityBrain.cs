using MHLib;
using MHLib.Hexagon;
using ProceduralLife.Conditions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class SimulationEntityBrain : AStateMachine
    {
        public SimulationEntityBrain(SimulationEntityBrainDefinition definition, SimulationEntity entity) : base(entity, false)
        {
            this.definition = definition;
        }

        private readonly SimulationEntityBrainDefinition definition;

        private bool dealingWithDanger = false;
        
        protected override AState GetNewState()
        {
            AState GetStateFromBehaviourList(List<GoalDefinition> behaviourList, bool isDanger = false)
            {
                foreach (GoalDefinition behaviour in behaviourList)
                {
                    if (behaviour.Condition.CheckOnce(new ConditionContext(this.entity)))
                    {
                        if (isDanger)
                            this.dealingWithDanger = true;
                        return behaviour.Behaviour.GetBehaviour(new BehaviourContext(this.entity));
                    }
                }
                
                return null;
            }
            
            // 1- If the entity is already dealing with something of high priority, it keeps doing so at all costs.
            if (this.dealingWithDanger)
            {
                if (this.state != null)
                    return this.state;

                this.dealingWithDanger = false;
            }
            
            // 2 - If it is in danger, it responds to it immediately.
            AState newState = GetStateFromBehaviourList(this.definition.Dangers, true);
            if (newState != null)
                return newState;
            
            // 3 - If it is doing something of lower priority, it finishes it.
            if (this.state != null)
                return this.state;
            
            // 4 - It satisfies its needs.
            newState = GetStateFromBehaviourList(this.definition.Needs);
            if (newState != null)
                return newState;
            
            // 5 - It satisfies its wants.
            newState = GetStateFromBehaviourList(this.definition.Wants);
            if (newState != null)
                return newState;
            
            // 6 - It waits. This should never occur in a properly integrated AI and is more of a safety thing.
            // Feel free to integrate wandering or waiting as something the entity wants, with low priority.
            //return new WaitState(this.entity, Constants.Simulation.DEFAULT_WAIT_DURATION);
            
            // Old move test
            Vector2Int targetTile = SimulationContext.MapData.Tiles.ElementAt(Random.Range(0, SimulationContext.MapData.Tiles.Count)).Key;
            float GetDistance(Vector2Int origin, Vector2Int target) => HexagonHelper.Distance(origin, target);

            List<Vector2Int> path = AStar.GetPath(this.entity.Position, targetTile, GetDistance, GetDistance, SimulationContext.MapData.GetTileNeighbours);
            return new MoveState(this.entity, path);
        }
    }
}