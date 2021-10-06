using System;
using TMPro;
using UnityEngine;

public class Creature : MonoBehaviour
{
    #region Fields

    private Action<Creature> removeCreatureCallback;
    private Action hideAllMiniGameCallback;

    [SerializeField] private float maxLike = 100.0f;
    [SerializeField] private Transform likeBarTransform;
    [SerializeField] private TextMeshPro creatureName;

    [SerializeField] private MiniGame miniGame;


    private float currentLike = 0.0f;
    private string creatureId;

    #endregion



    #region Properties

    public string CreatureId => creatureId;

    #endregion



    #region Methods

    public void Initialize(string creatureId, Action<Creature> removeCreatureCallback, Action hideAllMiniGameCallback)
    {
        this.creatureId = creatureId;
        this.removeCreatureCallback = removeCreatureCallback;
        this.hideAllMiniGameCallback = hideAllMiniGameCallback;

        creatureName.text = creatureId;

        miniGame.Initialize();

        HideMiniGame();
        UpdateLikeBar();
    }


    public void Click()
    {
        if (!miniGame.gameObject.activeSelf)
        {
            ShowMiniGame();

            return;
        }

        Interact();
    }


    public void HideMiniGame()
    {
        miniGame.gameObject.SetActive(false);
    }


    private void ShowMiniGame()
    {
        hideAllMiniGameCallback?.Invoke();
        miniGame.gameObject.SetActive(true);
    }


    private void Interact()
    {
        float multiplier = miniGame.EvaluateInteraction();

        currentLike += 10.0f * multiplier;
        currentLike = Mathf.Clamp(currentLike, 0.0f, maxLike);

        UpdateLikeBar();

        if (currentLike >= maxLike)
        {
            Remove();
        }
    }


    private void UpdateLikeBar()
    {
        likeBarTransform.localScale = new Vector3(currentLike / maxLike, 1.0f, 1.0f);
    }


    private void Remove()
    {
        removeCreatureCallback?.Invoke(this);

        Destroy(this.gameObject);
    }

    #endregion
}
