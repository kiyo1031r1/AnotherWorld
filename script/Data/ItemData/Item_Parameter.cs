using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/Item/Parameter")]
public class Item_Parameter : ScriptableObject
{
    public enum EffectTimeTypeEnum
    {
        Temporary,
        Permanent,
    }
    public enum EffectRatioTypeEnum
    {
        Fixed,
        Percentage,
    }

    [SerializeField] private EffectTimeTypeEnum effectTimeType;
    [SerializeField] private EffectRatioTypeEnum effectRatioType;
    [SerializeField] private int itemID;
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;
    [SerializeField] private float attack;
    [SerializeField] private float speed;
    [SerializeField] private float effectTime;
    public enum EffectTimeTargetEnum
    {
        None,
        Attack,
    }
    [SerializeField] private EffectTimeTargetEnum effectTimeTarget;

    public EffectTimeTypeEnum EffectTimeType => effectTimeType;
    public EffectRatioTypeEnum EffectRatioType => effectRatioType;
    public int ItemID => itemID;
    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public float Attack => attack;
    public float Speed => speed;
    public float EffectTime => effectTime;
    public EffectTimeTargetEnum EffectTimeTarget => effectTimeTarget;

}
