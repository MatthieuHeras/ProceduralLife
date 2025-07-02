using MHLib;
using MHLib.Hexagon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralLife.Simulation
{
    public class MoveState : AState
    {
        public MoveState(SimulationEntity entity, Vector2Int targetPosition)
            : base(entity)
        {
            this.path = AStar.GetPath(this.entity.Position, targetPosition, this.GetDistance, this.GetDistance, SimulationContext.MapData.GetTileNeighbours);
        }
        
        private readonly List<Vector2Int> path;
        private int currentPathIndex = 1;
        
        private bool isMoving = false;
        private ulong moveDuration = 0ul;
        
        public override StateDoData Do()
        {
            bool wasMoving = this.isMoving;
            ulong duration = this.moveDuration;
            this.moveDuration = 50ul;
            
            if (wasMoving)
            {
                this.entity.MoveEnd(this.path[this.currentPathIndex]);
                this.currentPathIndex++;
                this.isMoving = false;
            }
            else if (this.currentPathIndex < this.path.Count) // Safety for empty path (one tile, being self)
            {
                this.moveDuration = (ulong)Random.Range(300, 1000);
                this.entity.MoveStart(this.path[this.currentPathIndex], this.entity.NextExecutionMoment.Time, this.moveDuration, true);
                this.isMoving = true;
            }
            
            AState nextState = !this.isMoving && this.currentPathIndex == this.path.Count
                               ? null
                               : this;
            
            return new StateDoData(new MoveStateData(wasMoving, duration, this.moveDuration), this.moveDuration, nextState);
        }
        
        public override void Undo(AStateData stateData)
        {
            Assert.IsTrue(stateData is MoveStateData);
            Assert.IsTrue(this.currentPathIndex > 0);
            
            MoveStateData moveStateData = (MoveStateData)stateData;

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
                this.entity.MoveStart(this.path[this.currentPathIndex - 1], this.entity.PreviousExecutionMoment.Time, moveStateData.PreviousDuration, false);
            }
            
            this.isMoving = !this.isMoving;
        }
        
        public override void Redo(AStateData stateData)
        {
            Assert.IsTrue(stateData is MoveStateData);
            MoveStateData moveStateData = (MoveStateData)stateData;
            
            Assert.IsTrue(this.isMoving == moveStateData.IsMoving);
            
            if (this.isMoving)
            {
                this.entity.MoveEnd(this.path[this.currentPathIndex]);
                this.currentPathIndex++;
                this.isMoving = false;
            }
            else if (this.currentPathIndex < this.path.Count) // Safety for empty path (one tile, being self)
            {
                this.entity.MoveStart(this.path[this.currentPathIndex], this.entity.NextExecutionMoment.Time, moveStateData.Duration, true);
                this.isMoving = true;
            }
        }
        
        private float GetDistance(Vector2Int origin, Vector2Int target) => HexagonHelper.Distance(origin, target);
    }
}