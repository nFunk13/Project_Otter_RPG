using UnityEngine;

[CreateAssetMenu(fileName = "PlayableCharacterData", menuName = "Scriptable Objects/PlayableCharacterData")]
public class PlayableCharacterData : ScriptableObject
{
    [Tooltip("Character Name")]
    public string characterName = "";

    [Tooltip("Player Health")]
    public int characterMaxHealth = 0;
    [HideInInspector] public int characterCurrentHealth = 0;
}
