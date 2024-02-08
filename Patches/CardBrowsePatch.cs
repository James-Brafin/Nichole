﻿using System.Collections.Generic;
using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JamesBrafin.Nichole.Actions;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using System.Linq;

namespace JamesBrafin.Nichole.Patches;

[HarmonyPatch(typeof(CardBrowse))]
public class CardBrowsePatch
{
    [HarmonyTranspiler]
    [HarmonyPatch("GetCardList")]
    private static IEnumerable<CodeInstruction> GetCardListTranspiler(IEnumerable<CodeInstruction> instructions,
        ILGenerator il, MethodBase originalMethod)
    {
        try
        {
            var localVars = originalMethod.GetMethodBody()!.LocalVariables;

            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(
                    ILMatches.Ldloc<List<Card>>(localVars),
                    ILMatches.AnyLdloc,
                    ILMatches.Call("AddRange")
                )
                .PointerMatcher(SequenceMatcherRelativeElement.First)
                .CreateLdlocInstruction(out var cardList)
                .Advance(2)
                .Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.JustInsertion,
                    new List<CodeInstruction>
                    {
                        new(OpCodes.Ldarg_0),
                        cardList,
                        new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(CardBrowsePatch), nameof(InjectCards)))
                    })
                .AllElements();
        }
        catch (Exception e)
        {
            Console.WriteLine("CardBrowse.GetCardList patch failed!");
            Console.WriteLine(e);
            return instructions;
        }
    }

    private static void InjectCards(CardBrowse browse, List<Card> cardList)
    {
        if (browse is not SelectedCardBrowse acb) return;
        var toInject = acb.Cards;

        if (toInject == null) return;
        var allCards = MG.inst.g.state.deck.ToList();
        if (MG.inst.g.state.route is Combat combat)
        {
            allCards.AddRange(combat.hand);
            allCards.AddRange(combat.discard);
            allCards.AddRange(combat.exhausted);
        }
        cardList.Clear();
        cardList.AddRange(toInject.Select(c => allCards.FirstOrDefault(c2 => c2.uuid == c.uuid, c)));
    }
}