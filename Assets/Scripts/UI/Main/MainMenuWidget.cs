using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWidget : UiWidget
{
    #region Fields

    [SerializeField] private Button levelsButton;
    [SerializeField] private Button heroButton;
    [SerializeField] private Button partyButton;
    [SerializeField] private Button homeButton;

    #endregion



    #region Methods

    public override void Initialize()
    {
        levelsButton.onClick.AddListener(() => ShowScreen(UiWidgetType.LevelSelect));
        heroButton.onClick.AddListener(() => ShowScreen(UiWidgetType.HeroStats));
        partyButton.onClick.AddListener(() => ShowScreen(UiWidgetType.PartySelect));
        homeButton.onClick.AddListener(() => ShowScreen(UiWidgetType.HomeView));

        UpdateButtons();
    }


    private void UpdateButtons()
    {
        levelsButton.interactable = !UiManager.IsUiActive(UiWidgetType.LevelSelect);
        heroButton.interactable = !UiManager.IsUiActive(UiWidgetType.HeroStats);
        partyButton.interactable = !UiManager.IsUiActive(UiWidgetType.PartySelect);
        homeButton.interactable = !UiManager.IsUiActive(UiWidgetType.HomeView);
    }


    private void ShowScreen(UiWidgetType uiType)
    {
        HideCurrent();
        UiManager.Show(uiType);
        UpdateButtons();
    }


    private void HideCurrent()
    {
        UiManager.HideAll(new List<UiWidgetType> { UiWidgetType.CoinsCounter, UiWidgetType.MainMenu });
    }

    #endregion
}
