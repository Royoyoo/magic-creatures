using System.Collections.Generic;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    #region Fields

    private const string maxLevelReachedKey = "level_reached";
    private const string coinsKey = "coins";
    private const string creaturesKey = "creatures";


    [ShowInInspector] private int maxLevelReached;
    [ShowInInspector] private int coins;
    [ShowInInspector] private List<string> availableCreatures = new List<string>();


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

    public static IReadOnlyList<string> AvailableCreatures => Instance.availableCreatures;

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
        Instance.availableCreatures.Add(creatureId);

        PlayerPrefs.SetString(creaturesKey, JsonUtility.ToJson(Instance.availableCreatures));
        PlayerPrefs.Save();
    }


    private void LoadPlayerInfo()
    {
        maxLevelReached = PlayerPrefs.GetInt(maxLevelReachedKey, 1);
        coins = PlayerPrefs.GetInt(coinsKey, 0);

        string creaturesJson = PlayerPrefs.GetString(creaturesKey, "");

        if (!string.IsNullOrEmpty(creaturesJson))
        {
            availableCreatures = JsonUtility.FromJson<List<string>>(creaturesJson);
        }
        else
        {
            availableCreatures = new List<string>();
        }
    }

    #endregion
}
