using TMPro;
using UnityEngine;

public class CoinsCounterWidget : UiWidget
{
    #region Fields

    [SerializeField] private TextMeshProUGUI coinsText;

    #endregion



    #region Unity lifecycle

    private void Update()
    {
        UpdateCount();
    }

    #endregion



    #region Methods

    public override void Initialize()
    {
        UpdateCount();
    }


    private void UpdateCount()
    {
        coinsText.text = $"{PlayerInfo.Coins}$";
    }

    #endregion
}
