using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static BillboardManager;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteInstance : MonoBehaviour
{
    [Header("References")]
    public SpriteBundleData bundleData;
    private GameObject silhouette;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer silhouetteRenderer;
    private Direction _currentDirection;
    public Direction CurrentDirection
    {
        get { return _currentDirection; }
        set 
        { 
            Direction old = _currentDirection;
            if (old != value)
            {
                if (!string.IsNullOrEmpty(currentAnim.name)) Play(currentAnim.name);
                _currentDirection = value;
            }
        }
    }

    [Header("Animation")]
    [SerializeField] private SpriteAnimation currentAnim;
    [SerializeField] private int currentFrameIndex;

    [Tooltip("Will be automatically played in Start().")]
    [SerializeField] private string defaultAnimation;
    
    private Coroutine animTimer;

    [Header("Misc")]
    [SerializeField] private bool hasSilhouette;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (hasSilhouette)
        {
            silhouette = transform.parent.Find("Silhouette").gameObject;
            silhouetteRenderer = silhouette.GetComponent<SpriteRenderer>();
        }

        if (BillboardManager.Instance != null)
        {
            BillboardManager.Instance.spriteInstances.Add(this);
            if(hasSilhouette) BillboardManager.Instance.silhouettes.Add(silhouette);
        }

        if(!string.IsNullOrEmpty(defaultAnimation)) Play(defaultAnimation);
        else Debug.Log("no default animation found");
    }

    private void LateUpdate()
    {
        if (currentAnim == null) return;
        int dir = (int)_currentDirection;
        if (dir >= currentAnim.frameSets.Length) return;
        var frames = currentAnim.frameSets[dir].frames;
        if (currentFrameIndex >= frames.Length) return;
        spriteRenderer.sprite = frames[currentFrameIndex];
        if(hasSilhouette) silhouetteRenderer.sprite = frames[currentFrameIndex];
    }

    private void OnDestroy()
    {
        if (BillboardManager.Instance != null)
            BillboardManager.Instance.spriteInstances.Remove(this);
    }

    public void Play(string name)
    {
        if (animTimer != null) StopCoroutine(animTimer);
        var findAnim = bundleData.animations.Find((anim) => anim.name == name);

        if (findAnim != null)
        {
            currentAnim = findAnim;
            currentFrameIndex = 0;
            animTimer = StartCoroutine(AnimationTimer());
        }
        else Debug.Log($"anim '{name}' not found");
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
        bool hasSpecificFrameDelays = currentAnim.frameDelays.Length > 0;

        do
        {
            for (currentFrameIndex = 0; currentFrameIndex < currentAnim.frameSets[(int)_currentDirection].frames.Length; currentFrameIndex++)
            {
                if (hasSpecificFrameDelays) yield return new WaitForSeconds(currentAnim.frameDelays[currentFrameIndex]);
                else yield return new WaitForSeconds(currentAnim.uniformFrameDelay);
            }
        }
        while (currentAnim.looping);

        if(!string.IsNullOrEmpty(currentAnim.animOnEnd))
        {
            Play(currentAnim.animOnEnd);
        }
    }
}
