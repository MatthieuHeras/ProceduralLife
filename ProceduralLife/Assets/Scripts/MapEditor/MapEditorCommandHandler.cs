using MHLib.CommandPattern;
using UnityEngine;

namespace ProceduralLife.MapEditor
{
    public class MapEditorCommandHandler : MonoBehaviour
    {
        private readonly CommandLinkedList<AMapEditorCommand> commandLinkedList = new();
        
        public void DoCommand(AMapEditorCommand command) => this.commandLinkedList.Do(command);
        public void Undo() => this.commandLinkedList.Undo();
        public void Redo() => this.commandLinkedList.Redo();
    }
}