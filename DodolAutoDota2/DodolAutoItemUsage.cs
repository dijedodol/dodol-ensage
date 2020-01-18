using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Ensage;
using Ensage.Common.Extensions;
using Ensage.SDK.Abilities.Items;
using Ensage.SDK.Inventory;
using EntityExtensions = Ensage.SDK.Extensions.EntityExtensions;

namespace DodolAutoDota2
{
    public class DodolAutoItemUsage
    {
        private Object mutex = new object();
        private bool _virgin = true;
        private LoopWorkerThread _loopWorkerThread;

        internal DodolAutoItemUsage()
        {
            _loopWorkerThread = new LoopWorkerThread(() => new MainLoop().Run());
        }

        internal void start()
        {
            lock (mutex)
            {
                if (!_virgin) return;

                _virgin = false;
                _loopWorkerThread.Start();
            }
        }

        internal void stop()
        {
            _loopWorkerThread.Stop();
        }

        private class MainLoop
        {
            private Hero _localHero;

            internal void Run()
            {
                if (!Game.IsInGame || Game.IsPaused) return;

                _localHero = ObjectManager.LocalHero;
                if (_localHero == null) return;

                AutoPhaseBoots();
                AutoDagon();
                Thread.Sleep(TimeSpan.FromMilliseconds(50));
            }

            private void AutoPhaseBoots()
            {
                var ability = _localHero.GetAbilityById(AbilityId.item_phase_boots);
                if (ability.Id == AbilityId.item_phase_boots
                    && ability.CanBeCasted()
                    && _localHero.CanUseItems()
                    && _localHero.IsMoving
                    && _localHero.MovementSpeed > 0)
                {
                    ability.UseAbility();
                }
            }

            private void AutoDagon()
            {
                var ability = GetDagonAbility();
                var target = _localHero.Target as Hero;
                if (ability != null
                    && target != null
                    && _localHero.CanUseItems()
                    && _localHero.IsAttacking()
                    && ability.CanBeCasted(target)
                    && target.Health < ability.GetDamage(ability.Level)
                    && target.Distance2D(_localHero) <= ability.CastRange)
                {
                    ability.UseAbility(target);
                }
            }

            private Ability GetDagonAbility()
            {
                var ability = _localHero.GetAbilityById(AbilityId.item_dagon);
                if (ability.Id == AbilityId.item_dagon) return ability;

                ability = _localHero.GetAbilityById(AbilityId.item_dagon_2);
                if (ability.Id == AbilityId.item_dagon_2) return ability;

                ability = _localHero.GetAbilityById(AbilityId.item_dagon_3);
                if (ability.Id == AbilityId.item_dagon_3) return ability;

                ability = _localHero.GetAbilityById(AbilityId.item_dagon_4);
                if (ability.Id == AbilityId.item_dagon_4) return ability;

                ability = _localHero.GetAbilityById(AbilityId.item_dagon_5);
                if (ability.Id == AbilityId.item_dagon_5) return ability;

                return null;
            }
        }
    }
}