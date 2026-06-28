using System.Collections;
using UnityEngine;
using static BillboardManager;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteInstance : MonoBehaviour
{
    [Header("References")]
    public SpriteBundleData bundleData;
    private SpriteRenderer spriteRenderer;
    public Direction currentDirection;

    [Header("Animation")]
    [SerializeField] private SpriteAnimation currentAnim;
    [SerializeField] private int currentFrameIndex;
    private Coroutine animTimer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (BillboardManager.Instance != null)
        {
            BillboardManager.Instance.spriteInstances.Add(this);
        }
        else
        {
            Debug.Log("billboard manager null");
        }        
    }

    private void LateUpdate()
    {
        if (currentAnim == null) return;
        int dir = (int)currentDirection;
        if (dir >= currentAnim.sprites.Length) return;
        var frames = currentAnim.sprites[dir].sprites;
        if (currentFrameIndex >= frames.Length) return;
        spriteRenderer.sprite = frames[currentFrameIndex];
    }

    private void OnDestroy()
    {
        if (BillboardManager.Instance != null)
            BillboardManager.Instance.spriteInstances.Remove(this);
    }

    public void Play(string name)
    {
        if (animTimer != null) StopCoroutine(animTimer);
        var findAnim = bundleData.anims.Find((anim) => anim.name == name);

        if (findAnim != null)
        {
            currentAnim = findAnim;
            currentFrameIndex = 0;
            animTimer = StartCoroutine(AnimationTimer());
        }
        else Debug.LogWarning($"anim '{name}' not found in bundle", this);
    }

    public void Stop(string fallback = null)
    {
        if (string.IsNullOrEmpty(fallback))
        {
            Play("idle");
        }
        else Play(fallback);
    }

    private IEnumerator AnimationTimer()
    {
        do
        {
            for (currentFrameIndex = 0; currentFrameIndex < currentAnim.sprites[(int)currentDirection].sprites.Length; currentFrameIndex++)
            {
                yield return new WaitForSeconds(currentAnim.spriteDelay);
            }
        }
        while (currentAnim.looping);

        if(!string.IsNullOrEmpty(currentAnim.animOnEnd))
        {
            Play(currentAnim.animOnEnd);
        }
    }
}
