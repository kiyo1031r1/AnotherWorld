using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="DataBase/Enemy/Enemy_DataBase")]
public class Enemy_DataBase : ScriptableObject
{
    [SerializeField] private List<Enemy> enemyList = new List<Enemy>();
    public Enemy[] EnemyList
    {
        get
        {
            return enemyList.ToArray();
        }
    }
}
