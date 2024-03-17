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
    internal class LeadtoGold : Artifact, NicholeArtifact
    {
        public static void Register(IModHelper helper)
        {
            helper.Content.Artifacts.RegisterArtifact("LeadtoGold", new()
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    owner = ModEntry.Instance.Nichole_Deck.Deck,
                    pools = [ArtifactPool.Boss]
                },
                Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/LeadtoGold.png")).Sprite,
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "LeadtoGold", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "LeadtoGold", "description"]).Localize
            });
        }
        public override void OnCombatStart(State state, Combat combat)
        {
            combat.QueueImmediate(
                new AStatus() { status = Status.perfectShield, statusAmount = 1, targetPlayer = true }
                );
            combat.QueueImmediate(new AStatus() { status = Status.drawLessNextTurn, statusAmount = 1, targetPlayer = true });
        }
    }
}
