using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PlayerStatusCSVImporter))]
public class PlayerStatusCSVImporterEXEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var csvImporter = target as PlayerStatusCSVImporter;

        if (GUILayout.Button("プレイヤーデータの作成"))
        {
            CSVDataToScriptableObject(csvImporter);
        }
    }

    private void CSVDataToScriptableObject(PlayerStatusCSVImporter csvImporter)
    {
        if(csvImporter.csvFile == null)
        {
            throw new Exception(csvImporter.name + " : 読み込むcsvファイルがセットされていません");
        }

        //DBファイル作成
        string DBfileName = "PlayerStatusDataData" + ".asset";
        string DBpath = "Assets/00_MyProject/Scripts/Data/PlayerStatusData/AssetsData/" + DBfileName;
        var playerStatusData_DataBase = CreateInstance<PlayerStatusData_DataBase>();

        //データ作成
        string csvText = csvImporter.csvFile.text;
        string[] rowData = csvText.Split('\n');
        for(int i = 1; rowData.Length > i; i++)
        {
            string fileName = "PlayerStatus" + i.ToString() + ".asset";
            string path = "Assets/00_MyProject/Scripts/Data/PlayerStatusData/AssetsData/" + fileName;

            string[] columns = rowData[i].Split(',');
            var columnNum = 0;
            var playerStatusData = CreateInstance<PlayerStatusData>();

            playerStatusData.number = int.Parse(columns[columnNum]);

            columnNum += 1;
            playerStatusData.level = int.Parse(columns[columnNum]);

            columnNum += 1;
            playerStatusData.hitPoint = int.Parse(columns[columnNum]);

            columnNum += 1;
            playerStatusData.attack = int.Parse(columns[columnNum]);

            columnNum += 1;
            playerStatusData.defence = int.Parse(columns[columnNum]);

            columnNum += 1;
            playerStatusData.needExp = int.Parse(columns[columnNum]);

            var asset = (PlayerStatusData)AssetDatabase.LoadAssetAtPath(path, typeof(PlayerStatusData));
            if(asset == null)
            {
                AssetDatabase.CreateAsset(playerStatusData, path);
            }
            else
            {
                EditorUtility.CopySerialized(playerStatusData, asset);
                AssetDatabase.SaveAssets();
            }
            AssetDatabase.Refresh();

            //作成データをDBデータに格納
            var addAsset = (PlayerStatusData)AssetDatabase.LoadAssetAtPath(path, typeof(PlayerStatusData));
            playerStatusData_DataBase.playerStatusDataList.Add(addAsset);
        }

        //DBのアセットファイル作成
        var DBasset = (PlayerStatusData_DataBase)AssetDatabase.LoadAssetAtPath(DBpath, typeof(PlayerStatusData_DataBase));
        if (DBasset == null)
        {
            AssetDatabase.CreateAsset(playerStatusData_DataBase, DBpath);
        }
        else
        {
            EditorUtility.CopySerialized(playerStatusData_DataBase, DBasset);
            AssetDatabase.SaveAssets();
        }
        AssetDatabase.Refresh();
        Debug.Log(csvImporter.name + " : プレイヤーデータの作成が完了しました。");
    }
}
