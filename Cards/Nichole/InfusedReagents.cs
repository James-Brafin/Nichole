using Nickel;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using JamesBrafin.Nichole.Actions;

namespace JamesBrafin.Nichole.Cards.Nichole
{
    internal class InfusedReagents : Card, NicholeCard
    {
        
        public static void Register(IModHelper helper)
        {
            helper.Content.Cards.RegisterCard("InfusedReagents", new()
            {
                CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    deck = ModEntry.Instance.Nichole_Deck.Deck,
                    rarity = Rarity.common,
                    upgradesTo = [Upgrade.A, Upgrade.B]
                },
                Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "InfusedReagents", "name"]).Localize
            });
        }

        public override CardData GetData(State state)
        {
            CardData data = new CardData()
            {
                /* Give your card some meta data, such as giving it an energy cost, making it exhaustable, and more */
                cost = 1,
                description = ModEntry.Instance.Localizations.Localize(["card", "InfusedReagents", "description", upgrade.ToString()])

                /* if we don't set a card specific 'art' here, the game will give it the deck's 'DefaultCardArt' */
            };
            return data;
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> actions = new List<CardAction>();

            switch (upgrade)
            {
                case Upgrade.None:
                    List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AAddRandomPotion()
                    {
                      isUpgraded = false
                    },
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                    actions = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AAddRandomPotion()
                    {
                      isUpgraded = false
                    },
                    new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 1,
                        targetPlayer = true

                    }
                };
                    actions = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AAddRandomPotion()
                    {
                      isUpgraded = false
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false
                    },
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                    actions = cardActionList3;
                    break;
            }

            return actions;
        }
    }
}
