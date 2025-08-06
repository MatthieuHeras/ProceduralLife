using MHLib;
using ProceduralLife.Map;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralLife.Simulation
{
    public class SearchBehaviour : ABehaviour
    {
        public SearchBehaviour(BehaviourContext context, SearchBehaviourParameter parameter) : base(context)
        {
            this.parameter = parameter;
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
            
            if (searchState == SearchState.LOOKING && this.TryFindTarget())
                searchState = SearchState.CHASING;
            
            switch (this.searchState)
            {
                case SearchState.LOOKING:
                    Vector2Int[] neighbours = SimulationContext.MapData.GetTileNeighbours(this.entity.Position);
                    return new MoveState(this.entity, new List<Vector2Int>(2) {this.entity.Position, neighbours[Random.Range(0, neighbours.Length)]});
                case SearchState.CHASING:
                    return new MoveState(this.entity, this.pathToTarget);
            }
            
            Assert.IsTrue(0 == 1);
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
                        this.target = tileEntity;
                        return true;
                    }
                }

                return false;
            }
            
            this.pathToTarget = Dijkstra.GetClosestNode(this.entity.Position, (_, _) => 1f, (node) => map.GetTileNeighbours(node.Node), HasTarget, this.entity.SightRange);
            
            return this.pathToTarget.Count > 0;
        }

        protected override bool ShouldStop()
        {
            // Has finished eating.
            return base.ShouldStop() && this.searchState == SearchState.EATING;
        }
    }
}