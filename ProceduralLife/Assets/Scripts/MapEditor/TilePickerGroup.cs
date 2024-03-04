using System;
using ProceduralLife.Map;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralLife.MapEditor
{
    public class TilePickerGroup : MonoBehaviour
    {
        [SerializeField, Required]
        private TileDefinition[] tiles;

        [SerializeField, Required]
        private Transform toggleAnchor;

        [SerializeField, Required]
        private TilePicker togglePrefab;

        [SerializeField, Required]
        private ToggleGroup toggleGroup;
        
        private void Awake()
        {
            foreach (TileDefinition tileDefinition in this.tiles)
            {
                TilePicker picker = Instantiate(this.togglePrefab, this.toggleAnchor);
                picker.Toggle.group = this.toggleGroup;
                this.toggleGroup.RegisterToggle(picker.Toggle);
                
                picker.Init(tileDefinition);
            }
        }
    }
}