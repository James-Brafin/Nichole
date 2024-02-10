using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Artifacts
{
    internal class LiquidExtender : Artifact, NicholeArtifact
    {
        public static void Register(IModHelper helper)
        {
            helper.Content.Artifacts.RegisterArtifact("LiquidExtender", new()
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    owner = ModEntry.Instance.Nichole_Deck.Deck,
                    pools = [ArtifactPool.Common]
                },
                Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/liquidExtender.png")).Sprite,
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "LiquidExtender", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "LiquidExtender", "description"]).Localize
            });
        }

        public override void OnCombatStart(State state, Combat combat)
        {
            this.Pulse();
            state.ship.Set(ModEntry.Instance.PotionSaver.Status, 1);
        }
    }
}
