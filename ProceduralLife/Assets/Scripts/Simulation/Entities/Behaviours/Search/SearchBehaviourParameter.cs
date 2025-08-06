using System;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    [Serializable]
    public record SearchBehaviourParameter
    {
        [field: SerializeField]
        public E_EntityType EntityType { get; private set; }
    }
}