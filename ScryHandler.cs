﻿using daisyowl.text;
using FSPRO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JamesBrafin.Nichole
{
    internal class ScryHandler : Route, OnMouseDown
{
    public List<Card> cards = new List<Card>();

    public int amount;

    public double? takeCardAnimation;

    public double flourishTimer;

    public double flipFloppableCardsTimer;

    public const double INPUT_WAIT_TIME = 0.7;

    public const double FLIP_WAIT_TIME = 3.0;

    private bool didFirstFrame;

    public override bool GetShowOverworldPanels()
    {
        return true;
    }

    public override bool CanBePeeked()
    {
        return true;
    }

    public void OnMouseDown(G g, Box b)
    {
        int? num = b.key?.ValueFor(UK.card);
        if (num.HasValue)
        {
            int uuid = num.GetValueOrDefault();
            Card? card = cards.FirstOrDefault((Card c) => c.uuid == uuid);
            if (card != null && !takeCardAnimation.HasValue)
            {
                Audio.Play(Event.CardHandling);
                TakeCard(g, card);
            }
        }

        if (!(b.key == UK.cardReward_skip) || takeCardAnimation.HasValue)
        {
            return;
        }

        if (cards[0].temporaryOverride != true)
        {
            Analytics.Log(g.state, "cardReward", new
            {
                skipped = true,
                cards = cards.Select((Card c) => c.Key())
            });
        }

        Audio.Play(Event.Click);
        g.CloseRoute(this);
    }

    public void TakeCard(G g, Card card)
    {
            if (g.state.route is Combat combat)
            {
                foreach (Card c in cards)
                {
                    if (c == card)
                    {
                        combat.SendCardToHand(g.state, c);
                    }
                    else
                    {
                        combat.SendCardToDiscard(g.state, c);
                    }
                }
            }

        card.targetPos = new Vec(G.screenSize.x / 2.0, G.screenSize.y + 100.0);
        takeCardAnimation = 0.0;
    }

    public override void Render(G g)
    {
        if (cards.Count == 0)
        {
            g.CloseRoute(this);
            return;
        }

        if (takeCardAnimation.HasValue)
        {
            takeCardAnimation += g.dt;
        }

        if (takeCardAnimation > 0.5)
        {
            g.CloseRoute(this);
        }

        if (!takeCardAnimation.HasValue)
        {
            List<Card> hand = cards;
            Vec offset = new Vec(240.0, 100.0);
            bool instant = !didFirstFrame;
            CardUtils.FanOut(hand, offset, 68, null, instant);
        }

        int num = 0;
        foreach (Card card in cards)
        {
            if (!((double)(num++ + 3) > flourishTimer * 20.0))
            {
                card.UpdateAnimation(g);
            }
        }

        flourishTimer += g.dt;
        if (flipFloppableCardsTimer > 3.0)
        {
            flipFloppableCardsTimer = 0.0;
            foreach (Card card2 in cards)
            {
                if (card2.GetDataWithOverrides(g.state).floppable && !card2.isForeground)
                {
                    card2.flipped = !card2.flipped;
                    card2.flipAnim = 1.0;
                }
            }
        }

        flipFloppableCardsTimer += g.dt;
        if (!didFirstFrame)
        {
            didFirstFrame = true;
        }

        SharedArt.DrawEngineering(g);
        string str = Loc.T("cardReward.title", "PICK A CARD");
        Font stapler = DB.stapler;
        Color? color = Colors.textMain;
        TAlign? align = TAlign.Center;
        Draw.Text(str, 240.0, 44.0, stapler, color, null, null, null, align);

        foreach (Card card3 in cards)
        {
            State fakeState = DB.fakeState;
            bool instant = card3.targetPos == card3.pos;
            OnMouseDown? onMouseDown = (flourishTimer > 0.7) ? this : null;
            card3.Render(g, null, fakeState, ignoreAnim: false, ignoreHover: false, hideFace: false, hilight: false, instant, autoFocus: true, null, onMouseDown);
        }
    }

    public override bool TryCloseSubRoute(G g, Route r, object? arg)
    {
        return false;
    }
}
}