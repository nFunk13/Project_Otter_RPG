using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MoveData", menuName = "Scriptable Objects/MoveData")]
public class MoveData : ScriptableObject
{
    [Tooltip("Move name")]
    public string moveName = "";

    [Tooltip("Move damage")]
    public int attackDamage = 0;

    [Tooltip("Wanted Tiles")]
    public List<int> tileKeys = new List<int>();
}
