using System.Collections.Generic;
using UnityEngine;

public class CreaturesStatsWidget : UiWidget
{
    #region Fields

    [SerializeField] private CreatureInfoWidget creatureInfoWidgetPrefab;
    [SerializeField] private RectTransform creatureListParent;

    private List<CreatureInfoWidget> creatureInfoWidgets = new List<CreatureInfoWidget>();

    #endregion



    #region Methods

    public override void Initialize()
    {
        for (int i = 0; i < PlayerInfo.AvailableCreatures.Count; i++)
        {
            CreatureInfoWidget creatureInfoWidget = Instantiate(creatureInfoWidgetPrefab, creatureListParent);

            creatureInfoWidget.Initialize();
            creatureInfoWidget.SetCreature(PlayerInfo.AvailableCreatures[i]);
            creatureInfoWidget.OnCreatureUpgraded += UpdateStats;

            creatureInfoWidgets.Add(creatureInfoWidget);
        }
    }


    public override void Deinitialize()
    {
        base.Deinitialize();

        for (int i = 0; i < creatureInfoWidgets.Count; i++)
        {
            Destroy(creatureInfoWidgets[i].gameObject);
        }

        creatureInfoWidgets.Clear();
    }


    private void UpdateStats()
    {
        for (int i = 0; i < creatureInfoWidgets.Count; i++)
        {
            creatureInfoWidgets[i].UpdateInfo();
        }
    }

    #endregion
}
