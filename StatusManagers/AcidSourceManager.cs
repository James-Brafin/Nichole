using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole
{
    internal sealed class AcidSourceManager
    {
        public AcidSourceManager()
        {
            
            ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnPlayerTakeNormalDamage), (State state, Combat combat, Part? part) =>
            {
                Status status = ModEntry.Instance.AcidSource.Status;
                if (combat.otherShip.Get(status) <= 0)
                    return;

                combat.QueueImmediate(new AStatus()
                {
                    status = Status.corrode,
                    statusAmount = combat.otherShip.Get(status),
                    targetPlayer = true,
                    omitFromTooltips = true,
                });
            }, 0);

            ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnEnemyGetHit), (State state, Combat combat, Part? part) =>
            {
                Status status = ModEntry.Instance.AcidSource.Status;
                if (state.ship.Get(status) <= 0)
                    return;

                combat.QueueImmediate(new AStatus()
                {
                    status = Status.corrode,
                    statusAmount = state.ship.Get(status),
                    targetPlayer = false,
                    omitFromTooltips = true,
                });
            }, 0); 
        }
    }
}
