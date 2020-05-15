using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/GuildRequest/EnemySpawn")]
public class EnemySpawn : ScriptableObject
{
    [SerializeField] Enemy enemy;
    [SerializeField] private int maxSpawn;
    [SerializeField] private float spawnWait;
    [SerializeField] [Range(0, 1)] private float spawnProbability = 1;

    public Enemy Enemy => enemy;
    public int MaxSpawn => maxSpawn;
    public float SpawnWait => spawnWait;
    public float SpawnProbability => spawnProbability;
}
