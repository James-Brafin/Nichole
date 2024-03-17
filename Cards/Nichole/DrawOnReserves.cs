using Nickel;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using JamesBrafin.Nichole.Actions;

namespace JamesBrafin.Nichole.Cards.Nichole
{
    internal class DrawOnReserves : Card, NicholeCard
    {
        
        public static void Register(IModHelper helper)
        {
            helper.Content.Cards.RegisterCard("DrawOnReserves", new()
            {
                CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    deck = ModEntry.Instance.Nichole_Deck.Deck,
                    rarity = Rarity.common,
                    upgradesTo = [Upgrade.A, Upgrade.B]
                },
                Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "DrawOnReserves", "name"]).Localize
            });
        }

        public override CardData GetData(State state)
        {
            CardData data = new CardData()
            {
                /* Give your card some meta data, such as giving it an energy cost, making it exhaustable, and more */
                cost = upgrade == Upgrade.B ? 1 : 2,
                exhaust = true,
                description = ModEntry.Instance.Localizations.Localize(["card", "DrawOnReserves", "description", upgrade.ToString()])

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
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    }
                };
                    actions = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new ADrawCard()
                    {
                        count = 2
                    }
                };
                    actions = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    },
                    new AAddRandomPotion()
                    {
                      isUpgraded = false,
                      destination = CardDestination.Deck
                    }
                };
                    actions = cardActionList3;
                    break;
            }

            return actions;
        }
    }
}
