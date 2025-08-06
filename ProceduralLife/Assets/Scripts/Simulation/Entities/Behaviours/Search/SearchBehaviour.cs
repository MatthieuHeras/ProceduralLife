using MHLib;
using ProceduralLife.Map;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class SearchBehaviour : ABehaviour
    {
        public SearchBehaviour(BehaviourContext context, SearchBehaviourParameter parameter) : base(context)
        {
            this.parameter = parameter;
            this.entity.DeathEvent += this.OnSelfDeath;
        }
        
        private enum SearchState
        {
            LOOKING,
            CHASING,
            EATING
        }
        
        private readonly SearchBehaviourParameter parameter;
        private SearchState searchState = SearchState.LOOKING;
        private SimulationEntity target = null;
        private List<Vector2Int> pathToTarget = null;
        
        protected override AState GetNewState()
        {
            if (this.state != null)
                return this.state;
            
            // Check if we have reached our current goal
            switch (this.searchState)
            {
                case SearchState.LOOKING:
                    if (this.TryFindTarget())
                        searchState = SearchState.CHASING;
                    break;
                case SearchState.CHASING:
                    if (this.target.Position == this.entity.Position)
                        searchState = SearchState.EATING;
                    break;
                default:
                    Debug.LogError($"Reached unsupported code flow, current search state: {this.searchState}");
                    return null;
            }
            
            switch (this.searchState)
            {
                case SearchState.LOOKING:
                    Vector2Int[] neighbours = SimulationContext.MapData.GetTileNeighbours(this.entity.Position);
                    return new MoveState(this.entity, new List<Vector2Int>(2) {this.entity.Position, neighbours[Random.Range(0, neighbours.Length)]});
                case SearchState.CHASING:
                    return new MoveState(this.entity, this.pathToTarget);
                case SearchState.EATING:
                {
                    this.target.DeathEvent -= this.OnTargetDeath;
                    return new EatState(this.entity, this.target);
                }
            }
            
            Debug.LogError($"Reached unsupported code flow, current search state: {this.searchState}");
            return null;
        }

        private bool TryFindTarget()
        {
            MapData map = SimulationContext.MapData;
            
            bool HasTarget(DijkstraNode<Vector2Int> node)
            {
                foreach (SimulationEntity tileEntity in map.Tiles[node.Node].Entities)
                {
                    if (tileEntity != this.entity && tileEntity.Definition.Type == this.parameter.EntityType)
                    {
                        this.SetTarget(tileEntity);
                        return true;
                    }
                }

                return false;
            }
            
            this.pathToTarget = Dijkstra.GetClosestNode(this.entity.Position, (_, _) => 1f, (node) => map.GetTileNeighbours(node.Node), HasTarget, this.entity.SightRange);
            
            return this.pathToTarget.Count > 0;
        }

        private void SetTarget(SimulationEntity newTarget)
        {
            this.target = newTarget;
            newTarget.DeathEvent += this.OnTargetDeath;
        }

        private void OnTargetDeath(bool timeIsForward)
        {
            this.target.DeathEvent -= this.OnTargetDeath;
            this.target = null;
            this.pathToTarget = null;
            this.searchState = SearchState.LOOKING;
        }

        private void OnSelfDeath(bool timeIsForward)
        {
            if (this.target != null)
            {
                if (timeIsForward)
                    this.target.DeathEvent -= this.OnTargetDeath;
                else
                    this.target.DeathEvent += this.OnTargetDeath;
            }
        }

        protected override bool ShouldStop()
        {
            // Has finished eating.
            return base.ShouldStop() && this.searchState == SearchState.EATING;
        }
    }
}