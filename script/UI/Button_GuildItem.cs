using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_GuildItem : MonoBehaviour
{
    private Item item;
    [SerializeField] private Text itemName;
    [SerializeField] private Text currentNumber;
    [SerializeField] private Text targetNumber;

    public Item Item => item;
    public Text CurrentNumber => currentNumber;
    public Text TargetNumber => targetNumber;


    public void SetGuildItem(Item item)
    {
        this.item = item;
        itemName.text = this.item.ItemName;
    }

}
