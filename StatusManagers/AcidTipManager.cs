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
            Status status = ModEntry.Instance.AcidTip.Status;
            ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnPlayerTakeNormalDamage), (State state, Combat combat, Part? part) =>
            {
                if (part is null || combat.otherShip.Get(status) <= 0)
                    return;

                combat.QueueImmediate(new AStatus()
                {
                    status = Status.corrode,
                    statusAmount = combat.otherShip.Get(status),
                    targetPlayer = true,
                    omitFromTooltips = true,
                });

                combat.otherShip.Set(status, 0);
            }, 0);

            ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnEnemyGetHit), (State state, Combat combat, Part? part) =>
            {
                if (part is null || state.ship.Get(status) <= 0)
                    return;

                combat.QueueImmediate(new AStatus()
                {
                    status = Status.corrode,
                    statusAmount = state.ship.Get(status),
                    targetPlayer = false,
                    omitFromTooltips = true,
                });

                state.ship.Set(status, 0);
            }, 0); 
        }
    }
}
