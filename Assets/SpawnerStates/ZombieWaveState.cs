using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWaveState : SpawnerState
{
    public int ZombiesToSpawn = 0;
    public SpawnerStateEnum NextState = SpawnerStateEnum.Complete;

    public ZombieWaveState(ZombieSpawner spawner, SpawnerStateMachine stateMachine) : base(spawner ,stateMachine)
    {
        
    }


    public override void Start()
    {
        base.Start();

        for(int i = 0; i < ZombiesToSpawn; i++)
        {
            SpawnZombie();
        }
    }
}
