using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "MoveData", menuName = "Scriptable Objects/MoveData")]
public class MoveData : ScriptableObject
{
    [Header("Move name")]
    public string moveName = "";

    [Header("Move damage")]
    public int attackDamage = 0;

    [Header("Wanted Tiles")]
    public List<int> tileKeys = new List<int>();

    [Header("Tiles highest in a certain direction")]
    [Tooltip("Left most tile -lowest value-")]
    public int leftMostTileKey;

    [Tooltip("Center tile -Where mouse is-")]
    public int centerTileKey;

    [Tooltip("Right most tile -largest value-")]
    public int rightMostTileKey;

    [Header("Whether an tile will spill over to the next tile")]
    public bool tileSpillage = false;
}
