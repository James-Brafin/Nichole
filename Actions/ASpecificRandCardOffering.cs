using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    public sealed class ASpecificRandCardOffering : CardAction
    {
        public List<Card> Cards { get; set; } = [];
        public bool CanSkip { get; set; } = false;
        public CardDestination Destination { get; set; } = CardDestination.Hand;

        public int amount;

        private static Random rng = new Random();

        internal static void ApplyPatches(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(CardReward), nameof(CardReward.TakeCard)),
                postfix: new HarmonyMethod(typeof(ASpecificRandCardOffering), nameof(CardReward_TakeCard_Postfix))
            );
        }

        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            timer = 0;
            List<Card> randomized = Cards.OrderBy(_ => rng.Next()).ToList();
            List<Card> options = new List<Card>();
            for (int i = 0; i < amount; i++)
            {
                options.Add(randomized[i].CopyWithNewId());
            }

            return new CustomCardReward
            {
                cards = options.Select(c =>
                {
                    c.drawAnim = 1;
                    c.flipAnim = 1;
                    return c;
                }).ToList(),
                canSkip = CanSkip,
                Destination = Destination
            };
        }

        public override List<Tooltip> GetTooltips(State s)
            => [
                new TTGlossary("action.cardOffering")
            ];

        private static void CardReward_TakeCard_Postfix(CardReward __instance, G g, Card card)
        {
            if (__instance is not CustomCardReward)
                return;
            if (g.state.route is not Combat combat)
                return;

            g.state.RemoveCardFromWhereverItIs(card.uuid);
            combat.QueueImmediate(new AAddCard
            {
                card = card,
                destination = ((CustomCardReward)__instance).Destination
            });
        }

        public sealed class CustomCardReward : CardReward
        {
            public CardDestination Destination { get; set; } = CardDestination.Hand;
        }
    }
}
