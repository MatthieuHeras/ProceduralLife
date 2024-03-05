using System.Collections.Generic;
using MHLib.CommandPattern;

namespace ProceduralLife.Simulation
{
    public class SimulationTime
    {
        private readonly CommandLinkedList<ASimulationCommand> commandLinkedList = new();
        private ulong currentTime;
        
        private readonly List<ASimulationCommand> upcomingCommands = new();
        
        public void IterateForward(ulong deltaTime)
        {
            this.currentTime += deltaTime;
            
            this.RedoUndoneCommands();
            this.DoUpcomingCommands();
        }
        
        public void IterateBackward(ulong deltaTime)
        {
            this.currentTime -= deltaTime;
            this.UndoCommands();
        }
        
        private void RedoUndoneCommands()
        {
            ASimulationCommand nextCommand = this.commandLinkedList.NextCommand;
            
            while (nextCommand != null && this.currentTime >= nextCommand.ExecutionMoment)
            {
                this.commandLinkedList.Redo();
                nextCommand = this.commandLinkedList.NextCommand;
            }
        }
        
        private void DoUpcomingCommands()
        {
            while (this.upcomingCommands.Count > 0 && this.currentTime >= this.upcomingCommands[0].ExecutionMoment)
            {
                this.commandLinkedList.Do(this.upcomingCommands[0]);
                this.upcomingCommands.RemoveAt(0);
            }
        }
        
        private void UndoCommands()
        {
            ASimulationCommand currentCommand = this.commandLinkedList.CurrentCommand;

            while (currentCommand != null && this.currentTime < currentCommand.ExecutionMoment)
            {
                this.commandLinkedList.Undo();
                currentCommand = this.commandLinkedList.CurrentCommand;
            }
        }
        
        public void InsertUpcomingCommand(ASimulationCommand upcomingCommand)
        {
            int insertIndex = 0;
            while (insertIndex < this.upcomingCommands.Count && upcomingCommand.ExecutionMoment > this.upcomingCommands[insertIndex].ExecutionMoment)
                insertIndex++;
            
            this.upcomingCommands.Insert(insertIndex, upcomingCommand);
        }
    }
}