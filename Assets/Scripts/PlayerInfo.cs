using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    #region Nested classes

    [Serializable]
    public class HeroStats
    {
        public List<int> Stats = new List<int>();
    }

    #endregion



    #region Fields

    public static readonly List<string> AllAvailableCreatures = new List<string>()
    {
        "creature_1",
        "creature_2",
        "creature_3",
        "creature_4",
        "creature_5",
    };


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
    private const string creatureKeyPrefix = "creature";


    [ShowInInspector] private int maxLevelReached;
    [ShowInInspector] private int coins;

    [SerializeField] private HeroStats heroStats;

    [ShowInInspector] private List<CreatureInfo> availableCreatures = new List<CreatureInfo>();
    [ShowInInspector] private List<CreatureInfo> activeParty = new List<CreatureInfo>();


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

    public static IReadOnlyList<CreatureInfo> AvailableCreatures => Instance.availableCreatures;

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
        CreatureInfo existingCreatureInfo = Instance.availableCreatures.Find(creature => creature.Id == creatureId);

        if (existingCreatureInfo != null)
        {
            return;
        }

        CreatureInfo creatureInfo = new CreatureInfo()
        {
            Id = creatureId,
            Level = 0,
            statType = CreatureByStatTypes[creatureId],
        };

        Instance.availableCreatures.Add(creatureInfo);

        SaveCreatureInfo(creatureId);
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


    public static bool TryUpgradeCreature(string creatureId)
    {
        CreatureInfo creatureInfo = GetCreature(creatureId);

        if (creatureInfo.CardsCount < creatureInfo.CardsToLevelUp)
        {
            return false;
        }

        creatureInfo.CardsCount -= creatureInfo.CardsToLevelUp;
        creatureInfo.Level++;

        SaveCreatureInfo(creatureId);

        return true;
    }


    public static void AddCreatureCards(string creatureId, int amount)
    {
        CreatureInfo creatureInfo = GetCreature(creatureId);
        creatureInfo.CardsCount += amount;

        SaveCreatureInfo(creatureId);
    }


    public static int GetCreaturesBonusForStat(StatType statType)
    {
        int bonus = 0;

        for (int i = 0; i < AvailableCreatures.Count; i++)
        {
            if (AvailableCreatures[i].statType == statType)
            {
                bonus += AvailableCreatures[i].StatBonus;
            }
        }

        return bonus;
    }


    public static CreatureInfo GetCreature(string creatureId)
    {
        CreatureInfo existingCreatureInfo = Instance.availableCreatures.Find(creature => creature.Id == creatureId);

        if (existingCreatureInfo == null)
        {
            throw new ArgumentException("Creature with this Id is unavailable");
        }

        return existingCreatureInfo;
    }


    private static void SaveCreatureInfo(string creatureId)
    {
        string creatureKey = $"{creatureKeyPrefix}_{creatureId}";

        PlayerPrefs.SetString(creatureKey, JsonUtility.ToJson(GetCreature(creatureId)));
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

        for (int i = 0; i < AllAvailableCreatures.Count; i++)
        {
            string creatureKey = $"{creatureKeyPrefix}_{AllAvailableCreatures[i]}";

            string creatureJson = PlayerPrefs.GetString(creatureKey, null);

            if (!string.IsNullOrEmpty(creatureJson))
            {
                CreatureInfo creatureInfo = JsonUtility.FromJson<CreatureInfo>(creatureJson);
                Instance.availableCreatures.Add(creatureInfo);
            }
            else
            {
                AddNewCreature(AllAvailableCreatures[i]);
            }
        }

        //TODO Make 'availableCreatures' saveable!!!

        // string creaturesJson = PlayerPrefs.GetString(creaturesKey, "");

        // availableCreatures = JsonUtility.FromJson<List<CreatureInfo>>(creaturesJson);
        //
        // if (availableCreatures == null)
        // {
        // availableCreatures = new List<CreatureInfo>();
        // AddAllCreatures();

        // }
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
