using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureInfoWidget : UiWidget
{
    #region Fields

    public event Action OnCreatureUpgraded;


    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI statBonus;
    [SerializeField] private TextMeshProUGUI cardsText;
    [SerializeField] private Image cardsBar;
    [SerializeField] private Button upgradeButton;


    private CreatureInfo creature;

    #endregion



    #region Methods

    public override void Initialize()
    {
        upgradeButton.onClick.AddListener(TryUpgrade);
    }


    public void SetCreature(CreatureInfo creature)
    {
        this.creature = creature;

        nameText.text = creature.Id;

        UpdateInfo();
    }


    public void UpdateInfo()
    {
        upgradeButton.gameObject.SetActive(creature.CardsCount >= creature.CardsToLevelUp);

        cardsText.text = $"{creature.CardsCount} / {creature.CardsToLevelUp}";
        cardsBar.fillAmount = (float)creature.CardsCount / creature.CardsToLevelUp;

        if (creature.Level == 0)
        {
            HideInfo();

            return;
        }

        level.text = $"{creature.Level} Lvl";
        statBonus.text = $"+{creature.StatBonus} {creature.statType}";
    }


    private void HideInfo()
    {
        level.text = "???";
        statBonus.text = "";
    }


    private void TryUpgrade()
    {
        if (creature == null)
        {
            return;
        }

        if (PlayerInfo.TryUpgradeCreature(creature.Id))
        {
            OnCreatureUpgraded?.Invoke();
        }
    }

    #endregion
}
