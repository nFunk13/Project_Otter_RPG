using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] Color enemyColor;
    [SerializeField] Color playerColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void init(bool isEnemy)
    {
        spriteRenderer.color = isEnemy ?  enemyColor : playerColor;
    }
}
