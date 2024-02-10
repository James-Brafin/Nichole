using Microsoft.Extensions.Logging;
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
        public int number = 1;
        public bool Attack = false;
        public int atkAmount = 0; 
        public override void Begin(G g, State s, Combat c)
        {
            for(int i = 0; i < number; i++) {
                foreach (Card card in s.deck)
                {
                    if (card.GetMeta().deck == ModEntry.Instance.Potion_Deck.Deck)
                    {
                        c.QueueImmediate(new APlaySpecificCardFromAnywhere()
                        {
                            CardId = card.uuid
                        });
                        if (All == false)
                        {
                            break;
                        }
                    }
                }

                if (Discards)
                {
                    foreach (Card card in c.discard)
                    {
                        if (card.GetMeta().deck == ModEntry.Instance.Potion_Deck.Deck)
                        {
                            c.QueueImmediate(new APlaySpecificCardFromAnywhere()
                            {
                                CardId = card.uuid
                            });
                            if (All == false)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (Attack)
            {
                c.Queue(new AAttack()
                {
                    damage = atkAmount
                });
            }
            
        }
    }
}
