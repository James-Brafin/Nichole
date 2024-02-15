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
    internal class AlchemicalEngine : Artifact, NicholeArtifact
    {
        public static void Register(IModHelper helper)
        {
            helper.Content.Artifacts.RegisterArtifact("AlchemicalEngine", new()
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    owner = ModEntry.Instance.Nichole_Deck.Deck,
                    pools = [ArtifactPool.Common]
                },
                Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/AlchemicalEngine.png")).Sprite,
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AlchemicalEngine", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AlchemicalEngine", "description"]).Localize
            });
        }
        public int potionsPlayed = 0;

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
        {
            if (card.GetMeta().deck == ModEntry.Instance.Potion_Deck.Deck)
            {
                this.Pulse();
                potionsPlayed++;

                if (potionsPlayed >= 3)
                {
                    potionsPlayed = 0;
                    combat.QueueImmediate(new AEnergy()
                    {
                        changeAmount = 1
                    });
                }
            }
        }

        public override int? GetDisplayNumber(State s)
        {
            if (this.potionsPlayed != 0)
                return this.potionsPlayed;
            return null;
        }
    }
}
