using System;
using DG.Tweening;
using UnityEngine;

public class HeroCharacter : MonoBehaviour
{
    #region Fields

    public Action BreakInteractionCallback;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float interactDistance;


    private Vector3 destination;
    private Creature targetCreature;
    private bool isMoving = false;

    #endregion



    #region Methods

    public void Deinitialize()
    {
        DOTween.Kill(this);
    }


    public void Update()
    {
        if (isMoving)
        {
            CheckTarget();
        }
    }


    public void SetPointerPointAsDestination()
    {
        BreakInteractionCallback?.Invoke();

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0.0f;

        targetCreature = null;

        SetDestination(position);
    }


    public void SetDestination(Creature creature)
    {
        if (targetCreature != creature)
        {
            BreakInteractionCallback?.Invoke();
        }

        if (Vector3.Distance(transform.position, creature.transform.position) > interactDistance)
        {
            SetDestination(creature.transform.position);
        }
        else
        {
            creature.Click();
        }


        targetCreature = creature;
    }


    private void SetDestination(Vector3 position)
    {
        destination = position;
        DOTween.Kill(this);

        float duration = Vector3.Distance(transform.position, destination) / moveSpeed;

        transform
            .DOMove(destination, duration)
            .SetId(this);

        isMoving = true;
    }


    private void CheckTarget()
    {
        if (targetCreature == null || Vector3.Distance(transform.position, destination) > interactDistance)
        {
            return;
        }

        DOTween.Kill(this);

        targetCreature.Click();
        isMoving = false;
    }

    #endregion
}
