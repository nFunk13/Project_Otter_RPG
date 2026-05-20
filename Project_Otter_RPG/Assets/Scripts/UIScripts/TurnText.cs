using TMPro;
using UnityEngine;

public class TurnText : MonoBehaviour
{
    private void Start()
    {
        DisplayTurn();
    }

    private void Update()
    {
        DisplayTurn();
    }

    private void DisplayTurn()
    {
        if (GameManager.GetInstance().GetPlayersTurn())
        {
            this.gameObject.GetComponent<TextMeshProUGUI>().text = "Player's turn";
            this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.lightGreen;
        }
        else if (!GameManager.GetInstance().GetPlayersTurn())
        {
            this.gameObject.GetComponent<TextMeshProUGUI>().text = "Enemy's turn";
            this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.violetRed;
        }
    }
}
