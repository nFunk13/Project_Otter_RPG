using System;
using UnityEngine;

[Serializable]
public struct SpriteArrayWrapper
{
    public Sprite[] sprites;
}

[Serializable]
public class SpriteAnimation
{
    [Tooltip("Ordered from South -> East -> North -> West.")] public SpriteArrayWrapper[] sprites; // [direction], [sprites]
    public bool looping = false;
    public float spriteDelay;
    public string animOnEnd; // for non-looping animations
    public string name;
}
