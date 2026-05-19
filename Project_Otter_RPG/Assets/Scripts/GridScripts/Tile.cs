using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] Color enemyColor;
    [SerializeField] Color playerColor;
    private bool characterOn = false;
    private GameObject characterOnTile;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void init(bool isEnemy)
    {
        spriteRenderer.color = isEnemy ?  enemyColor : playerColor;
    }

    public void SetCharacterOn(bool truthValue)
    {
        characterOn = truthValue;
    }

    public bool GetCharacterOn()
    {
        return characterOn;
    }

    public void SetCharacterOnTile(GameObject character)
    {
        characterOnTile = character;
    }

    public GameObject GetCharacterOnTile()
    {
        return characterOnTile;
    }
}
