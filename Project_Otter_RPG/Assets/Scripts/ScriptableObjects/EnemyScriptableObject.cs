using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Scriptable Objects/EnemyScriptableObject")]
public class EnemyScriptableObject : ScriptableObject
{
    [Tooltip("Enemy Name")]
    public string enemyName = "";

    [Tooltip("Enemy Health")]
    public int enemyHealth = 0;

    [Tooltip("Enemy Moves")]
    [SerializeField] public List<MoveData> moveList = new List<MoveData>();
}
