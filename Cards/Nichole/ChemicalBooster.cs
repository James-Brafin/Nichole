﻿using Nickel;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using JamesBrafin.Nichole.Actions;
using static JamesBrafin.Nichole.Actions.APotionSelect;

namespace JamesBrafin.Nichole.Cards.Nichole
{
    internal class ChemicalBooster : Card, NicholeCard
    {
        public static void Register(IModHelper helper)
        {
            helper.Content.Cards.RegisterCard("ChemicalBooster", new()
            {
                CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    deck = ModEntry.Instance.Nichole_Deck.Deck,
                    rarity = Rarity.common,
                    upgradesTo = [Upgrade.A, Upgrade.B]
                },
                Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ChemicalBooster", "name"]).Localize
            });
        }

        public override CardData GetData(State state)
        {
            CardData data = new CardData()
            {
                /* Give your card some meta data, such as giving it an energy cost, making it exhaustable, and more */
                cost = 0,
                description = ModEntry.Instance.Localizations.Localize(["card", "ChemicalBooster", "description", upgrade.ToString()])

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
                       targetZone = CardSource.NotExhaust,
                       action = new AUpgradeToCard(){randomUpgrade = true },
                       mode = CardBrowse.Mode.Browse,
                       filterUnupgraded = true
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
                       action = new AUpgradeToCard(){upgrade = Upgrade.A},
                       mode = CardBrowse.Mode.Browse,
                       filterUnupgraded = true
                   }
                };
                    actions = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new APotionSelect()
                   {
                       targetZone = CardSource.NotExhaust,
                       action = new AUpgradeToCard(){upgrade = Upgrade.B},
                       mode = CardBrowse.Mode.Browse,
                       filterUnupgraded = true
                   }
                };
                    actions = cardActionList3;
                    break;
            }

            return actions;
        }
    }
}
