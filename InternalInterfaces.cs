using Nickel;

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
