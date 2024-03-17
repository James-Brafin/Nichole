using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JamesBrafin.Nichole.Artifacts;
using Nickel;

namespace JamesBrafin.Nichole.Actions
{
    internal class AAddRandomPotion : CardAction
    {
        public int amount = 1;
        public bool isUpgraded;
        public bool randomUpgrade = false;
        public Upgrade upgradeType = Upgrade.None;
        public CardDestination destination = CardDestination.Hand;

        public override void Begin(G g, State s, Combat c)
        {
            var options = new List<Card>();
            foreach (var artifact in s.EnumerateAllArtifacts())
            {
                if (artifact is RecipieBook recipieBook)
                {
                    foreach (var c2 in recipieBook.learnedPotions)
                    {
                        c2.CopyWithNewId();
                       options.Add(c2);
                    }

                    Card selected = options.Random(s.rngCardOfferings).CopyWithNewId();
                    Upgrade randUpgrade;
                    Rand rng = new Rand();

                    if (isUpgraded)
                    {
                        if (randomUpgrade)
                        {
                            int rand = rng.NextInt() % 2;
                            if (rand == 0) { randUpgrade = Upgrade.A; } else { randUpgrade = Upgrade.B; }
                            selected.upgrade = randUpgrade;
                        }
                        else
                        {
                            selected.upgrade = upgradeType;
                        }

                    }

                    c.QueueImmediate(new AAddCard()
                    {
                        amount = amount,
                        card = selected,
                        destination = destination

                    });
                }
            }            

        }
    }
}
