using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Item : MonoBehaviour
{
    private Item itemData;
    [SerializeField] private Text nameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text haveText;

    public Item ItemData => itemData;

    public void SetItem(Item item)
    {
        itemData = item;
        nameText.text = itemData.ItemName;

        var ownedItem = PlayData_OwnedItems.Instance.SearchItem(itemData);

        //アイテム未所持
        if (ownedItem == null)
        {
            haveText.text = "0";
        }
        else
        {
            haveText.text = ownedItem.Number.ToString();
        }
    }

    public void SetBuyPrice()
    {
        priceText.text = itemData.BuyPrice.ToString();
    }

    public void SetSellPrice()
    {
        priceText.text = itemData.SellPrice.ToString();
    }
}
