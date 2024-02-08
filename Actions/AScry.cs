using FSPRO;
using Microsoft.Extensions.Logging;
using RandallMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JamesBrafin.Nichole.Actions.APotionSelect;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace JamesBrafin.Nichole.Actions
{
    public class AScry : CardAction
    {
        public int amount = 3;
        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            if (amount > s.deck.Count)
            {
                c.QueueImmediate(new ADiscardShuffle());                
            }

            List<Card> cardSet = new List<Card>();
                cardSet = s.deck;

            List<Card> cards = new List<Card>();
            for (int i = 0; i < amount; i++)
            {
                cards.Add(cardSet.ElementAt(i));
            }

            if (cards.Count < 0) { return null; }

            var cardBrowse = new SelectedCardBrowse
            {
                mode = CardBrowse.Mode.Browse,
                browseAction = new AScryHandler() {cards = cards},
                Cards = cards,
                allowCancel = false
            };

            timer = 0;

            return cardBrowse.GetCardList(g).Count != 0 ? cardBrowse : null;
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon((Spr)ModEntry.Instance.Foresight.Sprite, amount, Colors.textMain);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            return [
             new CustomTTGlossary(
                 CustomTTGlossary.GlossaryType.action,
                 () => ModEntry.Instance.Foresight.Sprite,
                 () => ModEntry.Instance.Localizations.Localize(["action", "AScry", "name"]),
                 () => ModEntry.Instance.Localizations.Localize(["action", "AScry", "description"], new { count = amount })
             )
         ];
        }

    }
}

