using System;
using System.Collections.Generic;
using MHLib.CommandPattern;
using ProceduralLife.Map;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    public class SimulationTime
    {
        public SimulationTime(MapData mapData)
        {
            this.SimulationContext = new SimulationContext(this, mapData);
        }
        
        private readonly CommandLinkedList<ASimulationCommand> commandLinkedList = new();
        private readonly List<ASimulationElement> upcomingElements = new();
        public readonly SimulationContext SimulationContext;
        
        public ulong CurrentTime { get; private set; }
        
        public static event Action<ulong, ulong> CurrentTimeChanged = delegate { };
        
        public void IterateForward(ulong deltaTime)
        {
            ulong previousTime = this.CurrentTime;
            
            this.CurrentTime += deltaTime;
            
            this.RedoUndoneCommands();
            this.ApplyUpcomingElements();
            
            CurrentTimeChanged.Invoke(previousTime, this.CurrentTime);
        }
        
        public void IterateBackward(ulong deltaTime)
        {
            ulong previousTime = this.CurrentTime;
            
            if (deltaTime > this.CurrentTime)
                this.CurrentTime = 0;
            else
                this.CurrentTime -= deltaTime;
            
            this.UndoCommands();
            
            CurrentTimeChanged.Invoke(previousTime, this.CurrentTime);
        }
        
        private void RedoUndoneCommands()
        {
            ASimulationCommand nextCommand = this.commandLinkedList.NextCommand;
            
            while (nextCommand != null && this.CurrentTime >= nextCommand.ExecutionMoment)
            {
                this.commandLinkedList.Redo();
                nextCommand = this.commandLinkedList.NextCommand;
            }
        }
        
        private void ApplyUpcomingElements()
        {
            while (this.upcomingElements.Count > 0 && this.CurrentTime >= this.upcomingElements[0].ExecutionMoment)
            {
                ASimulationElement upcomingElement = this.upcomingElements[0];
                this.upcomingElements.RemoveAt(0);

                ulong executionMoment = upcomingElement.ExecutionMoment;
                ASimulationCommand newCommand = upcomingElement.Apply(this.SimulationContext);
                newCommand.SetExecutionMoment(executionMoment);
                this.commandLinkedList.Do(newCommand);
            }
        }
        
        private void UndoCommands()
        {
            while (this.commandLinkedList.CurrentCommand is { } currentCommand && this.CurrentTime < currentCommand.ExecutionMoment)
                this.commandLinkedList.Undo();
        }
        
        public void InsertUpcomingEntity(SimulationEntity upcomingEntity)
        {
            int insertIndex = 0;
            while (insertIndex < this.upcomingElements.Count && upcomingEntity.ExecutionMoment > this.upcomingElements[insertIndex].ExecutionMoment)
                insertIndex++;
            
            this.upcomingElements.Insert(insertIndex, upcomingEntity);
        }
        
        public void SpeedUpEntity(SimulationEntity entity, ulong time)
        {
            int entityIndex = this.FindEntityIndex(entity);
            
            if (entityIndex == -1)
            {
                Debug.LogError($"Tried to speed up {entity} but it could not be found.");
                return;
            }
            
            entity.ExecutionMoment -= time;
            
            int newEntityIndex = entityIndex;
            while (newEntityIndex > 0 && this.upcomingElements[newEntityIndex - 1].ExecutionMoment > entity.ExecutionMoment)
                newEntityIndex--;
            
            if (newEntityIndex == entityIndex)
                return;
            
            this.upcomingElements.RemoveAt(entityIndex);
            this.upcomingElements.Insert(newEntityIndex, entity);
        }
        
        public void SlowDownEntity(SimulationEntity entity, ulong time)
        {
            int entityIndex = this.FindEntityIndex(entity);
            
            if (entityIndex == -1)
            {
                Debug.LogError($"Tried to slow down {entity} but it could not be found.");
                return;
            }
            
            entity.ExecutionMoment += time;
            
            int newEntityIndex = entityIndex;
            while (newEntityIndex < this.upcomingElements.Count - 1 && this.upcomingElements[newEntityIndex + 1].ExecutionMoment < entity.ExecutionMoment)
                newEntityIndex++;
            
            if (newEntityIndex == entityIndex)
                return;
            
            this.upcomingElements.Insert(newEntityIndex, entity);
            this.upcomingElements.RemoveAt(entityIndex);
        }
        
        private int FindEntityIndex(SimulationEntity entity)
        {
            for (int i = 0; i < this.upcomingElements.Count; i++)
            {
                if (this.upcomingElements[i] == entity)
                    return i;
            }
            
            return -1;
        }
    }
}