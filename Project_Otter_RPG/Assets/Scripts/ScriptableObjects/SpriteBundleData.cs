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
    [Tooltip("Ordered from North -> West -> South -> East.")] public Sprite[] idleSprites;
    public SpriteArrayWrapper[] walkSprites; // unused for now
}
