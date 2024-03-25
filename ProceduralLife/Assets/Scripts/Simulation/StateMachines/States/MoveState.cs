using MHLib;
using MHLib.Hexagon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class MoveState : AState
    {
        public MoveState(SimulationEntity entity, Vector2Int targetPosition)
            : base(entity)
        {
            this.path = AStar.GetPath(this.entity.Position, targetPosition, this.GetDistance, this.GetDistance, this.entity.MapData.GetTileNeighbours);
        }
        
        private readonly List<Vector2Int> path;
        private int currentPathIndex = 0;

        public bool isMoving = false;
        
        public override StateDoData Do()
        {
            if (!this.isMoving)
            {
                this.entity.MoveStart(this.path[this.currentPathIndex], this.entity.NextExecutionMoment.Time, 1000, true);
            }
            else
            {
                this.entity.MoveEnd(this.path[this.currentPathIndex]);
                this.currentPathIndex++;
            }
            
            AState nextState = this.isMoving && this.currentPathIndex == this.path.Count
                               ? new MoveState(this.entity, this.entity.MapData.Tiles.ElementAt(Random.Range(0, this.entity.MapData.Tiles.Count)).Key)
                               : this;

            this.isMoving = !this.isMoving;
            
            return new StateDoData(new MoveStateData(!this.isMoving), this.isMoving ? 1000ul : 0ul, nextState);
        }
        
        public override void Undo(AStateData stateData)
        {
            throw new System.NotImplementedException();
        }
        
        public override void Redo(AStateData stateData)
        {
            throw new System.NotImplementedException();
        }
        
        private float GetDistance(Vector2Int origin, Vector2Int target) => HexagonHelper.Distance(origin, target);
    }
}