using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/GuildRequest/Event")]
public class GuildRequestEvent : ScriptableObject
{
    [SerializeField] private int eventID;
    [SerializeField, TextArea] private string information;

    public int EventID => eventID;
    public string Information => information;
    
}
