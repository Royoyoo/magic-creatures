using UnityEngine;
using UnityEngine.UI;

public class ExitLevelWidget : UiWidget
{
    #region Fields

    [SerializeField] private Button exitButton;

    #endregion



    #region Methods

    public override void Initialize()
    {
        exitButton.onClick.AddListener(LevelManager.ExitLevel);
    }

    #endregion
}
