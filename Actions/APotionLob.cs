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
        public override void Begin(G g, State s, Combat c)
        {
            foreach (Card card in s.deck)
            {
                if (card.GetMeta().deck == ModEntry.Instance.Potion_Deck.Deck)
                {
                    List<CardAction> actions = card.GetActions(s, c);
                    s.RemoveCardFromWhereverItIs(card.uuid);
                    c.SendCardToExhaust(s, card);
                    foreach (CardAction action in actions)
                    {
                        if(action.GetType() == typeof(ADrawCard))
                        {
                            c.Queue(action);
                        }
                        else
                        {
                            c.QueueImmediate(action);
                        }
                    }
                    
              
                    if(All == false)
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
                        List<CardAction> actions = card.GetActions(s, c);
                        s.RemoveCardFromWhereverItIs(card.uuid);
                        c.SendCardToExhaust(s, card);
                        c.QueueImmediate(actions);
                        foreach (CardAction action in actions)
                        {
                            if (action.GetType() == typeof(ADrawCard))
                            {
                                c.Queue(action);
                            }
                            else
                            {
                                c.QueueImmediate(action);
                            }
                        }

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
