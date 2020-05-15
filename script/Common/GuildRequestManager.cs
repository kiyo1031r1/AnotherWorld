using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GuildRequestManager : MonoBehaviour
{
    private static GuildRequestManager _instance;
    public static GuildRequestManager Instance => _instance;
    [SerializeField] GuildRequestEvent_DataBase guildRequestEventData;
    private GuildRequest guildRequest;

    private bool isRequest;
    private string firstEventName;
    private int currentFirstEventClearNumber;
    private string secondEventName;
    private int currentSecondEventClearNumber;
    private bool firstEventClearFlag;
    private bool secondEventClearFlag;

    public bool IsRequest => isRequest;
    public GuildRequest GuildRequest => guildRequest;
    public string FirstEventName => firstEventName;
    public int CurrentFirstEventClearNumber => currentFirstEventClearNumber;
    public string SecondEventName => secondEventName;
    public int CurrentSecondEventClearNumber => currentSecondEventClearNumber;
    public bool IsSecondEvent => guildRequest.SecondTargetEvent != null;
    public bool FirstEventClearFlag => firstEventClearFlag;
    public bool SecondEventClearFlag => secondEventClearFlag;


    private GuildRequestManager()
    {

    }

    void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetRequestID(GuildRequest request)
    {
        ResetRequestID();
        guildRequest = request;

        //第一イベント
        var firstEvent = guildRequestEventData.GuildRequestEventList.FirstOrDefault(x => x== guildRequest.FirstTargetEvent);
        firstEventName = firstEvent.Information;

        //第二イベント
        if(guildRequest.SecondTargetEvent != null)
        {
            var secondEvent = guildRequestEventData.GuildRequestEventList.FirstOrDefault(x => x == guildRequest.SecondTargetEvent);
            secondEventName = secondEvent.Information;
        }

        //第二イベント未設定
        else
        {
            secondEventClearFlag = true;
        }

        isRequest = true;
    }


    public void ResetRequestID()
    {
        guildRequest = null;
        isRequest = false;
        firstEventName = "";
        secondEventName = "";
        currentFirstEventClearNumber = 0;
        currentSecondEventClearNumber = 0;
        firstEventClearFlag = false;
        secondEventClearFlag = false;
    }

    public void FailedRequest()
    {
        ResetRequestID();
        Message.Instance.FailedGuildRequest();
    }

    public void CountUpEvent(GuildRequestEvent guildRequestEvent)
    {
        //第一イベント処理
        if(guildRequestEvent == guildRequest.FirstTargetEvent)
        {
            currentFirstEventClearNumber++;
            if (currentFirstEventClearNumber  < guildRequest.FirstEventClearNumber) return;
            
            firstEventClearFlag = true;

            if(firstEventClearFlag && secondEventClearFlag)
            {
                Message.Instance.ClearGuildRequest();
            }

        }
        //第二イベント処理
        else if (guildRequestEvent == guildRequest.SecondTargetEvent)
        {
            currentSecondEventClearNumber++;
            if (currentSecondEventClearNumber < guildRequest.SecondEventClearNumber) return;

            secondEventClearFlag = true;

            if (firstEventClearFlag && secondEventClearFlag)
            {
                Message.Instance.ClearGuildRequest();
            }
        }
    }

}
