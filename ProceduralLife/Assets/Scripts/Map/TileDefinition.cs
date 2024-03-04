using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.Map
{
    [CreateAssetMenu(fileName = "NewTile", menuName = "Map/Tile")]
    public class TileDefinition : ScriptableObject
    {
        [field: SerializeField, Required]
        public string Name { get; private set; }
        
        [field: SerializeField, Required]
        public GameObject View { get; private set; }
    }
}