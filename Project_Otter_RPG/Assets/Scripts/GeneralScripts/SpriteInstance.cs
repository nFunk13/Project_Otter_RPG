using Unity.VisualScripting;
using UnityEngine;
using static SpriteBillboarder;

public class SpriteInstance : MonoBehaviour
{
    [UnitHeaderInspectable("References")]
    public SpriteBundleData bundleData;
    public SpriteRenderer spriteRenderer;
    [Tooltip("Which set of directional sprites to use when animating")] public Direction directionSet;
}
