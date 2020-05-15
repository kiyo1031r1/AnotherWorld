using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/Item/All")]
public class Item : ScriptableObject
{
    public enum ItemCategoryEnum
    {
        Parameter,
        Guild,
    }

    [SerializeField] private int itemID;
    [SerializeField] private string itemName;
    [SerializeField, TextArea] private string information;
    [SerializeField] private int buyPrice;
    [SerializeField] private int sellPrice;
    [SerializeField] ItemCategoryEnum Category;
    [SerializeField] private Item_Parameter parameterItem;
    [SerializeField] private GuildRequestEvent guildRequestEvent;


    public int ItemID => itemID;
    public string ItemName => itemName;
    public string Information => information;
    public int BuyPrice => buyPrice;
    public int SellPrice => sellPrice;
    public ItemCategoryEnum ItemCategory => Category;
    public Item_Parameter ParameteItem => parameterItem;
    public GuildRequestEvent GuildRequestEvent => guildRequestEvent;

}
