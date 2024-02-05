using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Actions
{
    internal class APotionSelect : CardAction
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CardSource
        {
            Hand,
            HandorDeck,
            NotExhaust
        }
        public CardSource targetZone;
        public CardAction? action;
        public CardBrowse.Mode mode;
        public bool filterUnupgraded = false;
        public bool filterUpgraded = false;

        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            var cardSet = new List<Card>();
            if (targetZone == CardSource.Hand)
            {
                cardSet = c.hand;
            }
            else if (targetZone == CardSource.HandorDeck)
            {
                cardSet = s.deck;
                cardSet.AddRange(c.hand);
            }
            else if (targetZone == CardSource.NotExhaust)
            {
                cardSet = s.deck;
                cardSet.AddRange(c.hand);
                cardSet.AddRange(c.discard);
            }

            var cards = new List<Card>();

            foreach (var card in cardSet) 
            {
                if (card.GetMeta().deck == ModEntry.Instance.Potion_Deck.Deck)
                {
                    if ((filterUnupgraded && card.upgrade == Upgrade.None) || (filterUpgraded && card.upgrade != Upgrade.None))
                    {
                        cards.Add(card);
                    }
                    else if(!filterUnupgraded && !filterUpgraded)
                    {
                        cards.Add(card);
                    }
                }
            }

            if (cards.Count < 0) { return null;}

            var cardBrowse = new PotionCardBrowse
            {
                mode = mode,
                browseAction = action,
                Cards = cards,
                allowCancel = false
            };

            timer = 0;

            return cardBrowse.GetCardList(g).Count != 0 ? cardBrowse : null;
        }
    }
}
