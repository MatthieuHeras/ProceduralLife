using MHLib;
using MHLib.Hexagon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

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
        private int currentPathIndex = 1;

        public bool isMoving = false;
        
        public override StateDoData Do()
        {
            if (this.isMoving)
            {
                this.entity.MoveEnd(this.path[this.currentPathIndex]);
                this.currentPathIndex++;
                this.isMoving = false;
            }
            else if (this.currentPathIndex < this.path.Count) // Safety for empty path (one tile, being self)
            {
                this.entity.MoveStart(this.path[this.currentPathIndex], this.entity.NextExecutionMoment.Time, 1000, true);
                this.isMoving = true;
            }
            
            AState nextState = !this.isMoving && this.currentPathIndex == this.path.Count
                               ? new MoveState(this.entity, this.entity.MapData.Tiles.ElementAt(Random.Range(0, this.entity.MapData.Tiles.Count)).Key)
                               : this;
            
            return new StateDoData(new MoveStateData(!this.isMoving), this.isMoving ? 1000ul : 100ul, nextState);
        }
        
        public override void Undo(AStateData stateData)
        {
            Assert.IsTrue(stateData is MoveStateData);
            Assert.IsTrue(this.currentPathIndex > 0);
            
            if (this.isMoving)
            {
                this.entity.MoveEnd(this.path[this.currentPathIndex - 1]);
            }
            else
            {
                // Safety for empty path (one tile, being self)
                if (this.currentPathIndex <= 1)
                    return;
                
                this.currentPathIndex--;
                this.entity.MoveStart(this.path[this.currentPathIndex - 1], this.entity.PreviousExecutionMoment.Time, 1000ul, false);
            }
            
            this.isMoving = !this.isMoving;
        }
        
        public override void Redo(AStateData stateData)
        {
            Assert.IsTrue(stateData is MoveStateData);
            
            if (this.isMoving)
            {
                this.entity.MoveEnd(this.path[this.currentPathIndex]);
                this.currentPathIndex++;
                this.isMoving = false;
            }
            else if (this.currentPathIndex < this.path.Count) // Safety for empty path (one tile, being self)
            {
                this.entity.MoveStart(this.path[this.currentPathIndex], this.entity.NextExecutionMoment.Time, 1000, true);
                this.isMoving = true;
            }
        }
        
        private float GetDistance(Vector2Int origin, Vector2Int target) => HexagonHelper.Distance(origin, target);
    }
}