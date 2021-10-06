using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossCharacter : MonoBehaviour
{
    #region Fields

    [SerializeField] private float moveSpeed;
    [SerializeField] private float interactDistance;
    [SerializeField] private float interactDuration;


    private IReadOnlyList<Creature> availableCreatures;

    private Vector3 destination;
    private Creature targetCreature;
    private bool isMoving = false;

    #endregion



    #region Methods

    public void Initialize(IReadOnlyList<Creature> availableCreatures)
    {
        this.availableCreatures = availableCreatures;
    }


    public void Deinitialize()
    {
        DOTween.Kill(this);
    }


    public void Update()
    {
        if (targetCreature == null)
        {
            targetCreature = availableCreatures[Random.Range(0, availableCreatures.Count)];
            SetDestination(targetCreature);
        }

        if (isMoving)
        {
            CheckTarget();
        }
    }


    public void SetDestination(Creature creature)
    {
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


    private void Interact()
    {
        DOVirtual
            .DelayedCall(interactDuration, () =>
            {
                if (targetCreature != null) targetCreature.RemoveByBoss();
            })
            .SetId(this);
    }


    private void CheckTarget()
    {
        if (targetCreature == null || Vector3.Distance(transform.position, destination) > interactDistance)
        {
            return;
        }

        DOTween.Kill(this);

        Interact();
        isMoving = false;
    }

    #endregion
}
