using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/GuildRequest/Request")]
public class GuildRequest : ScriptableObject
{
    [SerializeField] private int requestID;
    [SerializeField] private string requestName;
    public enum RequestTypeEnum
    {
        Hunting,
        Item,
    }
    [SerializeField] private RequestTypeEnum requestType;
    [SerializeField] private int reward;
    public enum RankEnum
    {
        Iron,
        Bronze,
        Copper,
    }
    [SerializeField] private GuildRequest.RankEnum rank;
    [SerializeField, TextArea] private string information;
    [SerializeField] private string location;
    [SerializeField] private GuildRequestEvent firstTargetEvent;
    [SerializeField] private int firstEventClearNumber;
    [SerializeField] private GuildRequestEvent secondTargetEvent;
    [SerializeField] private int secondEventClearNumber;
    [SerializeField] private EnemySpawn_DataBase enemySpawnData; 

    public int RequestID => requestID;
    public string RequestName => requestName;
    public RequestTypeEnum RequestType => requestType;
    public int Reward => reward;
    public GuildRequest.RankEnum Rank => rank;
    public string RankText
    {
        get
        {
            string rankText = "";
            switch (rank)
            {
                case RankEnum.Iron:
                    rankText = "アイアン級";
                    break;
                case RankEnum.Bronze:
                    rankText = "ブロンズ級";
                    break;
                case RankEnum.Copper:
                    rankText = "カッパー級";
                    break;
            }
            return rankText;
        }
    }
    public string Information => information;
    public string Location => location;
    public GuildRequestEvent FirstTargetEvent => firstTargetEvent;
    public int FirstEventClearNumber => firstEventClearNumber;
    public GuildRequestEvent SecondTargetEvent =>secondTargetEvent;
    public int SecondEventClearNumber => secondEventClearNumber;
    public EnemySpawn_DataBase EnemySpawnData => enemySpawnData;

}
