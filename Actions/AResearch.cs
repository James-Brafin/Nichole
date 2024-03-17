using HarmonyLib;
using JamesBrafin.Nichole.Artifacts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JamesBrafin.Nichole.Actions.ASpecificRandCardOffering;

namespace JamesBrafin.Nichole.Actions
{
    public sealed class AResearch : CardAction
    {
        public bool CanSkip { get; set; } = false;

        public int amount = 3;

        private static Random rng = new Random();

        internal static void ApplyPatches(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(CardReward), nameof(CardReward.TakeCard)),
                postfix: new HarmonyMethod(typeof(AResearch), nameof(CardReward_TakeCard_Postfix))
            );
        }

        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            timer = 0;
            List<Card> Cards = new List<Card>();
            foreach (var artifact in g.state.EnumerateAllArtifacts())
            {
                if (artifact is RecipieBook recipieBook)
                {
                    Cards = recipieBook.unlearnedPotions;
                }
            }
            List<Card> randomized = Cards.OrderBy(_ => rng.Next()).ToList();
            ModEntry.Instance.Logger.LogInformation(randomized.Count().ToString());
            List<Card> options = new List<Card>();
            for (int i = 0; i < amount; i++)
            {
                options.Add(randomized[i].CopyWithNewId());
            }

            return new Research
            {
                cards = options.Select(c =>
                {
                    c.drawAnim = 1;
                    c.flipAnim = 1;
                    return c;
                }).ToList(),
                canSkip = CanSkip,
            };
        }

        public override List<Tooltip> GetTooltips(State s)
            => [
                new TTGlossary("action.cardOffering")
            ];

        private static void CardReward_TakeCard_Postfix(CardReward __instance, G g, Card card)
        {
            if (__instance is not Research custom)
                return;

            foreach (var artifact in g.state.EnumerateAllArtifacts())
            {
                if (artifact is RecipieBook recipieBook)
                {
                    recipieBook.learnedPotions.Add(card);
                    for (int i = 0; i < recipieBook.unlearnedPotions.Count; i++)
                    {
                        if (recipieBook.unlearnedPotions.ElementAt(i).GetType() == card.GetType())
                        {
                            recipieBook.unlearnedPotions.RemoveAt(i); break;
                        }
                    }
                }
            }
            
        }
        

        public sealed class Research : CardReward
        {

        }
    }
}
