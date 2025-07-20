using MHLib;
using MHLib.Hexagon;
using ProceduralLife.Conditions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class SimulationEntityBrain : StateMachine
    {
        public SimulationEntityBrain(SimulationEntityBrainDefinition definition, SimulationEntity entity) : base(definition, entity, false)
        {
        }

        protected override AState GetNewState()
        {
            AState GetStateFromBehaviourList(List<GoalDefinition> behaviourList)
            {
                foreach (GoalDefinition behaviour in behaviourList)
                {
                    if (behaviour.Condition.CheckOnce(new ConditionContext()))
                        return behaviour.Behaviour.GetState(this.entity);
                }

                return null;
            }
            
            SimulationEntityBrainDefinition brainDefinition = (SimulationEntityBrainDefinition)this.Definition;
            
            // 1 - If the entity is in danger, it responds to it immediately.
            AState newState = GetStateFromBehaviourList(brainDefinition.Dangers);
            if (newState != null)
                return newState;
            
            // 2 - If it's doing something, it finishes it.
            if (this.state != null)
                return this.state;
            
            // 3 - It satisfies its needs.
            newState = GetStateFromBehaviourList(brainDefinition.Needs);
            if (newState != null)
                return newState;
            
            // 4 - It satisfies its wants.
            newState = GetStateFromBehaviourList(brainDefinition.Wants);
            if (newState != null)
                return newState;
            
            // 5 - It waits.
            // [TODO] Define wait duration, for example through entity definition or global constant.
            return new WaitState(this.entity, 100);
            
            // Old move test
            /*Vector2Int targetTile = SimulationContext.MapData.Tiles.ElementAt(Random.Range(0, SimulationContext.MapData.Tiles.Count)).Key;
            float GetDistance(Vector2Int origin, Vector2Int target) => HexagonHelper.Distance(origin, target);

            List<Vector2Int> path = AStar.GetPath(this.entity.Position, targetTile, GetDistance, GetDistance, SimulationContext.MapData.GetTileNeighbours);
            return new MoveState(this.entity, path);*/
        }
    }
}