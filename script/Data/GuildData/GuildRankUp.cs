using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="DataBase/GuildRequest/GuildRankUp")]
public class GuildRankUp : ScriptableObject
{
    [SerializeField] private GuildRequest.RankEnum rank;
    [SerializeField] private GuildRequest.RankEnum nextRank;
    [SerializeField] private float clearIronNumber;
    [SerializeField] private float clearBronzeNumber;
    [SerializeField] private float clearCopperNumber;

    public GuildRequest.RankEnum Rank => rank;
    public GuildRequest.RankEnum NextRank => nextRank;
    public float ClearIronNumber => clearIronNumber;
    public float ClearBronzeNumber => clearBronzeNumber;
    public float ClearCopperNumber => clearCopperNumber;
}
