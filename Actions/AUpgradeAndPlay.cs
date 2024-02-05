using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    internal class AUpgradeAndPlay : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            List<int> list = new List<int>();
            Upgrade randUpgrade;
            Rand rng = new Rand();
            int rand = rng.NextInt() % 2;
            if (rand == 0) { randUpgrade = Upgrade.A; } else { randUpgrade = Upgrade.B; }
            if (selectedCard != null)
            {
                selectedCard.upgrade = randUpgrade;
                c.TryPlayCard(s, selectedCard, playNoMatterWhatForFree: true, true);
            }
        }
    }
}
