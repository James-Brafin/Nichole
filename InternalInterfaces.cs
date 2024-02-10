using Nickel;
using System.Diagnostics.CodeAnalysis;

namespace JamesBrafin.Nichole;
internal interface PotionCard
{
    static abstract void Register(IModHelper helper);
}

internal interface NicholeCard
{
    static abstract void Register(IModHelper helper);
}

internal interface NicholeArtifact
{
    static abstract void Register(IModHelper helper);
}

public interface IProxyProvider
{
    bool TryProxy<T>(object @object, [MaybeNullWhen(false)] out T proxy) where T : class;
}
