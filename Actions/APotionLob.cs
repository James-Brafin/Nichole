using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    internal class APotionLob : CardAction
    {
        public bool All = false;
        public bool Discards = false;
        public override void Begin(G g, State s, Combat c)
        {
            List<Card> cardList = s.deck;
            foreach (Card card in cardList)
            {
                if (card.GetMeta().deck == ModEntry.Instance.Potion_Deck.Deck)
                {
                    c.TryPlayCard(s, card, playNoMatterWhatForFree: true, true);
                    if(All == false)
                    {
                        break;
                    } 
                }
            }

            if (Discards)
            {
                List<Card> discardList = c.discard;
                foreach (Card card in cardList)
                {
                    if (card.GetMeta().deck == ModEntry.Instance.Potion_Deck.Deck)
                    {
                        c.TryPlayCard(s, card, playNoMatterWhatForFree: true, true);
                        if (All == false)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
