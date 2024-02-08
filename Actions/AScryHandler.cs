using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    internal class AScryHandler : CardAction
    {
        public List<Card> cards = new List<Card>();
        public override void Begin(G g, State s, Combat c)
        {
            if (selectedCard != null)
            {
                foreach (Card card in cards) {
                    s.RemoveCardFromWhereverItIs(card.uuid);
                    if (card.uuid.Equals(selectedCard.uuid))
                        {
                        c.SendCardToHand(s, card);
                    }
                    else
                    {
                        c.SendCardToDiscard(s, card);
                    }
                }
                
                

            }
        }
    }
}
