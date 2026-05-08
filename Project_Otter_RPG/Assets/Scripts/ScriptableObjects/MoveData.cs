using UnityEngine;

[CreateAssetMenu(fileName = "MoveData", menuName = "Scriptable Objects/MoveData")]
public class MoveData : ScriptableObject
{
    [Tooltip("Move name")]
    public string moveName = "";

    [Tooltip("Move damage")]
    public int attackDamage = 0;
}
