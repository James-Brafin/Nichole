using JamesBrafin.Nichole.Artifacts;
using JamesBrafin.Nichole.Cards;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;

/* In the Cobalt Core modding community it is common for namespaces to be <Author>.<ModName>
 * This is helpful to know at a glance what mod we're looking at, and who made it */
namespace JamesBrafin.Nichole;

/* ModEntry is the base for our mod. Others like to name it Manifest, and some like to name it <ModName>
 * Notice the ': SimpleMod'. This means ModEntry is a subclass (child) of the superclass SimpleMod (parent). This is help us use Nickel's functions more easily! */
public sealed class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }

    public void LoadManifest(IStatusRegistry statusRegistry)
    {
        var enflame = new ExternalStatus("JamesBrafin.Nichole.Statuses.Enflame", true, System.Drawing.Color.Red, null, sprites["placeholder"], false);
        statusRegistry.RegisterStatus(enflame);
        redraw.AddLocalisation("Enflame", "Your attacks apply {0} heat until end of turn.");
        statuses["enflame"] = enflame;
    }

}
