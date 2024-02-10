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
    internal class RecipieBook : Artifact, NicholeArtifact
    {
        public static void Register(IModHelper helper)
        {
            helper.Content.Artifacts.RegisterArtifact("RecipieBook", new()
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    owner = ModEntry.Instance.Nichole_Deck.Deck,
                    pools = [ArtifactPool.Boss]
                },
                Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/recipieBook.png")).Sprite,
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "RecipieBook", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "RecipieBook", "description"]).Localize
            });
        }
        public bool potionPlayed = false;

        public override void OnTurnStart(State state, Combat combat)
        {
            potionPlayed = false;
        }

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
        {
            if (!potionPlayed && card.GetMeta().deck == ModEntry.Instance.Potion_Deck.Deck)
            {
                this.Pulse();
                potionPlayed = true;
                combat.QueueImmediate(new AAddRandomPotion() { rarity = card.GetMeta().rarity });
            }
        }
    }
}
