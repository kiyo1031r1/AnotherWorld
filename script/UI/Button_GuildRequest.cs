using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_GuildRequest : MonoBehaviour
{
    private GuildRequest guildRequest;
    [SerializeField] private Text requestName;
    [SerializeField] private Text reward;
    [SerializeField] private Text rankText;

    public GuildRequest GuildRequest => guildRequest;
    public Text RequestName => requestName;
    public Text Reward => reward;
    public Text RankText => rankText;

    public void SetRequest(GuildRequest request)
    {
        guildRequest = request;
        requestName.text = guildRequest.RequestName;
        reward.text = guildRequest.Reward + "円";
        rankText.text = guildRequest.RankText;
    }
}
