using JamesBrafin.Nichole.Actions;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Artifacts
{
    internal class FrostRay : Artifact, NicholeArtifact
    {
        public static void Register(IModHelper helper)
        {
            helper.Content.Artifacts.RegisterArtifact("FrostRay", new()
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    owner = ModEntry.Instance.Nichole_Deck.Deck,
                    pools = [ArtifactPool.Common]
                },
                Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/FrostRay.png")).Sprite,
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FrostRay", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FrostRay", "description"]).Localize
            });
        }
        public bool activated = false;

        public override void OnTurnStart(State state, Combat combat)
        {
            activated = false;
        }

        public override void OnEnemyGetHit(State state, Combat combat, Part? part)
        {
            if (activated == false)
            {
                this.Pulse();
                combat.QueueImmediate(new AStatus() { status = ModEntry.Instance.Cryo.Status, statusAmount = 1, targetPlayer = false });
            }
            activated = true;
        }
    }
}
