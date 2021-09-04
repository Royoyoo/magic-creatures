using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private UiManager uiManager;
    [SerializeField] private LevelResultWidget levelResultWidget;
    [SerializeField] private CommonLevel commonLevel;


    private static LevelManager Instance;

    #endregion



    #region Unity lifecycle

    private void Start()
    {
        Instance = this;

        uiManager.Initialize();
        commonLevel.Initialize();
    }

    #endregion



    #region Methods

    public static void WinLevel()
    {
        FinishLevel(true);
    }


    public static void LoseLevel()
    {
        FinishLevel(false);
    }


    public static void ExitLevel()
    {
        Instance.commonLevel.Deinitialize();

        SceneManager.LoadScene("Main");
    }


    private static void FinishLevel(bool isWon)
    {
        UiManager.Hide(UiWidgetType.InGame);
        UiManager.Show(UiWidgetType.LevelResult);

        Instance.commonLevel.Deinitialize();

        Instance.levelResultWidget.ShowResult(isWon);
    }

    #endregion
}
