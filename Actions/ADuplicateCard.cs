using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    internal class ADuplicateCard : CardAction
    {
        public int amount = 1;
        public override void Begin(G g, State s, Combat c)
        {
            if (selectedCard != null)
            {
                Card newCard = selectedCard.CopyWithNewId();
                ModEntry.Instance.Logger.LogInformation("check");
                c.QueueImmediate(new AAddCard()
                {
                    card = newCard,
                    destination = CardDestination.Hand,
                    amount = amount
                });
                ModEntry.Instance.Logger.LogInformation("check");
            }
            
        }
    }
}
