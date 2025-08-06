using System;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    [Serializable]
    public record WaitBehaviourParameter
    {
        [field: SerializeField]
        public ulong Duration { get; private set; }
    }
}