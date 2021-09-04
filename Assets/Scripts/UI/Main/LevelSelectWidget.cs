using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectWidget : UiWidget
{
    #region Fields

    [SerializeField] private List<Button> levelButtons;

    #endregion



    #region Methods

    public override void Initialize()
    {
        for (int buttonIndex = 0; buttonIndex < levelButtons.Count; buttonIndex++)
        {
            Button levelButton = levelButtons[buttonIndex];
            int levelNumber = buttonIndex + 1;
            levelButton.onClick.AddListener(() => StartLevel(levelNumber));
        }

        UpdateButtons();
    }


    private void UpdateButtons()
    {
        for (int buttonIndex = 0; buttonIndex < levelButtons.Count; buttonIndex++)
        {
            Button levelButton = levelButtons[buttonIndex];
            levelButton.interactable = buttonIndex < PlayerInfo.MaxLevelReached;
        }
    }


    private void StartLevel(int levelNumber)
    {
        PlayerInfo.SelectedLevel = levelNumber;

        SceneManager.LoadScene("Level");
    }

    #endregion
}
