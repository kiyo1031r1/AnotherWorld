using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataLoadManager : MonoBehaviour
{
    [SerializeField] private PlayerStatusData_DataBase playerStatusDataBase;
    [SerializeField] private GuildRankUp_DataBase guildRankUpDataBase;
    [SerializeField] private Item_DataBase itemDataBase;

    void Start()
    {
        LoadStatusData();
        LoadItemData();
    }

    public void LoadStatusData()
    {
        //ステータスを取得
        PlayData_Status.Instance.playerStatusDataBase = playerStatusDataBase;

        //ギルドランクUPデータを取得
        PlayData_Status.Instance.guildRankUpDataBase = guildRankUpDataBase;
    }

    public void LoadItemData()
    {
        //アイテムデータを取得
        for(int i = 0; PlayData_OwnedItems.Instance.OwnedItemList.Length > i; i++)
        {
            var item = itemDataBase.ItemList.FirstOrDefault(x => x.ItemID == PlayData_OwnedItems.Instance.OwnedItemList[i].ItemID);
            PlayData_OwnedItems.Instance.OwnedItemList[i].SetItemData(item);
        }
        PlayData_OwnedItems.Instance.Save();
    }
}
