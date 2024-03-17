﻿using JamesBrafin.Nichole.Actions;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace JamesBrafin.Nichole.Cards.Potions;

internal sealed class CanOfMissiles : Card, PotionCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("CanOfMissiles", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Potion_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CanOfMissiles", "name"]).Localize
        });
    }

    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            /* Give your card some meta data, such as giving it an energy cost, making it exhaustable, and more */
            cost = 0,
            temporary = true,
            exhaust = true

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
                    new ASpawn()
                    {
                        thing = new Missile

                        {   yAnimation = 0.0,
                            missileType = MissileType.normal
                        }
                    }
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new ASpawn()
                    {
                        thing = new Missile

                        {   yAnimation = 0.0,
                            missileType = MissileType.heavy
                        }
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new ASpawn()
                    {
                        thing = new Missile

                        {   yAnimation = 0.0,
                            missileType = MissileType.seeker
                        }
                    }

                };
                actions = cardActionList3;
                break;
        }
        actions.Add(new AReturnExhaust() { card = this });
        return actions;
    }
}
