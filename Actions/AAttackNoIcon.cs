using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    public class AAttackNoIcon : AAttack
    {
        public override void Begin(G g, State s, Combat c)
        {
            base.Begin(g, s, c);
        }
        public override Icon? GetIcon(State s)
        {
            return null;
        }
    }
}
