using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/Enemy/Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField] private int enemyID;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private string enemyName;
    [SerializeField] private float maxHP;
    [SerializeField] private float attack;
    [SerializeField] private float defence;
    [SerializeField] private float speed;
    [SerializeField] private float viewEuler;
    [SerializeField] private float exp;
    [SerializeField] private GuildRequestEvent guildRequestEvent;
    [SerializeField] private Item dropItem1;
    [SerializeField] private int dropItem1_Number;
    [SerializeField, Range(0, 1f)] private float dropItem1_Probability;
    [SerializeField] private Item dropItem2;
    [SerializeField] private int dropItem2_Number;
    [SerializeField, Range(0, 1f)] private float dropItem2_Probability;
    [SerializeField] private AudioClip attackSE;
    [SerializeField] private AudioClip dieSE;

    public int EnemyID => enemyID;
    public GameObject EnemyPrefab => enemyPrefab;
    public string EnemyName => enemyName;
    public float MaxHP => maxHP;
    public float Attack => attack;
    public float Defence => defence;
    public float Speed => speed;
    public float ViewEuler => viewEuler;
    public float Exp => exp;
    public GuildRequestEvent GuildRequestEvent => guildRequestEvent;
    public Item DropItem1 => dropItem1;
    public int DropItem1_Number => dropItem1_Number;
    public float DropItem1_Probability => dropItem1_Probability;
    public Item DropItem2 => dropItem2;
    public int DropItem2_Number => dropItem2_Number;
    public float DropItem2_Probability => dropItem2_Probability;
    public AudioClip AttackSE => attackSE;
    public AudioClip DieSE => dieSE;
}
