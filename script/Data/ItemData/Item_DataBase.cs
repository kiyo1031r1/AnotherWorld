using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/Item/All_DataBase")]
public class Item_DataBase : ScriptableObject
{
    [SerializeField] private List<Item> itemList = new List<Item>();
    public Item[] ItemList
    {
        get
        {
            return itemList.ToArray();
        }
    }
}
