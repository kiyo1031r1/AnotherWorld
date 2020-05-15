using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="DataBase/PlayerStatusData/PlayerStatus")]

public class PlayerStatusData : ScriptableObject
{
    public int number;
    public int level;
    public float hitPoint;
    public float attack;
    public float defence;
    public float needExp;
}
