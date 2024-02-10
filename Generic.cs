using System;
using System.Collections.Generic;

namespace JamesBrafin.Nichole;

public partial interface IKokoroApi : IProxyProvider
{
	TimeSpan TotalGameTime { get; }
	IEnumerable<Card> GetCardsEverywhere(State state, bool hand = true, bool drawPile = true, bool discardPile = true, bool exhaustPile = true);
}

public interface IHookPriority
{
	double HookPriority { get; }
}