using UnityEngine;

namespace ProceduralLife
{
    public static class Constants
    {
        public static readonly float TILE_SIZE = 2f / Mathf.Sqrt(3f);

        public static class Simulation
        {
            public const ulong DEFAULT_WAIT_DURATION = 1000ul;
        }
        
        public static class Editor
        {
            public const string PATH_BASE = "Procedural Life/";
            public const string PATH_ENTITY = PATH_BASE + "Entity/";
            public const string PATH_BEHAVIOURS = PATH_ENTITY + "Behaviours/";
            
            public const string PATH_MAP = PATH_BASE + "Map/";
            public const string PATH_CONDITIONS = PATH_BASE + "Conditions/";
            public const string PATH_ACTIONS = PATH_BASE + "Actions/";
        }
    }
}