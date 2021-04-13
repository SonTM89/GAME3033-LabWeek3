using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(SpawnerStateMachine))]
public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] public GameObject[] ZombiePrefab;

    [SerializeField] public SpawnerVolume[] SpawnVolumes;

    public GameObject FollowTarget => FollowGameObject;
    private GameObject FollowGameObject;

    private SpawnerStateMachine StateMachine;

    // Start is called before the first frame update
    void Start()
    {
        StateMachine = GetComponent<SpawnerStateMachine>();

        FollowGameObject = GameObject.FindGameObjectWithTag("Player");

        ZombieWaveState beginnerWave = new ZombieWaveState(this, StateMachine)
        {
            ZombiesToSpawn = 10,
            NextState = SpawnerStateEnum.Complete
        };

        StateMachine.AddState(SpawnerStateEnum.Beginner, beginnerWave);

        StateMachine.Initialize(SpawnerStateEnum.Beginner);
    }
}
