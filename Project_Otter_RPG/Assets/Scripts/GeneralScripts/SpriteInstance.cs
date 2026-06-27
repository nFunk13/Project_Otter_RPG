using UnityEngine;
using static BillboardManager;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteInstance : MonoBehaviour
{
    [Header("References")]
    public SpriteBundleData bundleData;
    private SpriteRenderer spriteRenderer;
    private Direction _directionSet;
    public Direction directionSet
    {
        set
        {
            if (value == _directionSet) return;
            _directionSet = value;
            spriteRenderer.sprite = bundleData.idleSprites[(int)(value)]; //only setting idle sprites for now but ideally walk sprites would also be changed here
        }
        get => _directionSet;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _directionSet = Direction.SOUTH;
        if (BillboardManager.Instance != null)
        {
            BillboardManager.Instance.spriteInstances.Add(this);
        }
        else
        {
            Debug.Log("billboard manager null");
        }
    }

    private void OnDestroy()
    {
        if (BillboardManager.Instance != null)
            BillboardManager.Instance.spriteInstances.Remove(this);
    }
}
