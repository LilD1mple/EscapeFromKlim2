using DG.Tweening;
using UnityEngine;

namespace EFK2.Factory
{
    public class EnemySpawnPoint : SpawnPoint
    {
        [Header("Particles")]
        [SerializeField] private ParticleSystem[] _spawnParticles;

        [Header("Light Settings")]
        [SerializeField] private Light _effectLight;
        [SerializeField] private float _lightIntensity;
        [SerializeField] private float _lightEffectDuration;

        public override void OnObjectSpawned()
        {
            AnimateParticles();

            AnimateLight();
        }

        private void AnimateLight()
        {
            _effectLight.intensity = _lightIntensity;

            _effectLight.DOIntensity(0f, _lightEffectDuration);
        }

        private void AnimateParticles()
        {
            for (int i = 0; i < _spawnParticles.Length - 1; i++)
            {
                _spawnParticles[i].Play();
            }
        }
    }
}