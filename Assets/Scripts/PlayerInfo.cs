using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    #region Nested classes

    [Serializable]
    public class Creature
    {
        public string Id;
        public int Level;
        public StatType statType;
    }

    [Serializable]
    public class HeroStats
    {
        public List<int> Stats = new List<int>();
    }

    #endregion



    #region Fields

    private static readonly Dictionary<string, StatType> CreatureByStatTypes = new Dictionary<string, StatType>()
    {
        { "creature_1", StatType.Strength },
        { "creature_2", StatType.Stamina },
        { "creature_3", StatType.Agility },
        { "creature_4", StatType.Intelligence },
        { "creature_5", StatType.Willpower }
    };

    private const string maxLevelReachedKey = "level_reached";
    private const string coinsKey = "coins";
    private const string heroStatsKey = "hero_stats";
    private const string creaturesKey = "creatures";


    [ShowInInspector] private int maxLevelReached;
    [ShowInInspector] private int coins;

    [SerializeField] private HeroStats heroStats;

    [ShowInInspector] private List<Creature> availableCreatures = new List<Creature>();
    [ShowInInspector] private List<Creature> activeParty = new List<Creature>();


    private static PlayerInfo Instance;

    #endregion



    #region Properties

    public static int MaxLevelReached
    {
        get => Instance.maxLevelReached;
        set
        {
            Instance.maxLevelReached = value;
            PlayerPrefs.SetInt(maxLevelReachedKey, Instance.maxLevelReached);
            PlayerPrefs.Save();
        }
    }

    public static int Coins
    {
        get => Instance.coins;
        set
        {
            Instance.coins = value;
            PlayerPrefs.SetInt(coinsKey, Instance.coins);
            PlayerPrefs.Save();
        }
    }

    public static IReadOnlyList<Creature> AvailableCreatures => Instance.availableCreatures;

    public static int SelectedLevel { get; set; } = 1;

    #endregion



    #region Methods

    public void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);

            return;
        }

        DontDestroyOnLoad(this.gameObject);

        LoadPlayerInfo();
    }


    public static void AddNewCreature(string creatureId)
    {
        Creature existingCreature = Instance.availableCreatures.Find(creature => creature.Id == creatureId);

        if (existingCreature != null)
        {
            return;
        }

        Creature creature = new Creature()
        {
            Id = creatureId,
            Level = 1,
            statType = CreatureByStatTypes[creatureId],
        };

        Instance.availableCreatures.Add(creature);

        SaveAllCreaturesInfo();
    }


    public static void UpgradeHeroStat(StatType statType, int value)
    {
        Instance.heroStats.Stats[(int)statType] += value;
        SaveHeroStats();
    }


    public static int GetHeroStat(StatType statType)
    {
        return Instance.heroStats.Stats[(int)statType];
    }


    public static void UpgradeCreature(string creatureId)
    {
        Creature existingCreature = GetCreature(creatureId);
        existingCreature.Level++;

        SaveAllCreaturesInfo();
    }


    public static Creature GetCreature(string creatureId)
    {
        Creature existingCreature = Instance.availableCreatures.Find(creature => creature.Id == creatureId);

        if (existingCreature == null)
        {
            throw new ArgumentException("Creature with this Id is unavailable");
        }

        return existingCreature;
    }


    private static void SaveAllCreaturesInfo()
    {
        PlayerPrefs.SetString(creaturesKey, JsonUtility.ToJson(Instance.availableCreatures));
        PlayerPrefs.Save();
    }


    private static void SaveHeroStats()
    {
        PlayerPrefs.SetString(heroStatsKey, JsonUtility.ToJson(Instance.heroStats));
        PlayerPrefs.Save();
    }


    private void LoadPlayerInfo()
    {
        maxLevelReached = PlayerPrefs.GetInt(maxLevelReachedKey, 1);
        coins = PlayerPrefs.GetInt(coinsKey, 0);

        string heroStatsJson = PlayerPrefs.GetString(heroStatsKey, "");

        if (!string.IsNullOrEmpty(heroStatsJson))
        {
            heroStats = JsonUtility.FromJson<HeroStats>(heroStatsJson);
        }
        else
        {
            heroStats = new HeroStats
            {
                Stats = new List<int> { 10, 10, 10, 10, 10 },
            };

            SaveHeroStats();
        }

        string creaturesJson = PlayerPrefs.GetString(creaturesKey, "");

        availableCreatures = !string.IsNullOrEmpty(creaturesJson) ?
            JsonUtility.FromJson<List<Creature>>(creaturesJson) :
            new List<Creature>();
    }

    #endregion



    #region Editor

    [Button]
    public void Add500Coins()
    {
        Coins += 500;
    }


    [Button]
    public void AddAllCreatures()
    {
        AddNewCreature("creature_1");
        AddNewCreature("creature_2");
        AddNewCreature("creature_3");
        AddNewCreature("creature_4");
        AddNewCreature("creature_5");
    }

    #endregion
}
