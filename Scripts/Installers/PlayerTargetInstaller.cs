using EFK2.HealthSystem;
using EFK2.Player;
using EFK2.Target;
using EFK2.Target.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
    public class PlayerTargetInstaller : MonoInstaller
    {
        [Header("Controller")]
        [SerializeField] private PlayerController _playerController;

        [Header("Health")]
        [SerializeField] private Health _health;

        [Header("Transforms")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _targetPoint;

        public override void InstallBindings()
        {
            BindTarget();
        }

        private void BindTarget()
        {
            Container
                .Bind<ITargetService>()
                .To<PlayerTargetService>()
                .FromNew()
                .AsSingle()
                .WithArguments(_playerController, _health, _playerTransform, _targetPoint);
        }
    }
}