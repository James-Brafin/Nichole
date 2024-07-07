using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole
{
    internal sealed class AcidTipManager
    {
        public AcidTipManager()
        {
            ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnEnemyGetHit), (State state, Combat combat, Part? part) =>
            {
                Status status = ModEntry.Instance.AcidTip.Status;
                if (state.ship.Get(status) <= 0)
                    return;

                combat.QueueImmediate(new AStatus()
                {
                    status = Status.corrode,
                    statusAmount = state.ship.Get(status),
                    targetPlayer = false,
                    omitFromTooltips = true,
                });
 
                combat.QueueImmediate(
                new AStatus()
                {
                    status = status,
                    statusAmount = 0,
                    targetPlayer = true,
                    omitFromTooltips = false,
                    mode = AStatusMode.Set
                });

            }, 0);
            ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnPlayerTakeNormalDamage), (State state, Combat combat, Part? part) =>
            {
                Status status = ModEntry.Instance.AcidTip.Status;
                if (part is null || combat.otherShip.Get(status) <= 0)
                    return;

                combat.QueueImmediate(new AStatus()
                {
                    status = Status.corrode,
                    statusAmount = combat.otherShip.Get(status),
                    targetPlayer = true,
                    omitFromTooltips = true,
                });

                combat.QueueImmediate(
                new AStatus()
                {
                    status = status,
                    statusAmount = 0,
                    targetPlayer = false,
                    omitFromTooltips = false,
                    mode = AStatusMode.Set
                });
            }, 0);

            
        }
    }
}
