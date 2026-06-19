using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] Color enemyColor;
    [SerializeField] Color playerColor;
    private bool characterOn = false;
    private GameObject characterOnTile;
    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void init(bool isEnemy)
    {
        image.color = isEnemy ?  enemyColor : playerColor;
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
