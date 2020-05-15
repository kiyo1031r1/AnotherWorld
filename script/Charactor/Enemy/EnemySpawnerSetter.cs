using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawnerSetter : MonoBehaviour
{
    private GuildRequest guildRequest;
    void Start()
    {
        guildRequest = GuildRequestManager.Instance.GuildRequest;
        var enemySpawnList = guildRequest.EnemySpawnData.EnemySpawnList;

        foreach (EnemySpawn enemySpawn in enemySpawnList)
        {
            var enemySpawner = gameObject.AddComponent<EnemySpawner>();
            enemySpawner.EnemySpawn = enemySpawn;
        }
     }
}
