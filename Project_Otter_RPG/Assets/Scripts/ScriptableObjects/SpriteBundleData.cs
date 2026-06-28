using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteBundleData", menuName = "Scriptable Objects/SpriteBundleData")]
public class SpriteBundleData : ScriptableObject
{
    public List<SpriteAnimation> anims;
}
