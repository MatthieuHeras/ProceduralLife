using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.Map
{
    [CreateAssetMenu(fileName = "NewTile", menuName = Constants.Editor.PATH_MAP + "Tile")]
    public class TileDefinition : ScriptableObject
    {
        [field: SerializeField, Required]
        public GameObject View { get; private set; }

        [field: SerializeField]
        public bool HasWater { get; private set; } = false;
    }
}