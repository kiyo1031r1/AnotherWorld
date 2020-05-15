using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDialog : MonoBehaviour
{
    [SerializeField] private Button button_StatusReset;
    [SerializeField] private Button button_ItemReset;
    [SerializeField] private Button button_ItemGet;
    [SerializeField] private Item getItem;
    [SerializeField] private int getItemNumber;
    [SerializeField] private Button button_MoneyReset;
    [SerializeField] private Button button_MoneyMax;
    [SerializeField] private Button button_MoneyZero;
    [SerializeField] private Button button_GuildClear;

    const string playerPrefs_StatusKey = "STATUS";
    const string playerPrefs_OnwedItemKey = "OWNED_ITEM";
    const string playerPrefs_OwnedMoneyKey = "OWNED_MONEY";
    const int MAX_OWNED_MONEY = 9999;
    void Start()
    {
        button_StatusReset.onClick.AddListener(StatusReset);
        button_ItemReset.onClick.AddListener(ItemReset);
        button_ItemGet.onClick.AddListener(ItemGet);
        button_MoneyReset.onClick.AddListener(MoneyReset);
        button_MoneyMax.onClick.AddListener(MoneyMax);
        button_MoneyZero.onClick.AddListener(MoneyZero);
        button_GuildClear.onClick.AddListener(GuildClear);
    }

    public  void StatusReset()
    {
        PlayerPrefs.DeleteKey(playerPrefs_StatusKey);
        PlayerPrefs.Save();
        PlayData_Status.Instance.LoadFirstStatus();
        PlayData_Status.Instance.Save();
    }
    public void ItemReset()
    {
        PlayerPrefs.DeleteKey(playerPrefs_OnwedItemKey);
        PlayerPrefs.Save();
    }

    public void ItemGet()
    {
        PlayData_OwnedItems.Instance.Add(getItem, getItemNumber);
        PlayData_OwnedItems.Instance.Save();
    }

    public void MoneyReset()
    {
        PlayerPrefs.DeleteKey(playerPrefs_OwnedMoneyKey);
        PlayerPrefs.Save();
    }

    public void MoneyMax()
    {
        PlayData_OwnedMoney.Instance.Add(MAX_OWNED_MONEY);
        PlayData_OwnedMoney.Instance.Save();
    }

    public void MoneyZero()
    {
        PlayData_OwnedMoney.Instance.Use();
        PlayData_OwnedMoney.Instance.Save();
    }

    public void GuildClear()
    {
        PlayData_Status.Instance.AddClearGuildRequest(GuildRequest.RankEnum.Iron);
        PlayData_Status.Instance.AddClearGuildRequest(GuildRequest.RankEnum.Iron);
        PlayData_Status.Instance.AddClearGuildRequest(GuildRequest.RankEnum.Iron);
        PlayData_Status.Instance.AddClearGuildRequest(GuildRequest.RankEnum.Iron);
        PlayData_Status.Instance.AddClearGuildRequest(GuildRequest.RankEnum.Iron);
        PlayData_Status.Instance.OffRankUpFlug();
        PlayData_Status.Instance.Save();
    }

}
