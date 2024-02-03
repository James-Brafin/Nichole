using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace JamesBrafin.Nichole.Cards.Potions;

internal sealed class StrengthPotion : Card, PotionCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("StrengthPotion", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Potion_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "StrengthPotion", "name"]).Localize
        });
    }

    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            /* Give your card some meta data, such as giving it an energy cost, making it exhaustable, and more */
            cost = 0,
            temporary = true,
            exhaust = true,
            singleUse = true,

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
                    new AStatus()
                    {
                        status = Status.overdrive,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.overdrive,
                        statusAmount = 2,
                        targetPlayer = true
                    }
                };
                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.overdrive,
                        statusAmount = 1,
                        targetPlayer = true
                    },

                    new AAttack()
                    {
                        damage = GetDmg(s, 2)
                    }
                };
                actions = cardActionList3;
                break;
        }

        return actions;
    }
}
