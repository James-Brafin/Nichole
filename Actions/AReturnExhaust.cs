using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    internal class AReturnExhaust : CardAction
    {
        public Card? card;

        public override void Begin(G g, State s, Combat c)
        {
            if (card != null && card.exhaustOverrideIsPermanent != true)
            {
                card.exhaustOverride = true;
            }
        }
    }
}
