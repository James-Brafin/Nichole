using Nickel;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using JamesBrafin.Nichole.Actions;
using static JamesBrafin.Nichole.Actions.APotionSelect;

namespace JamesBrafin.Nichole.Cards.Nichole
{
    internal class SplitBatch : Card, NicholeCard
    {
        public static void Register(IModHelper helper)
        {
            helper.Content.Cards.RegisterCard("SplitBatch", new()
            {
                CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    deck = ModEntry.Instance.Nichole_Deck.Deck,
                    rarity = Rarity.uncommon,
                    upgradesTo = [Upgrade.A, Upgrade.B]
                },
                Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "SplitBatch", "name"]).Localize
            });
        }

        public override CardData GetData(State state)
        {
            CardData data = new CardData()
            {
                /* Give your card some meta data, such as giving it an energy cost, making it exhaustable, and more */
                cost = 1,
                exhaust = true,
                description = ModEntry.Instance.Localizations.Localize(["card", "SplitBatch", "description", upgrade.ToString()])

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
                   new APotionSelect()
                   {
                       targetZone = CardSource.Hand,
                       action = new ADuplicateCard(){amount = 1},
                       mode = CardBrowse.Mode.Browse
                   }
                };
                    actions = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>()
                {
                   new APotionSelect()
                   {
                       targetZone = CardSource.NotExhaust,
                       action = new ADuplicateCard(){amount = 1},
                       mode = CardBrowse.Mode.Browse
                   }
                };
                    actions = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>()
                {
                   new APotionSelect()
                   {
                       targetZone = CardSource.Hand,
                       action = new ADuplicateCard(){amount = 2},
                       mode = CardBrowse.Mode.Browse
                   }
                };
                    actions = cardActionList3;
                    break;
            }

            return actions;
        }
    }
}
