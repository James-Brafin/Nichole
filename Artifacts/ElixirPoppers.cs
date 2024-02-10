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
    internal class ElixirPoppers : Artifact, NicholeArtifact
    {
        public static void Register(IModHelper helper)
        {
            helper.Content.Artifacts.RegisterArtifact("ElixirPoppers", new()
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    owner = ModEntry.Instance.Nichole_Deck.Deck,
                    pools = [ArtifactPool.Boss]
                },
                Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/potionPoppers.png")).Sprite,
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ElixirPoppers", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ElixirPoppers", "description"]).Localize
            });
        }

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
        {
            if (card.GetMeta().deck == ModEntry.Instance.Potion_Deck.Deck)
            {
                this.Pulse();
                combat.Queue(new AAttack() { damage = card.GetDmg(state, 1) });
            }
        }
    }
}
