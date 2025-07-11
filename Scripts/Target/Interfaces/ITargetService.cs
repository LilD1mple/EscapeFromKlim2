using EFK2.HealthSystem;
using EFK2.Player;
using UnityEngine;

namespace EFK2.Target.Interfaces
{
    public interface ITargetService
    {
        PlayerController PlayerController { get; }

        Health Health { get; }

        Transform Player { get; }

        Transform TargetPoint { get; }
    }
}
