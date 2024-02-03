using JamesBrafin.Nichole.Cards;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using JamesBrafin.Nichole.Cards.Potions;


namespace JamesBrafin.Nichole;

public sealed class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    internal IStatusEntry Enflame { get; }
    internal IStatusEntry Cryo { get; }
    internal IDeckEntry Potion_Deck { get; }

    internal static IReadOnlyList<Type> Potion_CommonCard_Types { get; } = [
        typeof(AlchemistFire),
        typeof(FreezerBomb),
        typeof(SwiftnessPotion),
        typeof(ExtraReagents)
    ];

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

        Enflame = Helper.Content.Statuses.RegisterStatus("TempShieldNextTurn", new()
        {
            Definition = new()
            {
                icon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/statuses/heatAdd.png")).Sprite,
                color = new("b500be"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "Enflame", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "Enflame", "description"]).Localize
        });

        Cryo = Helper.Content.Statuses.RegisterStatus("Cryo", new()
        {
            Definition = new()
            {
                icon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/statuses/cryo.png")).Sprite,
                color = new("b500be"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "Cryo", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "Cryo", "description"]).Localize
        });

        var Potion_Default_CardBackground = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/potion/potion_default_background.png")).Sprite;
        var Potion_CardFrame = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/potion/potion_cardframe.png")).Sprite;

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
}
