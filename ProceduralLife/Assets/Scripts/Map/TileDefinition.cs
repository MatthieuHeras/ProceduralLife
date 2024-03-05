using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.Map
{
    [CreateAssetMenu(fileName = "NewTile", menuName = "Map/Tile")]
    public class TileDefinition : ScriptableObject
    {
        [field: SerializeField, Required]
        public string Name { get; private set; } = "NewTile";
        
        [field: SerializeField, Required]
        public GameObject View { get; private set; }

        [field: SerializeField]
        public int FoodQuantity { get; private set; } = 0;

        [field: SerializeField]
        public bool HasWater { get; private set; } = false;
    }
}