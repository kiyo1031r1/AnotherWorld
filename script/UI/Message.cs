using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Message : MonoBehaviour
{
    public static Message Instance => _instance;
    private static Message _instance;

    [SerializeField] private CanvasGroup messageCanvas;

    //guildMessage
    [SerializeField] private GameObject guildMessage;
    private CanvasGroup guildMessageCanvas;
    [SerializeField] private Text guildMessageText;
    private Transform guildMessageTransform;
    [SerializeField] private float guildMessageWaitTime;

    //itemMessage
    [SerializeField] private GameObject itemMessage;
    private CanvasGroup itemMessageCanvas;
    [SerializeField] private Text itemMessageText;
    private Transform itemMessageTransform;
    [SerializeField] private float itemMessageWaitTime;

    //levelUpMessage
    [SerializeField] private GameObject levelUpPrefab; 

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

    void Start()
    {
        guildMessageTransform = guildMessageText.GetComponent<Transform>();
        guildMessageCanvas = guildMessage.GetComponent<CanvasGroup>();
        itemMessageTransform = itemMessageText.GetComponent<Transform>();
        itemMessageCanvas = itemMessage.GetComponent<CanvasGroup>();

        messageCanvas.alpha = 0;
        messageCanvas.interactable = false;
        guildMessageCanvas.alpha = 0;
        itemMessageCanvas.alpha = 0;
    }

    public void ClearGuildRequest()
    {
        SoundManager.Instance.PlaySE(3);
        var playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();

        messageCanvas.alpha = 1;
        guildMessageCanvas.alpha = 1;
        DisplayGuildMessage("ギルド依頼を達成しました");
        DOVirtual.DelayedCall(guildMessageWaitTime, () =>
        {
            DisplayGuildMessage("拠点へ帰ります");
            DOVirtual.DelayedCall(guildMessageWaitTime, () =>
            {
                Fade.Instance.FadeOutFadeIn("GuildResultScene");
                messageCanvas.alpha = 0;
                guildMessageCanvas.alpha = 0;
            });
        });
    }

    public void FailedGuildRequest()
    {
        SoundManager.Instance.PlaySE(4);
        var playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();

        messageCanvas.alpha = 1;
        guildMessageCanvas.alpha = 1;
        DisplayGuildMessage("ギルド依頼を失敗しました");
        DOVirtual.DelayedCall(guildMessageWaitTime, () =>
        {
            DisplayGuildMessage("拠点へ帰ります");
            DOVirtual.DelayedCall(guildMessageWaitTime, () =>
            {
                Fade.Instance.FadeOutFadeIn("TownScene");
                GuildRequestManager.Instance.ResetRequestID();
                messageCanvas.alpha = 0;
                guildMessageCanvas.alpha = 0;
            });
        });
    }

    public void GetItemMessage(string name, int number)
    {
        messageCanvas.alpha = 1;
        itemMessageCanvas.alpha = 1;
        DisplayItemMessage(name + "を" + number + "個手に入れました");
        DOVirtual.DelayedCall(itemMessageWaitTime, () =>
        {
            messageCanvas.alpha = 0;
            itemMessageCanvas.alpha = 0;
        });
    }

    private void DisplayGuildMessage(string message)
    {
        SoundManager.Instance.PlaySE(13);
        guildMessageText.text = message;
        var transformCache = guildMessageTransform;
        var defaultPosition = transformCache.localPosition;
        transformCache.localPosition = defaultPosition + new Vector3(0, 20f);
        transformCache.DOLocalMove(defaultPosition, 0.5f);
    }

    private void DisplayItemMessage(string message)
    {
        SoundManager.Instance.PlaySE(13);
        itemMessageText.text = message;
        var transformCache = itemMessageTransform;
        var defaultPosition = transformCache.localPosition;
        transformCache.localPosition = defaultPosition + new Vector3(0, 5f);
        transformCache.DOLocalMove(defaultPosition, 0.5f);
    }

    public void LevelUpMessaga()
    {
        SoundManager.Instance.PlaySE(8);
        Instantiate(levelUpPrefab);
    }
}
