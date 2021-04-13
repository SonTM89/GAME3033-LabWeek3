using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerState : State<SpawnerStateEnum>
{
    protected ZombieSpawner Spawner;

    protected SpawnerState(ZombieSpawner spawner, SpawnerStateMachine stateMachine) : base(stateMachine)
    {
        Spawner = spawner;
    }

    protected void SpawnZombie()
    {
        GameObject zombieToSpawn = Spawner.ZombiePrefab[Random.Range(0, Spawner.ZombiePrefab.Length)];
        SpawnerVolume spawnVolume = Spawner.SpawnVolumes[Random.Range(0, Spawner.SpawnVolumes.Length)];

        if (!Spawner.FollowTarget)
        {
            return;
        }

        GameObject zombie = Object.Instantiate(zombieToSpawn, spawnVolume.GetPositionInBounds(), spawnVolume.transform.rotation);

        zombie.GetComponent<ZombieComponent>().Initialize(Spawner.FollowTarget);
    }
}
