using ProceduralLife.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ProceduralLife.Simulation
{
    public class SimulationEntity : ASimulationElement
    {
        public SimulationEntity(MapData mapData)
        {
            this.MapData = mapData;
            this.currentState = new MoveState(this, this.MapData.Tiles.ElementAt(Random.Range(0, this.MapData.Tiles.Count)).Key);
        }

        public readonly MapData MapData;
        public Vector2Int Position { get; private set; } = Vector2Int.zero;
        
        public event Action<Vector2Int, ulong, ulong, bool> MoveStartEvent = delegate { };
        public event Action<Vector2Int> MoveEndEvent = delegate { };
        
        private readonly List<AStateData> stateData = new();
        
        private int currentIndex = -1;
        private AState currentState;
        
        public override void Do()
        {
            Assert.IsTrue(this.currentIndex < this.stateData.Count);
            
            // Break future data, should happen when we break the replay to start a new timeline
            if (this.currentIndex < this.stateData.Count - 1)
                this.stateData.RemoveRange(this.currentIndex + 1, this.stateData.Count - 1 - this.currentIndex);
            
            StateDoData stateDoData = this.currentState.Do();
            
            SimulationMoment executionMoment = this.NextExecutionMoment;
            context.SimulationTime.DelayElement(this, stateDoData.Delay);
            
            stateDoData.StateData.InitStates(this.currentState, stateDoData.NextState)
                                 .InitExecutionMoments(executionMoment, this.NextExecutionMoment);
            
            this.stateData.Add(stateDoData.StateData);
            this.currentIndex++;
            
            this.currentState = stateDoData.NextState;
        }
        
        public override void Undo()
        {
            this.currentState.Undo(this.stateData[this.currentIndex]);
            this.currentIndex--;

            if (this.currentIndex >= 0)
            {
                this.PreviousExecutionMoment = this.stateData[this.currentIndex].ExecutionMoment;
                this.currentState = this.stateData[this.currentIndex].State;
            }
        }
        
        public override void Redo()
        {
            Assert.IsTrue(this.currentIndex < this.stateData.Count - 1);
            this.currentIndex++;
            
            this.currentState = this.stateData[this.currentIndex].State;
            this.currentState.Redo(this.stateData[this.currentIndex]);
            this.NextExecutionMoment = this.stateData[this.currentIndex].NextExecutionMoment;
        }
        
        public void MoveStart(Vector2Int newTarget, ulong startMoment, ulong duration, bool forward)
        {
            this.MoveStartEvent.Invoke(newTarget, startMoment, duration, forward);
        }
        
        public void MoveEnd(Vector2Int newPosition)
        {
            this.Position = newPosition;
            this.MoveEndEvent.Invoke(newPosition);
        }
    }
}