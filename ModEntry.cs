
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using JamesBrafin.Nichole.Cards.Potions;
using JamesBrafin.Nichole.Cards.Nichole;
using RandallMod;
using JamesBrafin.Nichole.Artifacts;
using JamesBrafin.Nichole.Actions;


namespace JamesBrafin.Nichole;

public sealed class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;

    internal IKokoroApi KokoroApi { get; private set; } = null!;
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    internal IStatusEntry Enflame { get; }
    internal IStatusEntry Cryo { get; }
    internal IStatusEntry AcidTip { get; }
    internal IStatusEntry AcidSource { get; }
    internal IStatusEntry SuperStun { get; }
    internal IStatusEntry PotionSaver { get; }
    internal IDeckEntry Potion_Deck { get; }
    internal IDeckEntry Nichole_Deck { get; }
    internal ISpriteEntry Nichole_Character_Panel {  get; }
    internal ISpriteEntry Nichole_Neutral_0 { get; }
    internal ISpriteEntry Nichole_Mini_0 { get; }
    internal ISpriteEntry Nichole_Squint_0 { get; }
    internal ISpriteEntry Foresight { get; }

    internal static IReadOnlyList<Type> Potion_StartingCards { get; } = [
        typeof(AlchemistFire),
        typeof(FreezerBomb),
        typeof(SwiftnessPotion)
    ];

    internal static IReadOnlyList<Type> Potion_notStartingCards { get; } = [
        typeof(ExtraReagents),
        typeof(DraughtOfCommand),
        typeof(BottleOfRocks),
        typeof(EnergyDrink),
        typeof(SightPotion),
        typeof(StrengthPotion),
        typeof(WrathPotion),
        typeof(CanOfMissiles),
        typeof(SteelshardGrenade),
        typeof(IronskinPotion),
        typeof(AcidCoating),
        typeof(ConfusionDraught),
        typeof(ShockWater),
        typeof(PotionBooster),
        typeof(WeakeningQuaff)
    ];

    internal static IReadOnlyList<Type> Nichole_StarterCard_Types { get; } = [
        typeof(InfusedReagents),
        typeof(IgnitionStrike)
    ];

    internal static IReadOnlyList<Type> Nichole_CommonCard_Types { get; } = [
        typeof(InfusedReagents),
        typeof(PreparedBatch),
        typeof(PotionLob),
        typeof(ChemicalBooster),
        typeof(PotionPrep),
        typeof(IgnitionStrike),
        typeof(FranticSearch),
        typeof(EnemyEnhancement),
        typeof(ChillingStrike)
    ];

    internal static IReadOnlyList<Type> Nichole_UnommonCard_Types { get; } = [
        typeof(SplitBatch),
        typeof(AcidSplash),
        typeof(DilutedDose),
        typeof(SuddenInspiration),
        typeof(AdvancedBrewing),
        typeof(PreciseMixture),
        typeof(ShockingStrike),
    ];

    internal static IReadOnlyList<Type> Nichole_RareCard_Types { get; } = [
        typeof(PhilosophersStone),
        typeof(RapidToss),
        typeof(DrawOnReserves),
        typeof(Eureka),
        typeof(FlameBlast)
    ];

    internal static IEnumerable<Type> Potion_AllCard_Types
        => Potion_StartingCards
        .Concat(Potion_notStartingCards);

    internal static IEnumerable<Type> Nichole_AllCard_Types
        => Nichole_CommonCard_Types
        .Concat(Nichole_UnommonCard_Types)
        .Concat(Nichole_RareCard_Types);

    internal static IReadOnlyList<Type> Nichole_CommonArtifact_Types { get; } = [
        typeof(RecipieBook),
        typeof(PotionBelt),
        typeof(ElixirPoppers)
    ];

    internal static IReadOnlyList<Type> Nichole_BossArtifact_Types { get; } = [
        typeof(LiquidExtender),
        typeof(ExtraHands),
        typeof(AlchemicalEngine)
    ];
    internal static IEnumerable<Type> Nichole_AllArtifact_Types
        => Nichole_CommonArtifact_Types
        .Concat(Nichole_BossArtifact_Types);

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!;

        this.AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        this.Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
        );

        Enflame = Helper.Content.Statuses.RegisterStatus("Enflame", new()
        {
            Definition = new()
            {
                icon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/statuses/heatAdd.png")).Sprite,
                color = new("ff4444"),
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
                color = new("cbf9ff"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "Cryo", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "Cryo", "description"]).Localize
        });

        AcidTip = Helper.Content.Statuses.RegisterStatus("AcidTip", new()
        {
            Definition = new()
            {
                icon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/statuses/AcidTip.png")).Sprite,
                color = new("84f408"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "AcidTip", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "AcidTip", "description"]).Localize
        });

        AcidSource = Helper.Content.Statuses.RegisterStatus("AcidSource", new()
        {
            Definition = new()
            {
                icon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/statuses/AcidSource.png")).Sprite,
                color = new("84f408"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "AcidSource", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "AcidSource", "description"]).Localize
        });

        SuperStun = Helper.Content.Statuses.RegisterStatus("SuperStun", new()
        {
            Definition = new()
            {
                icon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/statuses/SuperStun.png")).Sprite,
                color = new("b500be"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "SuperStun", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "SuperStun", "description"]).Localize
        });

        PotionSaver = Helper.Content.Statuses.RegisterStatus("PotionSaver", new()
        {
            Definition = new()
            {
                icon = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/statuses/PotionSaver.png")).Sprite,
                color = new("801cc0"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "PotionSaver", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "PotionSaver", "description"]).Localize
        });

        _ = new AcidTipManager();
        _ = new AcidSourceManager();

        var Potion_Default_CardBackground = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/potion/potion_default_background.png")).Sprite;
        var Potion_CardFrame = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/potion/potion_cardframe.png")).Sprite;
        var Nichole_Default_CardBackground = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/Nichole/nichole_default_background.png")).Sprite;
        var Nichole_CardFrame = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/Nichole/nichole_cardframe.png")).Sprite;

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
                color = new Color("abf5ff"),

                /* This color is for the card name in-game
                 * Make sure it has a good contrast against the CardFrame, and take rarity 'shine' into account as well */
                titleColor = new Color("000000")
            },
            DefaultCardArt = Potion_Default_CardBackground,
            BorderSprite = Potion_CardFrame,

            Name = this.AnyLocalizations.Bind(["deck", "Potions", "name"]).Localize,
        });

        Nichole_Deck = Helper.Content.Decks.RegisterDeck("NicholeDeck", new DeckConfiguration()
        {

            Definition = new DeckDef()
            {
                color = new Color("f8d280"),
                titleColor = new Color("000000")
            },
            DefaultCardArt = Nichole_Default_CardBackground,
            BorderSprite = Nichole_CardFrame,

            Name = this.AnyLocalizations.Bind(["character", "Nichole", "name"]).Localize,
        });

        Foresight = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/statuses/foresight.png"));

        Nichole_Character_Panel = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/nichole_panel.png"));
        Nichole_Neutral_0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/neutral/Nichole_Neutral_0.png"));
        Nichole_Mini_0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/Nichole_Mini.png"));
        Nichole_Squint_0 = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/squint/Nichole_Squint_0.png"));

        Helper.Content.Characters.RegisterCharacter("Nichole", new CharacterConfiguration()
        {
            /* What we registered above was an IDeckEntry object, but when you register a character the Helper will ask for you to provide its Deck 'id'
             * This is simple enough, as you can get it from DemoMod_Deck */
            Deck = Nichole_Deck.Deck,

            /* The Starter Card Types are, as the name implies, the cards you will start a DemoCharacter run with. 
             * You could provide vanilla cards if you want, but it's way more fun to create your own cards! */
            StarterCardTypes = Nichole_StarterCard_Types,

            StarterArtifactTypes = new Type[] { typeof(RecipieBook) },

            /* This is the little blurb that appears when you hover over the character in-game.
             * You can make it fluff, use it as a way to tell players about the character's playstyle, or a little bit of both! */
            Description = this.AnyLocalizations.Bind(["character", "Nichole", "description"]).Localize,

            /* This is the fancy panel that encapsulates your character while in active combat.
             * It's recommended that it follows the same color scheme as the character and deck, for cohesion */
            BorderSprite = Nichole_Character_Panel.Sprite,

            NeutralAnimation = new()
            {
                /* Characters themselves aren't used that much by the code itself, most of the time we care about having the character's deck at hand */
                Deck = Nichole_Deck.Deck,

                /* The Looptag is the 'name' of the animation. When making shouts and events, and you want your character to show emotions, the LoopTag is what you want
                 * In vanilla Cobalt Core, there are 3 'animations' looptags that any character should have: "neutral", "mini" and "squint", as these are used in: Neutral is used as default, mini is used in character select and out-of-combat UI, and Squink is hardcoded used in certain events */
                LoopTag = "neutral",

                /* The game doesn't use frames properly when there are only 2 or 3 frames. If you want a proper animation, avoid only adding 2 or 3 frames to it */
                Frames = new[]
                {
                    Nichole_Neutral_0.Sprite,
                    Nichole_Neutral_0.Sprite,
                    Nichole_Neutral_0.Sprite,
                    Nichole_Neutral_0.Sprite,
                    Nichole_Neutral_0.Sprite
                }
            },

            MiniAnimation = new()
            {
                Deck = Nichole_Deck.Deck,
                LoopTag = "mini",
                Frames = new[]
                {
                    /* Mini only needs one sprite. We call it animation just because we add it the same way as other expressions. */
                    Nichole_Mini_0.Sprite
                }
            }

        });

        Helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Nichole_Deck.Deck,
            LoopTag = "squint",
            Frames = new[]
            {
                Nichole_Squint_0.Sprite,
                Nichole_Squint_0.Sprite,
                Nichole_Squint_0.Sprite,
                Nichole_Squint_0.Sprite,
            }
        });

        foreach (var cardType in Potion_AllCard_Types)
            AccessTools.DeclaredMethod(cardType, nameof(PotionCard.Register))?.Invoke(null, [helper]);

        foreach (var cardType in Nichole_AllCard_Types)
            AccessTools.DeclaredMethod(cardType, nameof(NicholeCard.Register))?.Invoke(null, [helper]);

        foreach (var artifactType in Nichole_AllArtifact_Types)
            AccessTools.DeclaredMethod(artifactType, nameof(NicholeArtifact.Register))?.Invoke(null, [helper]);

        var harmony = new Harmony("Nichole");
        harmony.PatchAll();
        CustomTTGlossary.ApplyPatches(harmony);
        AResearch.ApplyPatches(harmony);
        ASpecificRandCardOffering.ApplyPatches(harmony);

        
    }
}
