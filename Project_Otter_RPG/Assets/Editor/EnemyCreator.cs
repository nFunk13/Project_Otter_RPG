using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class EnemyCreator : EditorWindow
{
    private string enemyName = "";

    private int enemyHealth = 0;

    private float attackRate = 0;

    private List<MoveData> moveList = new List<MoveData>();
    MoveData moveToAdd = new MoveData();

    private int weight = 0;
    private int weightDecreaseValue = 0;

    [MenuItem("Our Tools/EnemyCreator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EnemyCreator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New Enemy", EditorStyles.boldLabel);

        enemyName = EditorGUILayout.TextField("Name of enemy", enemyName);

        EditorGUILayout.Space();

        enemyHealth = EditorGUILayout.IntField("Health of the enemy", enemyHealth);

        EditorGUILayout.Space();

        attackRate = EditorGUILayout.Slider("Rate of Attack", attackRate, 1.0f, 2.0f);
        EditorGUILayout.Space();

        moveToAdd = EditorGUILayout.ObjectField("Move to Add to enemy List", moveToAdd, typeof(MoveData), false) as MoveData;

        if (GUILayout.Button("Add Move"))
        {
            AddMoveToList();
        }

        weight = EditorGUILayout.IntField("weight for enemy", weight);
        weightDecreaseValue = EditorGUILayout.IntField("How much the weight decreases", weightDecreaseValue);

        if (GUILayout.Button("Create Enemy"))
        {
            CreateEnemy();
        }
      }

      private void CreateEnemy()
      {
          EnemyScriptableObject enemyObj = CreateInstance<EnemyScriptableObject>();

          enemyObj.enemyName = enemyName;
          enemyObj.enemyMaxHealth = enemyHealth;
          enemyObj.attackRate = attackRate;
          enemyObj.moveList = moveList;
          enemyObj.weight = weight;
          enemyObj.weightDecreaseValue = weightDecreaseValue;

          string assetName = $"{enemyObj.enemyName}.asset";
          string folderPath = "Assets/Resources/ScriptableObjects/EnemySO";
          string fullPath = $"{folderPath}/{assetName}";

          AssetDatabase.CreateAsset(enemyObj, fullPath);
          AssetDatabase.SaveAssets();
          AssetDatabase.Refresh();
      }

      private void AddMoveToList()
      {
          moveList.Add(moveToAdd);
      }
}