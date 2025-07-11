using EFK2.HealthSystem;
using EFK2.Player;
using EFK2.Target.Interfaces;
using UnityEngine;

namespace EFK2.Target
{
    public sealed class PlayerTargetService : ITargetService
    {
        private readonly PlayerController _playerController;

        private readonly Health _health;

        private readonly Transform _playerTransform;
        private readonly Transform _targetPoint;

        public PlayerTargetService(PlayerController playerController, Health health, Transform playerTransform, Transform targetPoint)
        {
            _playerController = playerController;

            _health = health;

            _playerTransform = playerTransform;

            _targetPoint = targetPoint;
        }

        public PlayerController PlayerController => _playerController;

        public Health Health => _health;

        public Transform TargetPoint => _targetPoint;

        public Transform Player => _playerTransform;
    }
}