using JamesBrafin.Nichole.Artifacts;
using JamesBrafin.Nichole.Cards;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using JamesBrafin.Nichole.Cards.Potions;

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

    internal static IReadOnlyList<Type> Potion_CommonCard_Types { get; } = [
        typeof(AlchemistFire)
    ];

    public void LoadManifest(IStatusRegistry statusRegistry)
    {
        var enflame = new ExternalStatus("JamesBrafin.Nichole.Statuses.Enflame", true, System.Drawing.Color.Red, null, sprites["assets/statuses/heatAdd.png"], false);
        statusRegistry.RegisterStatus(enflame);
        redraw.AddLocalisation("Enflame", "Your attacks apply {0} heat until end of turn.");
        statuses["enflame"] = enflame;
    }

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;

        this.AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        this.Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
        );

        Potion_Default_CardBackground = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/potion/potion_default_background.png"));
        Potion_CardFrame = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/potion/potion_cardframe.png"));

        /* Decks are assigned separate of the character. This is because the game has decks like Trash which is not related to a playable character
         * Do note that Color accepts a HEX string format (like Color("a1b2c3")) or a Float RGB format (like Color(0.63, 0.7, 0.76). It does NOT allow a traditional RGB format (Meaning Color(161, 178, 195) will NOT work) */
        Potion_Deck = Helper.Content.Decks.RegisterDeck("PotionDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                /* This color is used in various situations. 
                 * It is used as the deck's rarity 'shine'
                 * If a playable character uses this deck, the character Name will use this color
                 * If a playable character uses this deck, the character mini panel will use this color */
                color = new Color("cd6a00"),

                /* This color is for the card name in-game
                 * Make sure it has a good contrast against the CardFrame, and take rarity 'shine' into account as well */
                titleColor = new Color("000000")
            },
            DefaultCardArt = Potion_Default_CardBackground,
            BorderSprite = Potion_CardFrame,

            /* Since this deck will be used by our Demo Character, we'll use their name. */
            Name = this.AnyLocalizations.Bind(["character", "DemoCharacter", "name"]).Localize,
        });
    }

    foreach (var cardType in DemoMod_AllCard_Types)
            AccessTools.DeclaredMethod(cardType, nameof(IDemoCard.Register))?.Invoke(null, [helper]);

}
