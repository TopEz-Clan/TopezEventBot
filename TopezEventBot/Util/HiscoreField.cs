namespace TopezEventBot.Util;

public enum HiscoreField {
    Overall,
    Attack,
    Defence,
    Strength,
    Hitpoints,
    Ranged,
    [Lookup("Prayer", "https://i.imgur.com/sPgbZjc.png", "https://i.imgur.com/S8KpQVe.png")]
    Prayer,
    Magic,
    [Lookup("Cooking", "https://i.imgur.com/veanoTh.png", "https://i.imgur.com/6WgWuxu.png")]
    Cooking,
    [Lookup("Woodcutting", "https://i.imgur.com/MwhgCyZ.png", "https://i.imgur.com/cKpxPzl.png")]
    Woodcutting,
    [Lookup("Fletching", "https://i.imgur.com/txuRrQz.png", "https://i.imgur.com/flCanBd.png")]
    Fletching,
    [Lookup("Fishing", "https://i.imgur.com/wQlLo8s.png", "https://i.imgur.com/yL4o7ln.png")]
    Fishing,
    [Lookup("Firemaking", "https://i.imgur.com/MCDr9Yl.png", "https://i.imgur.com/FYHTmeI.png")]
    Firemaking,
    [Lookup("Crafting", "https://i.imgur.com/Efd7ApV.png", "https://i.imgur.com/bddiWaW.png")]
    Crafting,
    [Lookup("Smithing", "https://i.imgur.com/KHNdKcl.png", "https://i.imgur.com/zVDCb4N.png")]
    Smithing,
    [Lookup("Mining", "https://i.imgur.com/KTMPqPq.png", "https://i.imgur.com/rKJkn02.png")]
    Mining,
    [Lookup("Herblore", "https://i.imgur.com/qVeJhCn.png", "https://i.imgur.com/2qeypvz.png")]
    Herblore,
    [Lookup("Agility", "https://i.imgur.com/R2KzwvK.png", "https://i.imgur.com/8MrFHfm.png")]
    Agility,
    [Lookup("Thieving", "https://i.imgur.com/2qoSRlg.png", "https://i.imgur.com/RQxgjzC.png")]
    Thieving,
    [Lookup("Slayer", "https://i.imgur.com/x0P2tSX.png", "https://i.imgur.com/pImROQo.png")]
    Slayer,
    [Lookup("Farming", "https://i.imgur.com/tmiOasj.png", "https://i.imgur.com/UEj3qda.png")]
    Farming,
    [Lookup("Runecrafting", "https://i.imgur.com/RIXW2F2.png", "https://i.imgur.com/Ood3WVh.png")]
    Runecraft,
    [Lookup("Hunter", "https://i.imgur.com/hLSo4XK.png", "https://i.imgur.com/M9f1FYv.png")]
    Hunter,
    [Lookup("Construction", "https://i.imgur.com/EwaRGOX.png", "https://i.imgur.com/YlUrD8q.png")]
    Construction,
    LeaguePoints,
    DeadmanPoints,
    BountyHunterHunter,
    BountyHunterRogue,
    BountyHunterLegacyHunter,
    BountyHunterLegacyRogue,
    ClueScrollsAll,
    ClueScrollsBeginner,
    ClueScrollsEasy,
    ClueScrollsMedium,
    ClueScrollsHard,
    ClueScrollsElite,
    ClueScrollsMaster,
    [Lookup(  "Last Man Standing", "https://i.imgur.com/2YS9E4a.png", "https://i.imgur.com/gZ1LKz7.png"  )]
    LMSRank,
    PvPArenaRank,
    SoulWarsZeal,
    [Lookup( "Guardians of the Rift", "https://i.imgur.com/A9Ggm2X.jpg", "https://i.imgur.com/KYOY141.png" )]
    Riftsclosed,
    AbyssalSire,
    AlchemicalHydra,
    [Lookup( "Artio", "https://i.imgur.com/VxpHZ1V.png", "https://i.imgur.com/FVqirmd.png" )]
    Artio,
    [Lookup( "Barrows", "https://i.imgur.com/8Tol65I.png", "https://i.imgur.com/4LUPBzX.png" )]
    BarrowsChests,
    Bryophyta,
    [Lookup( "Callisto", "https://i.imgur.com/VxpHZ1V.png", "https://i.imgur.com/Updm0jp.png" )]
    Callisto,
    [Lookup( "Calvarion", "https://i.imgur.com/fvdt3PK.png", "https://i.imgur.com/Y9kFHoL.png" )]
    Calvarion,
    Cerberus,
    [Lookup( "Chambers of Xeric", "https://i.imgur.com/9d3NEkX.png", "https://i.imgur.com/NtAJQGW.png" )]
    ChambersofXeric,
    [Lookup( "Chambers of Xeric", "https://i.imgur.com/9d3NEkX.png", "https://i.imgur.com/NtAJQGW.png" )]
    ChambersofXericChallengeMode,
    ChaosElemental,
    [Lookup( "Chaos Fanatic", "https://i.imgur.com/ITYwpqj.png", "https://i.imgur.com/tTwnZG4.png" )]
    ChaosFanatic,
    [Lookup( "Saradomin GWD", "https://i.imgur.com/WhbxRtV.png", "https://i.imgur.com/3ZyN8BC.png" )]
    CommanderZilyana,
    [Lookup( "Corporeal Beast", "https://i.imgur.com/bemRz9B.png", "https://i.imgur.com/stYUpXw.png" )]
    CorporealBeast,
    [Lookup( "Crazy Archaeologist", "https://i.imgur.com/OUw4BFq.png", "https://i.imgur.com/FOrwuvQ.png" )]
    CrazyArchaeologist,
    [Lookup( "Dagannoth Prime", "https://i.imgur.com/7cf7ELM.png", "https://i.imgur.com/hHTJxvz.png" )]
    DagannothPrime,
    [Lookup( "Dagannoth Rex", "https://i.imgur.com/tJZ2sPY.png", "https://i.imgur.com/hHTJxvz.png" )]
    DagannothRex,
    [Lookup( "Dagannoth Supreme", "https://i.imgur.com/56NSask.png", "https://i.imgur.com/hHTJxvz.png" )]
    DagannothSupreme,
    [Lookup( "Deranged Archaeologist", "https://i.imgur.com/grDg2C1.png", "https://i.imgur.com/vZxrh5U.png" )]
    DerangedArchaeologist,
    [Lookup( "Duke Sucellus", "https://i.imgur.com/XzrvYAL.png", "https://i.imgur.com/hmqZeVa.png" )]
    DukeSucellus,
    [Lookup( "Bandos GWD", "https://i.imgur.com/t1lLIse.png", "https://i.imgur.com/uNZRIPw.png" )]
    GeneralGraardor,
    [Lookup( "Giant Mole", "https://i.imgur.com/XtDPimm.png", "https://i.imgur.com/ObfQaO2.png" )]
    GiantMole,
    [Lookup( "Grotesque Guardians", "https://i.imgur.com/AR4n18J.png", "https://i.imgur.com/XFumhAz.png" )]
    GrotesqueGuardians,
    Hespori,
    [Lookup( "Kalphite Queen", "https://i.imgur.com/k3FfBM8.png", "https://i.imgur.com/H81C31H.png" )]
    KalphiteQueen,
    [Lookup( "King Black Dragon", "https://i.imgur.com/WWUxEfW.png", "https://i.imgur.com/qGXjmtG.png" )]
    KingBlackDragon,
    Kraken,
    [Lookup( "Armadyl GWD", "https://i.imgur.com/a5HpGO8.png", "https://i.imgur.com/ryBcAYh.png" )]
    KreeArra,
    [Lookup( "Zamorak GWD", "https://i.imgur.com/FJwK4Nw.png", "https://i.imgur.com/l8hTicu.png" )]
    KrilTsutsaroth,
    Mimic,
    [Lookup( "Nex", "https://i.imgur.com/CiBoJbG.png", "https://i.imgur.com/HnPmxHg.png" )]
    Nex,
    [Lookup( "Nightmare", "https://i.imgur.com/ixICLrF.png", "https://i.imgur.com/SL9PU4g.png" )]
    Nightmare,
    [Lookup( "Nightmare", "https://i.imgur.com/ixICLrF.png", "https://i.imgur.com/SL9PU4g.png" )]
    PhosanisNightmare,
    Obor,
    [Lookup( "Phantom Muspah", "https://i.imgur.com/KhqXf1x.png", "https://i.imgur.com/oxYKULf.png" )]
    PhantomMuspah,
    [Lookup( "Sarachnis", "https://i.imgur.com/BJgzIuj.png", "https://i.imgur.com/oR6HPLV.png" )]
    Sarachnis,
    [Lookup( "Scorpia", "https://i.imgur.com/nXfvdFv.png", "https://i.imgur.com/jezO98a.png" )]
    Scorpia,
    Skotizo,
    [Lookup( "Spindel", "https://i.imgur.com/EKDLfKX.png", "https://i.imgur.com/Hdp9rDa.png" )]
    Spindel,
    [Lookup( "Tempoross", "https://i.imgur.com/hquECGM.png", "https://i.imgur.com/uiay8qc.png" )]
    Tempoross,
    [Lookup( "The Gauntlet", "https://i.imgur.com/8xJ2rzq.png", "https://i.imgur.com/nJWGi4G.png" )]
    TheGauntlet,
    [Lookup( "Corrupted Gauntlet", "https://i.imgur.com/8xJ2rzq.png", "https://i.imgur.com/nJWGi4G.png" )]
    TheCorruptedGauntlet,
    [Lookup( "Leviathan", "https://i.imgur.com/3jZBoDY.png", "https://i.imgur.com/Nv8shff.png" )]
    TheLeviathan,
    [Lookup( "The Whisperer", "https://i.imgur.com/T41Rn0k.png", "https://i.imgur.com/GR7Ia6P.png" )]
    TheWhisperer,
    [Lookup( "Theatre of Blood", "https://i.imgur.com/ftJDmVR.png", "https://i.imgur.com/gmU7mv7.png" )]
    TheatreofBlood,
    [Lookup( "Theatre of Blood HM", "https://i.imgur.com/ftJDmVR.png", "https://i.imgur.com/gmU7mv7.png" )]
    TheatreofBloodHardMode,
    ThermonuclearSmokeDevil,
    [Lookup( "Tombs of Amascut", "https://i.imgur.com/pUxb27L.png", "https://i.imgur.com/kYxBoKG.png" )]
    TombsofAmascut,
    [Lookup( "Tombs of Amascut Expert Mode", "https://i.imgur.com/pUxb27L.png", "https://i.imgur.com/kYxBoKG.png" )]
    TombsofAmascutExpertMode,
    [Lookup( "The Inferno", "https://i.imgur.com/d7BhWGX.png", "https://i.imgur.com/nGAXKiE.png" )]
    TzKalZuk,
    [Lookup( "Jad", "https://i.imgur.com/usX4A18.png", "https://i.imgur.com/J4uB7HE.png" )]
    TzTokJad,
    [Lookup( "Vardorvis", "https://i.imgur.com/rLsesQR.png", "https://i.imgur.com/89DVRJy.png" )]
    Vardorvis,
    [Lookup( "Venenatis", "https://i.imgur.com/EKDLfKX.png", "https://i.imgur.com/quPP0yX.png" )]
    Venenatis,
    [Lookup( "Vetion", "https://i.imgur.com/fvdt3PK.png", "https://i.imgur.com/HLk1j1Y.png" )]
    Vetion,
    [Lookup( "Vorkath", "https://i.imgur.com/cj5GP9d.png", "https://i.imgur.com/66e8v4v.png" )]
    Vorkath,
    [Lookup( "Wintertodt", "https://i.imgur.com/9UXMJww.png", "https://i.imgur.com/F895yep.png" )]
    Wintertodt,
    [Lookup( "Zalcano", "https://i.imgur.com/nRpddEi.png", "https://i.imgur.com/Su1Uzr2.png" )]
    Zalcano,
    [Lookup( "Zulrah", "https://i.imgur.com/RuLHQEk.png", "https://i.imgur.com/lZ6Soqq.jpeg" )]
    Zulrah,
}