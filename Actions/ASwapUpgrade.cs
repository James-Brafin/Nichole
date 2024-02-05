using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    internal class ASwapUpgrade : CardAction
    {
        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            List<int> list = new List<int>();
            if (selectedCard != null) {
                if (selectedCard.upgrade == Upgrade.A) { selectedCard.upgrade = Upgrade.B; } 
                else if (selectedCard.upgrade == Upgrade.B) { selectedCard.upgrade = Upgrade.A; }
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
