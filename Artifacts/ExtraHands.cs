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
    internal class ExtraHands : Artifact, NicholeArtifact
    {
        public static void Register(IModHelper helper)
        {
            helper.Content.Artifacts.RegisterArtifact("ExtraHands", new()
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    owner = ModEntry.Instance.Nichole_Deck.Deck,
                    pools = [ArtifactPool.Common]
                },
                Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/extraHands.png")).Sprite,
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ExtraHands", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ExtraHands", "description"]).Localize
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
                combat.QueueImmediate(new ADrawCard() { count = 1 });
            }
        }
    }
}
