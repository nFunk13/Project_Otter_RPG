using UnityEngine;
using static BillboardManager;

[RequireComponent(typeof(SpriteRenderer))]
public class ShadowInstance : MonoBehaviour
{
    [SerializeField] private SpriteInstance spriteInstance;
    private SpriteBundleData bundleData;
    private SpriteRenderer spriteRenderer;
    private Vector3 directionToLight;

    private Direction currentDirection;
    private int oldFrameIndex;
    private string currentAnimName;

    private Renderer rend;
    private MaterialPropertyBlock mpb;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
        bundleData = spriteInstance.bundleData;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (LightManager.Instance != null)
        {
            Light strongestLight = LightManager.Instance.GetStrongestLightAt(transform.position);
            if (strongestLight != null)
            {
                directionToLight = (strongestLight.transform.position - transform.position).normalized;
                directionToLight.y = 0;

                if (directionToLight.sqrMagnitude < 0.0001f) return;
                Quaternion targetRot = Quaternion.LookRotation(directionToLight);

                transform.rotation = targetRot;
                DetermineSprite();
                ApplyFrame();
            }
        }

    }

    private void DetermineSprite()
    {
        Vector3 facingAngle = transform.parent.forward;
        Direction direction = (Direction)BillboardManager.Instance.GetDirectionFromSubjectToViewer(facingAngle, directionToLight, 4);

        if (direction == currentDirection && spriteInstance.currentAnim.name == currentAnimName && spriteInstance.CurrentFrameIndex == oldFrameIndex) return;
        else
        {
            currentDirection = direction;
            currentAnimName = spriteInstance.currentAnim.name;
            oldFrameIndex = spriteInstance.CurrentFrameIndex;
        }

        var anim = bundleData.animations.Find((anim) => anim.name == spriteInstance.currentAnim.name);
        if (anim != null) spriteRenderer.sprite = anim.frameSets[(int)currentDirection].frames[spriteInstance.CurrentFrameIndex];
    }

    private void ApplyFrame()
    {
        Rect r = spriteRenderer.sprite.rect;
        Texture t = spriteRenderer.sprite.texture;
        Vector4 uvRect = new Vector4(r.x / t.width, r.y / t.height, r.width / t.width, r.height / t.height);

        rend.GetPropertyBlock(mpb);
        mpb.SetVector("_UVRect", uvRect);
        mpb.SetTexture("_BaseMap", t);
        rend.SetPropertyBlock(mpb);
    }
}
