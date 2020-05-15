using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/GuildRequest/GuildRankUp_DataBase")]

public class GuildRankUp_DataBase : ScriptableObject
{
    [SerializeField] private List<GuildRankUp> guildRankUpList = new List<GuildRankUp>();
    public GuildRankUp[] GuildRankUpList
    {
        get
        {
            return guildRankUpList.ToArray();
        }
    }
  
}
