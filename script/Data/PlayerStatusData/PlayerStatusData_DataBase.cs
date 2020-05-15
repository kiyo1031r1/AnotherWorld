using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/PlayerStatusData/PlayerStatusData_DataBase")]

public class PlayerStatusData_DataBase : ScriptableObject
{
    public List<PlayerStatusData> playerStatusDataList = new List<PlayerStatusData>();
}
