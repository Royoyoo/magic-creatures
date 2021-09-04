using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelResultWidget : UiWidget
{
    #region Fields

    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Button ExitButton;


    private int rewardCount = 0;

    #endregion



    #region Methods

    public override void Initialize() { }


    public void ShowResult(bool isWon)
    {
        winText.SetActive(isWon);
        loseText.SetActive(!isWon);

        rewardText.gameObject.SetActive(isWon);
        rewardCount = PlayerInfo.SelectedLevel * 100;
        rewardText.text = $"Reward: {rewardCount}$";

        ExitButton.onClick.AddListener(() => ExitLevel(isWon));
    }


    private void ExitLevel(bool isWon)
    {
        if (isWon)
        {
            PlayerInfo.Coins += rewardCount;

            if (PlayerInfo.SelectedLevel == PlayerInfo.MaxLevelReached)
            {
                PlayerInfo.MaxLevelReached++;
            }
        }

        LevelManager.ExitLevel();
    }

    #endregion
}
