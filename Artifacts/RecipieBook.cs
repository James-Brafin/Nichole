using JamesBrafin.Nichole.Actions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nickel;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JamesBrafin.Nichole.Artifacts
{
    internal class RecipieBook : Artifact, NicholeArtifact
    {
        public List<Card> learnedPotions = new List<Card>();
        public List<Card> unlearnedPotions = new List<Card>();
        public static void Register(IModHelper helper)
        {
            helper.Content.Artifacts.RegisterArtifact("RecipieBook", new()
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    owner = ModEntry.Instance.Nichole_Deck.Deck,
                    pools = [ArtifactPool.Common],
                    unremovable = true
                    
                },
                Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/recipieBook.png")).Sprite,
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "RecipieBook", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "RecipieBook", "description"]).Localize
            });
        }
        public override void OnReceiveArtifact(State state)
        {
            learnedPotions = addCards(ModEntry.Potion_StartingCards.ToList());
            unlearnedPotions = addCards(ModEntry.Potion_notStartingCards.ToList());
        }

        public override void OnCombatStart(State state, Combat combat)
        {
            if(learnedPotions.Count <= 0)
            {
                learnedPotions = addCards(ModEntry.Potion_StartingCards.ToList());
            }

            if (unlearnedPotions.Count <= 0)
            {
                unlearnedPotions = addCards(ModEntry.Potion_notStartingCards.ToList());
            }
            ModEntry.Instance.Logger.LogInformation(unlearnedPotions.Count().ToString());
        }

        public override void OnCombatEnd(State state)
        {
            if (state.map.markers[state.map.currentLocation].contents is MapBattle mapBattle)
            {
                if (mapBattle.battleType == BattleType.Elite || mapBattle.battleType == BattleType.Boss)
                {
                    state.rewardsQueue.Add(new AResearch()
                    {

                    });
                }
            }
        }

        private List<Card> addCards(List<Type> initCards)
        {
            var list = new List<Card>();

            foreach (var recipe in initCards)
            {
                Card? card = (Card?)Activator.CreateInstance(recipe);
                if (card != null)
                {
                    list.Add(card);
                }
            }

            return list;
        }
    }
}
