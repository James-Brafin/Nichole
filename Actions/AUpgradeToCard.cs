using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    internal class AUpgradeToCard : CardAction
    {
        public bool randomUpgrade = false;
        public Upgrade upgrade = Upgrade.None;
        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            List<int> list = new List<int>();
            if (randomUpgrade == true)
            {
                Rand rng = new Rand();
                int rand = rng.NextInt() % 2;
                if (rand == 0) { upgrade = Upgrade.A; } else { upgrade = Upgrade.B; }
            }
            
            if (selectedCard != null ) {
                selectedCard.upgrade = upgrade;
                list.Add(selectedCard.uuid);
            }
            

            if (list.Count > 0)
            {
                return new ShowCards
                {
                    messageKey = "showcards.upgraded",
                    cardIds = list.ToList()
                };
            }

            return null;
        }
    }
}
