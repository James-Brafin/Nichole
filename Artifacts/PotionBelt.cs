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
    internal class PotionBelt : Artifact, NicholeArtifact
    {
        public static void Register(IModHelper helper)
        {
            helper.Content.Artifacts.RegisterArtifact("PotionBelt", new()
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    owner = ModEntry.Instance.Nichole_Deck.Deck,
                    pools = [ArtifactPool.Boss]
                },
                Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/potionBelt.png")).Sprite,
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "PotionBelt", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "PotionBelt", "description"]).Localize
            });
        }

        public override void OnCombatStart(State state, Combat combat)
        {
            this.Pulse();
            combat.QueueImmediate(new AAddRandomPotion()
            {
                isUpgraded = false
            });
            this.Pulse();
            combat.QueueImmediate(new AAddRandomPotion()
            {
                isUpgraded = false
            });
        }
    }
}
