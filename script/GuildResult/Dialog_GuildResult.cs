using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Dialog_GuildResult : MonoBehaviour
{
    [SerializeField] GameObject leftContent;
    [SerializeField] Text requestName;
    [SerializeField] Text reward;
    [SerializeField] Text rank;
    [SerializeField] GameObject money;
    [SerializeField] Text moneyNumber;
    [SerializeField] GameObject clearRank;
    [SerializeField] Text clearIronNumber;
    [SerializeField] Text clearBronzeNumber;
    [SerializeField] Text clearCopperNumber;
    [SerializeField] Text rankUpText;
    [SerializeField] Text endText;
    private float waitTime = 0.5f;
    private bool isEndable;

    void Start()
    {
        SetText();
        leftContent.SetActive(false);
        money.SetActive(false);
        clearRank.SetActive(false);
        rankUpText.text = "";
        endText.text = "";
        DisplayContent();
    }

    void Update()
    {
      if(isEndable)
        {
            //〇ボタン
            if (Input.GetButtonDown("Fire3"))
            {
                SoundManager.Instance.PlaySE(10);
                isEndable = false;
                GuildRequestManager.Instance.ResetRequestID();
                Fade.Instance.FadeOutFadeIn("TownScene");
            }
        }  
    }

    private void SetText()
    {
        PlayData_OwnedMoney.Instance.Add(GuildRequestManager.Instance.GuildRequest.Reward);
        PlayData_OwnedMoney.Instance.Save();
        PlayData_Status.Instance.AddClearGuildRequest(GuildRequestManager.Instance.GuildRequest.Rank);
        PlayData_Status.Instance.Save();

        requestName.text = GuildRequestManager.Instance.GuildRequest.RequestName;
        reward.text = ": " + GuildRequestManager.Instance.GuildRequest.Reward + "円";
        rank.text = ": " + GuildRequestManager.Instance.GuildRequest.RankText;
        moneyNumber.text = ": " + PlayData_OwnedMoney.Instance.OwnedMoney + "円";
        clearIronNumber.text = PlayData_Status.Instance.ClearIronNumber.ToString();
        clearBronzeNumber.text = PlayData_Status.Instance.ClearBronzeNumber.ToString();
        clearCopperNumber.text = PlayData_Status.Instance.ClearCopperNumber.ToString();
    }

    private void DisplayContent()
    {
        DOVirtual.DelayedCall(waitTime, () =>
        {
            SoundManager.Instance.PlaySE(5);
            leftContent.SetActive(true);
            DOVirtual.DelayedCall(waitTime, () =>
            {
                SoundManager.Instance.PlaySE(5);
                money.SetActive(true);
                DOVirtual.DelayedCall(waitTime, () =>
                {
                    SoundManager.Instance.PlaySE(5);
                    clearRank.SetActive(true);
                    DOVirtual.DelayedCall(waitTime, () =>
                    {
                        DisplayRankUpText();
                        DOVirtual.DelayedCall(waitTime, () =>
                        {
                            endText.text = "〇ボタンで終了";
                            isEndable = true;
                        });
                    });
                });
            });
        });
    }

    private void DisplayRankUpText()
    {
        if (PlayData_Status.Instance.IsRankUp)
        {
            SoundManager.Instance.PlaySE(8);
            rankUpText.text = PlayData_Status.Instance.RankText + "に昇格しました！";
            PlayData_Status.Instance.OffRankUpFlug();
        }
    }
}
