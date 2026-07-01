using System;
using UnityEngine;

[Serializable]
public struct FrameArrayWrapper
{
    public Sprite[] frames;
}

[Serializable]
public class SpriteAnimation
{
    [Tooltip("Ordered from South -> East -> North -> West.")] 
    public FrameArrayWrapper[] frameSets; // [direction], [sprites]
    
    [Tooltip("Shared across all directions, and MUST MATCH THE NUMBER OF FRAMES IN EACH DIRECTION EXACTLY. If this animation doesn't have specific frame delays, use Uniform Frame Delay instead.")] 
    public float[] frameDelays;

    [Tooltip("Only use this for animations that don't have specific frame delays.")]
    public float uniformFrameDelay;

    public bool looping = false;
    public string animOnEnd; // for non-looping animations
    public string name;
}
