using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nickel;

namespace JamesBrafin.Nichole.Actions
{
    internal class AAddRandomPotion : CardAction
    {
        public Rarity rarity;

        public bool isUpgraded;
        public override void Begin(G g, State s, Combat c)
        {
            var commonPots = ModEntry.Potion_CommonCard_Types;
            var uncommonPots = ModEntry.Potion_UnommonCard_Types;
            var rarePots = ModEntry.Potion_RareCard_Types;
            var allPots = ModEntry.Potion_AllCard_Types;
            var cards = new List<Type>();

            if (rarity == Rarity.common)
            {
                cards = commonPots.ToList();
            }
            else if(rarity == Rarity.uncommon)
            {
                cards = uncommonPots.ToList();
            }
            else if (rarity == Rarity.rare) 
            { 
                cards = rarePots.ToList();
            }

                var options = new List<Card>();

            foreach (var c2 in cards)
            {
                Card? card = (Card?)Activator.CreateInstance(c2);
                if (card != null)
                {
                    options.Add(card);
                }
            }

            Card selected = options.Random(s.rngCardOfferings);
            Upgrade randUpgrade;
            Rand rng = new Rand();

            if (isUpgraded)
            {
                int rand = rng.NextInt() % 2;
                if (rand == 0) { randUpgrade = Upgrade.A; } else { randUpgrade = Upgrade.B; }
                selected.upgrade = randUpgrade;
            }

            c.QueueImmediate(new AAddCard()
            { 
                amount = 1, 
                card = selected, 
                destination = CardDestination.Hand

            });

        }
    }
}
