using Nickel;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using JamesBrafin.Nichole.Actions;
using JamesBrafin.Nichole.Artifacts;

namespace JamesBrafin.Nichole.Cards.Nichole
{
    internal class PreciseMixture : Card, NicholeCard
    {
        
        public static void Register(IModHelper helper)
        {
            helper.Content.Cards.RegisterCard("PreciseMixture", new()
            {
                CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    deck = ModEntry.Instance.Nichole_Deck.Deck,
                    rarity = Rarity.uncommon,
                    upgradesTo = [Upgrade.A, Upgrade.B]
                },
                Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PreciseMixture", "name"]).Localize
            });
        }

        public override CardData GetData(State state)
        {
            CardData data = new CardData()
            {
                /* Give your card some meta data, such as giving it an energy cost, making it exhaustable, and more */
                cost = upgrade == Upgrade.A ? 0 : 1,
                description = ModEntry.Instance.Localizations.Localize(["card", "PreciseMixture", "description", upgrade.ToString()])

                /* if we don't set a card specific 'art' here, the game will give it the deck's 'DefaultCardArt' */
            };
            return data;
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> actions = new List<CardAction>();

            RecipieBook book = new RecipieBook();
            foreach (var artifact in s.EnumerateAllArtifacts())
            {
                if (artifact is RecipieBook recipieBook)
                {
                    switch (upgrade)
                    {
                        case Upgrade.None:
                            List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new ASpecificRandCardOffering()
                    {
                        Cards = recipieBook.learnedPotions,
                        amount = 3
                    }
                };
                            actions = cardActionList1;
                            break;
                        case Upgrade.A:
                            List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new ASpecificRandCardOffering()
                    {
                        Cards = recipieBook.learnedPotions,
                        amount = 3
                    }
                };
                            actions = cardActionList2;
                            break;
                        case Upgrade.B:
                            List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new ASpecificRandCardOffering()
                    {
                        Cards = recipieBook.learnedPotions,
                        amount = 3
                    },
                    new ASpecificRandCardOffering()
                    {
                        Cards = recipieBook.learnedPotions,
                        amount = 3
                    }
                };
                            actions = cardActionList3;
                            break;
                    }
                }
            }

            return actions;
        }
    }
}
