using System;
using ProceduralLife.Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.MapEditor
{
    public class HexTilePainter : MonoBehaviour
    {
        [SerializeField, Required]
        private MapEditorCommandGenerator commandGenerator;
        
        [SerializeField, Required]
        private HexTileHoverer tileHoverer;
        
        private Action<Vector2Int> paintAction = null;

        private void AddTileAction(Vector2Int tilePosition)
        {
            this.commandGenerator.GenerateAddTileCommand(tilePosition);
        }

        private void RemoveTileAction(Vector2Int tilePosition)
        {
            this.commandGenerator.GenerateRemoveTileCommand(tilePosition);
        }
        
        private void OnTileHovered(Vector2Int? tilePosition)
        {
            if (!tilePosition.HasValue || this.paintAction == null)
                return;

            this.paintAction(tilePosition.Value);
        }

        #region UNITY METHODS
        private void Update()
        {
            // WIP, waiting for proper input management.
            if (Input.GetMouseButtonDown(0))
                this.paintAction = this.AddTileAction;
            else if (Input.GetMouseButtonUp(0) && this.paintAction == this.AddTileAction)
                this.paintAction = null;
            else if (Input.GetMouseButtonDown(1))
                this.paintAction = this.RemoveTileAction;
            else if (Input.GetMouseButtonUp(1) && this.paintAction == this.RemoveTileAction)
                this.paintAction = null;
        }

        private void OnEnable()
        {
            HexTileHoverer.HoverTileEvent += this.OnTileHovered;
        }

        private void OnDisable()
        {
            HexTileHoverer.HoverTileEvent -= this.OnTileHovered;
        }
        #endregion UNITY METHODS
    }
}
