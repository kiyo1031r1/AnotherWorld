using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/GuildRequest/Event_DataBase")]
public class GuildRequestEvent_DataBase : ScriptableObject
{
    [SerializeField] private List<GuildRequestEvent> guildRequestEventList = new List<GuildRequestEvent>();
    public GuildRequestEvent[] GuildRequestEventList
    {
        get
        {
            return guildRequestEventList.ToArray();
        }
    }
}
