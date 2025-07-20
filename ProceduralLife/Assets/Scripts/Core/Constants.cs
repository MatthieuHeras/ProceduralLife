using UnityEngine;

namespace ProceduralLife
{
    public static class Constants
    {
        public static readonly float TILE_SIZE = 2f / Mathf.Sqrt(3f);

        public static class Editor
        {
            public const string PATH_BASE = "PL/";
            
            public const string PATH_CONDITIONS = PATH_BASE + "Conditions/";
            public const string PATH_ACTIONS = PATH_BASE + "Actions/";
        }
    }
}