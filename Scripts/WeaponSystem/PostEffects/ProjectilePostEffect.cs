using NTC.Pool;
using UnityEngine;

namespace EFK2.WeaponSystem.PostEffects
{
    public abstract class ProjectilePostEffect : MonoBehaviour, ISpawnable
    {
        [Header("Common")]
        [SerializeField] private float _effectDuration;

        public float EffectDuration => _effectDuration;

        void ISpawnable.OnSpawn()
        {
            OnPostEffectStarted();
        }

        protected virtual void OnPostEffectStarted() { }
    }
}