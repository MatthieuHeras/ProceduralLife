using System;
using MHLib.MHLib.Utils;
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

        [SerializeField, Required]
        private Camera mainCamera;
        
        private Action<Vector2Int> paintAction = null;

        private void ChangePaintAction(Action<Vector2Int> newPaintAction)
        {
            this.paintAction = newPaintAction;

            if (this.paintAction != null && this.tileHoverer.CurrentTilePosition.HasValue && MouseUtils.IsMousePositionValid(this.mainCamera))
                this.paintAction(this.tileHoverer.CurrentTilePosition.Value);
        }
        
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
            if (!tilePosition.HasValue || this.paintAction == null || !MouseUtils.IsMousePositionValid(this.mainCamera))
                return;

            this.paintAction(tilePosition.Value);
        }

        #region UNITY METHODS
        private void Update()
        {
            // WIP, waiting for proper input management.
            if (Input.GetMouseButtonDown(0))
                this.ChangePaintAction(this.AddTileAction);
            else if (Input.GetMouseButtonUp(0) && this.paintAction == this.AddTileAction)
                this.ChangePaintAction(null);
            else if (Input.GetMouseButtonDown(1))
                this.ChangePaintAction(this.RemoveTileAction);
            else if (Input.GetMouseButtonUp(1) && this.paintAction == this.RemoveTileAction)
                this.ChangePaintAction(null);
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
