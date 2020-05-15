using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/Item/Parameter_DataBase")]
public class Item_Parameter_DataBase : ScriptableObject
{
    [SerializeField] private List<Item_Parameter> item_ParameterList = new List<Item_Parameter>();
    public Item_Parameter[] Item_ParameteList
    {
        get
        {
            return item_ParameterList.ToArray();
        }
    }
}
