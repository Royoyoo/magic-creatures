using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatsWidget : UiWidget
{
    #region Fields

    [SerializeField] private List<StatInfoWidget> statInfoWidget;

    #endregion



    #region Methods

    public override void Initialize()
    {
        for (int i = 0; i < statInfoWidget.Count; i++)
        {
            statInfoWidget[i].Initialize();
            statInfoWidget[i].OnStatUpgraded += UpdateStats;
        }
    }


    public override void OnShow()
    {
        UpdateStats();
    }


    private void UpdateStats()
    {
        for (int i = 0; i < statInfoWidget.Count; i++)
        {
            statInfoWidget[i].UpdateStat();
        }
    }

    #endregion
}
