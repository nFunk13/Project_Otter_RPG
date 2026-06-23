using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpriteArrayWrapper
{
    public Sprite[] sprites;
}

[CreateAssetMenu(fileName = "SpriteBundleData", menuName = "Scriptable Objects/SpriteBundleData")]
public class SpriteBundleData : ScriptableObject
{
    [Tooltip("The walk sprites for each direction: North, South, East, West.")]
    public SpriteArrayWrapper[] walkSprites;
}
