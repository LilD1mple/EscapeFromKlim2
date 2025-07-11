using System;
using UnityEngine;

namespace EFK2.Difficult.Configurations
{
    [Serializable]
    public sealed class SpellConfig
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _regenerationTime;

        public float Damage => _damage;

        public float RegenerationTime => _regenerationTime;
    }
}
