using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public EnemySpawn EnemySpawn;
    private List<GameObject> enemyList = new List<GameObject>();

    //フィールドサイズ
    private float fieldX = 200;
    private float fieldZ = 200;

    void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            //リストを更新
            enemyList = enemyList.Where(x => x != null).ToList();

            if (enemyList.Count < EnemySpawn.MaxSpawn)
            {
                if (Random.Range(0f, 1f) < EnemySpawn.SpawnProbability)
                {
                    var spawnPosition = new Vector3(Random.Range(-fieldX, fieldX), 0f, Random.Range(-fieldZ, fieldZ));
                    if (NavMesh.SamplePosition(spawnPosition, out var navMeshHit, 10f, NavMesh.AllAreas))
                    {
                        var spawnEnemy = Instantiate(EnemySpawn.Enemy.EnemyPrefab, navMeshHit.position, Quaternion.identity);
                        spawnEnemy.GetComponent<EnemyStatus>().EnemyData = EnemySpawn.Enemy;
                        enemyList.Add(spawnEnemy);
                    }
                }
            }

            yield return new WaitForSeconds(EnemySpawn.SpawnWait);
        }
    }
}
