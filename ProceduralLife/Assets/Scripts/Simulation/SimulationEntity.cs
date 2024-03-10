using System;
using System.Collections.Generic;
using MHLib.Hexagon;
using ProceduralLife.Map;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralLife.Simulation
{
    public class SimulationEntity : ASimulationElement
    {
        public SimulationEntity(ulong insertMoment, Vector2Int position)
            : base(insertMoment)
        {
            this.Position = position;
        }
        
        public Vector2Int Position { get; private set; }
        
        public event Action<Vector2Int, ulong, ulong, bool> MoveStartEvent = delegate { };
        public event Action<Vector2Int, ulong, ulong> MoveStartBackwardEvent = delegate { };
        public event Action<Vector2Int> MoveEndEvent = delegate { };
        public event Action<Vector2Int> MoveEndBackwardEvent = delegate { };
        
        public override ASimulationCommand Apply(SimulationContext context)
        {
            ASimulationCommand command = this.isMoving ? this.ApplyMoveEnd(context) : this.ApplyMoveStart(context);
            this.isMoving = !this.isMoving;
            
            return command;
        }
        
        #region STATE
        private bool isMoving = false;
        private Vector2Int targetTile;
        
        private ASimulationCommand ApplyMoveStart(SimulationContext context)
        {
            this.ExecutionMoment += 2000;
            context.SimulationTime.InsertUpcomingEntity(this);
            
            Vector2Int[] offsets = HexagonHelper.GetTileOffsets(this.Position.y);
            
            List<Vector2Int> neighbours = new(HexagonHelper.DIRECTIONS_COUNT);
            
            for (int i = 0, offsetsLength = offsets.Length; i < offsetsLength; i++)
            {
                Vector2Int neighbour = this.Position + offsets[i];
                if (context.MapData.Tiles.TryGetValue(neighbour, out TileDefinition tileDefinition) && !tileDefinition.HasWater)
                    neighbours.Add(neighbour);
            }
            
            int randomIndex = Random.Range(0, neighbours.Count);
            Vector2Int newPosition = neighbours[randomIndex];
            
            return new MoveStartSimulationCommand(this, newPosition, 2000);
        }
        
        private ASimulationCommand ApplyMoveEnd(SimulationContext context)
        {
            context.SimulationTime.InsertUpcomingEntity(this);
            
            return new MoveEndSimulationCommand(this, this.targetTile, 2000);
        }
        
        #endregion STATE
        
        public void MoveStart(Vector2Int newTarget, ulong startMoment, ulong duration, bool forward)
        {
            this.targetTile = newTarget;
            this.MoveStartEvent.Invoke(newTarget, startMoment, duration, forward);
        }
        
        public void MoveEnd(Vector2Int newPosition)
        {
            this.Position = newPosition;
            this.MoveEndEvent.Invoke(newPosition);
        }
    }
}