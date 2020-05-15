using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class PlayData_OwnedItems
{
    const string playerPrefsKey = "OWNED_ITEM";
    private static PlayData_OwnedItems instance;
    public static PlayData_OwnedItems Instance
    {
        get
        {
            if (instance == null)
            {
                instance = PlayerPrefs.HasKey(playerPrefsKey)
                    ? JsonUtility.FromJson<PlayData_OwnedItems>(PlayerPrefs.GetString(playerPrefsKey))
                    : new PlayData_OwnedItems();
            }
            return instance;
        }
    }

    private PlayData_OwnedItems()
    {

    }

    public OwnedItem[] OwnedItemList
    {
        get
        {
            return ownedItemList.ToArray();
        }
    } 

    [SerializeField] private List<OwnedItem> ownedItemList = new List<OwnedItem>();

    public void LoadFirstItem()
    {
        ownedItemList = new List<OwnedItem>();
    }

    public void Add(Item item, int number = 1)
    {
        var targetItem = SearchItem(item);
        if (targetItem == null)
        {
            targetItem = new OwnedItem(item);
            ownedItemList.Add(targetItem);
        }
        targetItem.Add(number);
    }

    public void Use(Item item, int number = 1)
    {
        var targetItem = SearchItem(item);
        if (targetItem == null)
        {
            throw new Exception("アイテムを所持していません");
        }
        if (targetItem.Number < number)
        {
            throw new Exception("アイテムが足りません");
        }

        targetItem.Use(number);
        if (targetItem.Number == 0)
        {
            ownedItemList.Remove(targetItem);
        }
    }

    public OwnedItem SearchItem(Item item)
    {
        return ownedItemList.FirstOrDefault((x => x.ItemData == item));
    }

    public void Save()
    {
        var saveData = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(playerPrefsKey, saveData);
        PlayerPrefs.Save();
    }

    [Serializable]
    public class OwnedItem
    {
        [SerializeField] private Item itemData;
        [SerializeField] private int itemID;
        [SerializeField] private int number;

        public Item ItemData => itemData;
        public int ItemID => itemID;
        public int Number => number;

        public OwnedItem(Item item)
        {
            itemData = item;
            itemID = item.ItemID;
        }

        public void SetItemData(Item item)
        {
            itemData = item;
        }

        public void Add(int number = 1)
        {
            this.number += number;
        }

        public void Use(int number = 1)
        {
            this.number -= number;
        }
    }

}
