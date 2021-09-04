using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameWidget : UiWidget
{
    #region Fields

    [SerializeField] private CommonLevel commonLevel;
    [SerializeField] private TextMeshProUGUI infoText;

    [SerializeField] private Button winButton;
    [SerializeField] private Button loseButton;

    #endregion



    #region Unity lifecycle

    private void Update()
    {
        infoText.text = $"Creatures: {commonLevel.CreaturesCaught} / {commonLevel.CreaturesToWin}\n" +
                        $" Time left: {commonLevel.TimeLeft:F0}sec";
    }

    #endregion



    #region Methods

    public override void Initialize()
    {
        winButton.onClick.AddListener(LevelManager.WinLevel);
        loseButton.onClick.AddListener(LevelManager.LoseLevel);
    }

    #endregion
}
