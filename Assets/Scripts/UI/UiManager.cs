using System;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    #region Nested Types

    [Serializable]
    public class UiElement
    {
        public UiWidgetType Type;
        public UiWidget uiWidget;
    }

    #endregion



    #region Fields

    [SerializeField] private Camera uiCamera;
    [SerializeField] private List<UiElement> uiElements;
    [SerializeField] private List<UiWidgetType> uiShownByDefault;


    private static UiManager Instance;

    #endregion



    #region Methods

    public void Initialize()
    {
        Instance = this;

        HideAll();

        //TODO Validate uiElements list & uiShownByDefault list

        uiShownByDefault.ForEach(Show);

        uiElements.ForEach(uiElement =>
        {
            Canvas canvas = uiElement.uiWidget.GetComponent<Canvas>();
            canvas.worldCamera = uiCamera;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;

            uiElement.uiWidget.Initialize();
        });
    }


    public static void Show(UiWidgetType uiType)
    {
        SetActive(uiType, true);
    }


    public static void Hide(UiWidgetType uiType)
    {
        SetActive(uiType, false);
    }


    public static bool IsUiActive(UiWidgetType uiType)
    {
        UiElement uiElement = Instance.uiElements.Find(element => element.Type == uiType);


        if (uiElement != null)
        {
            return uiElement.uiWidget.gameObject.activeSelf;
        }

        //TODO throw custom Exception
        throw new NullReferenceException($"uiElement with id {uiType.ToString()} not exist");
    }


    public static void HideAll(List<UiWidgetType> stayActiveUis = null)
    {
        foreach (UiElement uiElement in Instance.uiElements)
        {
            if (stayActiveUis != null && stayActiveUis.Contains(uiElement.Type))
            {
                continue;
            }

            Hide(uiElement.Type);
        }
    }


    private static void SetActive(UiWidgetType uiType, bool isActive)
    {
        UiElement uiElement = Instance.uiElements.Find(element => element.Type == uiType);

        if (uiElement != null)
        {
            uiElement.uiWidget.gameObject.SetActive(isActive);
            uiElement.uiWidget.OnShow();
        }
        else
        {
            //TODO throw custom Exception
        }
    }

    #endregion
}
