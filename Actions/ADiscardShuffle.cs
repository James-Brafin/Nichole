using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    public class ADiscardShuffle : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            foreach (Card card in c.discard)
                s.SendCardToDeck(card, true, true);
            c.discard.Clear();
            s.ShuffleDeck(true);
        }
    }
}

