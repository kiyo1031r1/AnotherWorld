using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/GuildRequest/EnemySpawn_DataBase")]
public class EnemySpawn_DataBase : ScriptableObject
{
    [SerializeField] private List<EnemySpawn> enemySpawnList = new List<EnemySpawn>();
    public EnemySpawn[] EnemySpawnList
    {
        get
        {
            return enemySpawnList.ToArray();
        }
    }
}
