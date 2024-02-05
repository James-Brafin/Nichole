using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    internal class AExhaustandAttack : CardAction
    {
        public int damage;
        public override void Begin(G g, State s, Combat c)
        {
            if (selectedCard != null)
            {
                c.SendCardToExhaust(s, selectedCard);
                c.QueueImmediate(new AAttack()
                {
                    damage = damage
                });
            }
        }
    }
}
