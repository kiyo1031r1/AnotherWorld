using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/GuildRequest/Request_DataBase")]
public class GuildRequest_DataBase : ScriptableObject
{
    [SerializeField] private List<GuildRequest> guildRequestList = new List<GuildRequest>();
    public GuildRequest[] GuildRequestList
    {
        get
        {
            return guildRequestList.ToArray();
        }
    }
}
