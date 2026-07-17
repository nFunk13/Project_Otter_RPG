using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class MoveCreator : EditorWindow
{
    private string moveName = "";
    private int attackDamage = 1;
    private List<int> tileKeys = new List<int>();
    private int tileKey = 1;
    private bool tileSpillage = false;
    private string folderPath = "Assets/Resources/ScriptableObjects/MoveSO";

    [MenuItem("Our Tools/MoveCreator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MoveCreator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New Move", EditorStyles.boldLabel);

        moveName = EditorGUILayout.TextField("Name of Move", moveName);
        attackDamage = EditorGUILayout.IntField("Damage", attackDamage);

        EditorGUILayout.Space();
        tileKey = EditorGUILayout.IntSlider("Keys of Tiles", tileKey, 1, 16);
        if (GUILayout.Button("Add Key"))
        {
            AddKeyToTileList(tileKey);
        }

        tileSpillage = EditorGUILayout.Toggle(tileSpillage);

        if (GUILayout.Button("CreateMove"))
        {
            CreateMove();
        }

        //moveData = EditorGUILayout.ObjectField("MoveData", moveData, typeof(MoveData), false) as MoveData;
    }

    private void AddKeyToTileList(int key)
    {
        tileKeys.Add(key);
    }

    private void CreateMove()
    {
        tileKeys.Sort();
        MoveData moveData = CreateInstance<MoveData>();

        moveData.moveName = moveName;
        moveData.attackDamage = attackDamage;
        moveData.tileKeys = tileKeys;
        moveData.leftMostTileKey = tileKeys.First();
        moveData.centerTileKey = tileKeys[Mathf.CeilToInt(tileKeys.Count / 2)];
        moveData.rightMostTileKey = tileKeys[tileKeys.Count - 1];
        moveData.tileSpillage = tileSpillage;

        string assetName = $"{moveData.moveName}.asset";
        string fullPath = $"{folderPath}/{assetName}";

        AssetDatabase.CreateAsset(moveData, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
