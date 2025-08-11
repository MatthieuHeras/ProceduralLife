using System;
using UnityEngine;

namespace ProceduralLife.Simulation
{
    [Serializable]
    public record HuntBehaviourParameter
    {
        [field: SerializeField]
        public E_EntityType EntityType { get; private set; }
    }
}