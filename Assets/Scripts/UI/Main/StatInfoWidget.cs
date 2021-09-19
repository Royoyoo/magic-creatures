using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatInfoWidget : UiWidget
{
    #region Fields

    private const int UpgradePrice = 200;
    public event Action OnStatUpgraded;


    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI totalStatText;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;
    [SerializeField] private Button upgradeButton;

    #endregion



    #region Methods

    public override void Initialize()
    {
        statName.text = statType.ToString();
        upgradeButtonText.text = $"Upgrade\n{UpgradePrice}$";
        
        UpdateStat();

        upgradeButton.onClick.AddListener(TryUpgradeStat);
    }


    public void UpdateStat()
    {
        int heroStat = PlayerInfo.GetHeroStat(statType);
        int activePartyStat = 0;

        totalStatText.text = $"{heroStat} + {activePartyStat} = {heroStat + activePartyStat}";

        upgradeButton.interactable = PlayerInfo.Coins >= UpgradePrice;
    }


    private void TryUpgradeStat()
    {
        if (PlayerInfo.Coins < UpgradePrice)
        {
            return;
        }

        PlayerInfo.Coins -= UpgradePrice;
        PlayerInfo.UpgradeHeroStat(statType, 1);

        OnStatUpgraded?.Invoke();
    }

    #endregion
}
