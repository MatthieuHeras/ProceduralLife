using System;
using ProceduralLife.Map;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ProceduralLife.MapEditor
{
    public class TilePicker : MonoBehaviour
    {
        [field: SerializeField, Required]
        public Toggle Toggle { get; private set; }

        [SerializeField, Required]
        private TextMeshProUGUI title;

        private TileDefinition tileDefinition;
        
        public static event Action<TileDefinition> TilePickedEvent = delegate { };

        public void Init(TileDefinition newTileDefinition)
        {
            this.tileDefinition = newTileDefinition;
            this.title.text = this.tileDefinition.Name;
        }
        
        private void OnToggleValueChanged(bool value)
        {
            Assert.IsTrue(this.tileDefinition != null);
            
            if (value)
                TilePickedEvent.Invoke(this.tileDefinition);
        }

        private void OnEnable()
        {
            this.Toggle.onValueChanged.AddListener(this.OnToggleValueChanged);
        }

        private void OnDisable()
        {
            this.Toggle.onValueChanged.RemoveListener(this.OnToggleValueChanged);
        }
    }
}