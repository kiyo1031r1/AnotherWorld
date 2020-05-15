using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropItem : MonoBehaviour
{
    private Item item;
    private int dropNumber;

    public void GetItem()
    {
        PlayData_OwnedItems.Instance.Add(item, dropNumber);
        PlayData_OwnedItems.Instance.Save();
        Message.Instance.GetItemMessage(item.ItemName, dropNumber);
        Destroy(gameObject);
    }

    public void SetItem(Item item, int number)
    {
        this.item = item;
        dropNumber = number;
    }
}
