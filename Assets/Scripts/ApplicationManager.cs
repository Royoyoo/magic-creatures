using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private UiManager uiManager;
    [SerializeField] private PlayerInfo playerInfo;

    #endregion



    #region Unity lifecycle

    private void Start()
    {
        playerInfo.Initialize();
        uiManager.Initialize();
    }

    #endregion
}
