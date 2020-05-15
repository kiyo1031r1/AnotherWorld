using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayData_OwnedMoney
{
    const string playerPrefsKey = "OWNED_MONEY";
    private static PlayData_OwnedMoney instance;
    public static PlayData_OwnedMoney Instance
    {
        get
        {
            if (instance == null)
            {
                instance = PlayerPrefs.HasKey(playerPrefsKey)
                    ? JsonUtility.FromJson<PlayData_OwnedMoney>(PlayerPrefs.GetString(playerPrefsKey))
                    : new PlayData_OwnedMoney();
            }
            return instance;
        }
    }

    [SerializeField] private int ownedMoney = 1000;
    public int OwnedMoney => ownedMoney;
    const int MAX_OWNED_MONEY = 99999;

    private PlayData_OwnedMoney()
    {

    }

    public void Add(int money)
    {
        ownedMoney += money;
        if (ownedMoney > MAX_OWNED_MONEY)
        {
            ownedMoney = MAX_OWNED_MONEY;
        }
    }

    public void Use(int money)
    {
        if(ownedMoney < money)
        {
            throw new Exception("所持金が不足しています");
        }
        ownedMoney -= money;
    }

    public void Use()
    {
        ownedMoney = 0;
    }

    public void Save()
    {
        var saveData = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(playerPrefsKey, saveData);
        PlayerPrefs.Save();
    }

}
