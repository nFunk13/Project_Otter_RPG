using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> moveButtons = new List<GameObject>();

    // Adds move action to the actionTypes List in GameManager
    public void MoveAction()
    {
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.MOVE);
    }

    // Adds attack action to the actionTypes List in GameManager
    public void AttackAction()
    {
        PAttack playerAttack = GameObject.Find("Player").GetComponent<PAttack>();
        gameObject.transform.Find("Action_Menu").gameObject.SetActive(false);
        gameObject.transform.Find("Moves_Menu").gameObject.SetActive(true);
        for (int i = 0; i < moveButtons.Count; i++)
        {
            if (i > (playerAttack.GetMoves().Count - 1))
            {
                break;
            }
            moveButtons[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = playerAttack.GetMoves()[i].moveName;
        }
    }

    public void MoveOne()
    {
        PAttack playerAttack = GameObject.Find("Player").GetComponent<PAttack>();
        playerAttack.SetChosenMoveData(playerAttack.ChosenMove(moveButtons[0].name));
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.ATTACK);
    }
}
