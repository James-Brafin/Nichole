using FSPRO;
using RandallMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JamesBrafin.Nichole.Actions
{
    public class AScry : CardAction
    {
        public int amount = 3;
        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            if (amount < s.deck.Count)
            {
                c.QueueImmediate(new ADiscardShuffle());                
            }

            IEnumerable<Card> scryedCards = s.deck.Take(amount);
            timer = 0.0;
            return new ScryHandler
            {
                amount = amount
            };
        }

        public override Icon? GetIcon(State s)
        {
                return new Icon((Spr)ModEntry.Instance.Foresight.Sprite , amount, Colors.textMain);
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