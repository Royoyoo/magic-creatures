using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CommonLevel : MonoBehaviour
{
    #region Fields

    [SerializeField] private int appearedCreaturesCount = 3;
    [SerializeField] private float levelTime = 30.0f;

    [SerializeField] private HeroCharacter heroCharacter;
    [SerializeField] private BossCharacter bossCharacter;
    [SerializeField] private Creature creaturePrefab;
    [SerializeField] private List<Transform> spawnPoints;


    private List<Creature> activeCreatures = new List<Creature>();
    private int creaturesToWin;
    private int creaturesCaught = 0;
    private float levelStartTime;

    #endregion



    #region Properties

    public int CreaturesCaught => creaturesCaught;

    public int CreaturesToWin => creaturesToWin;

    public float TimeLeft => levelTime - (Time.time - levelStartTime);

    #endregion



    #region Methods

    public void Initialize()
    {
        levelStartTime = Time.time;
        creaturesToWin = PlayerInfo.SelectedLevel * 2;

        heroCharacter.BreakInteractionCallback = HideAllCreatureMiniGame;
        bossCharacter?.Initialize(activeCreatures);

        SpawnCreatures();

        DOVirtual
            .DelayedCall(levelTime, LevelManager.LoseLevel)
            .SetId(this);
    }


    public void Deinitialize()
    {
        DOTween.Kill(this);

        heroCharacter.Deinitialize();
        bossCharacter?.Deinitialize();
    }


    public void HideAllCreatureMiniGame()
    {
        for (int i = 0; i < activeCreatures.Count; i++)
        {
            activeCreatures[i].HideMiniGame();
        }
    }


    public void ClickCreature(Creature creature)
    {
        heroCharacter.SetDestination(creature);
    }


    private void RemoveCreature(Creature targetCreature, bool removedByHero)
    {
        if (removedByHero)
        {
            int cardsAmount = Random.Range(1, 4);
            PlayerInfo.AddCreatureCards(targetCreature.CreatureId, cardsAmount);

            creaturesCaught++;

            if (creaturesCaught >= creaturesToWin)
            {
                LevelManager.WinLevel();
            }
        }

        activeCreatures.Remove(targetCreature);

        SpawnNewCreature();
    }


    private void SpawnNewCreature()
    {
        List<Transform> availablePoints = spawnPoints.Where(point => point.childCount == 0).ToList();
        Transform randomPoint = availablePoints[Random.Range(0, availablePoints.Count)];

        string creatureId = PlayerInfo.AllAvailableCreatures[Random.Range(0, PlayerInfo.AllAvailableCreatures.Count)];

        Creature newCreature = Instantiate(creaturePrefab, randomPoint);
        newCreature.Initialize(creatureId);
        newCreature.RemoveCreatureCallback = RemoveCreature;
        newCreature.HideAllMiniGameCallback = HideAllCreatureMiniGame;
        newCreature.CreatureClickedCallback = ClickCreature;

        activeCreatures.Add(newCreature);
    }


    private void SpawnCreatures()
    {
        for (int i = 0; i < appearedCreaturesCount; i++)
        {
            SpawnNewCreature();
        }
    }

    #endregion
}
