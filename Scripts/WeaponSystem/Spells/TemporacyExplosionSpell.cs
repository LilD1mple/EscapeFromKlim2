using EFK2.HealthSystem.Interfaces;
using NTC.OverlapSugar;
using System.Collections.Generic;
using static EFK2.Extensions.Tasks.TaskSugar;

namespace EFK2.WeaponSystem.Spells
{
    public class TemporacyExplosionSpell : Spell
    {
        private readonly List<IDamageable> _overlapResults = new(32);

        public override void StartAttack()
        {
            WaitForDelay();
        }

        private async void WaitForDelay()
        {
            if (AttackDelay > 0)
            {
                await Delay(AttackDelay);
            }

            OnPerformAttack();

            ShakeCamera();
        }

        protected override void OnPerformAttack()
        {
            if (OverlapSettings.TryFind(_overlapResults))
                _overlapResults.ForEach(ApplyDamage);
        }

        private void ApplyDamage(IDamageable damageable)
        {
            damageable.TakeDamage(Damage);
        }
    }
}