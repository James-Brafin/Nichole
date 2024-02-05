using FSPRO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    public class AScry : CardAction
    {
        public int amount = 3;
        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            if (amount < s.deck.Count)
            {
                c.QueueImmediate(new ADiscardShuffle());                
            }

            IEnumerable<Card> scryedCards = s.deck.Take(amount);
            timer = 0.0;
            return new ScryHandler
            {
                amount = amount
            };
        }

    }
}