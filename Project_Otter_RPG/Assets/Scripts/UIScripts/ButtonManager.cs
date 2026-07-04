using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> moveButtons = new List<GameObject>();

    // Adds move action to the actionTypes List in GameManager
    public void MoveAction()
    {
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.MOVE);
        ResetActiveButton();
    }

    // Adds attack action to the actionTypes List in GameManager
    public void AttackAction()
    {
        PAttack playerAttack = GameObject.Find("Player_UI").GetComponent<PAttack>();
        gameObject.transform.Find("Action_Menu").gameObject.SetActive(false);
        gameObject.transform.Find("Moves_Menu").gameObject.SetActive(true);
        for (int i = 0; i < moveButtons.Count; i++)
        {
            if (i > (playerAttack.GetMoves().Count - 1))
            {
                break;
            }
            moveButtons[i].gameObject.SetActive(true);
            moveButtons[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = playerAttack.GetMoves()[i].moveName;
        }
        EventSystem.current.SetSelectedGameObject(moveButtons[0]);
    }

    public void MoveOne()
    {
        PAttack playerAttack = GameObject.Find("Player_UI").GetComponent<PAttack>();
        string moveName = moveButtons[0].gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        playerAttack.SetChosenMoveData(playerAttack.ChosenMove(moveName));
        gameObject.transform.Find("Moves_Menu").gameObject.SetActive(false);
        foreach (var button in moveButtons)
        {
            if (button.activeInHierarchy)
            {
                button.SetActive(false);
            }
            else
            {
                break;
            }
        }
        gameObject.transform.Find("Action_Menu").gameObject.SetActive(true);
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.ATTACK);
        ResetActiveButton();
    }

    public void MoveTwo()
    {
        PAttack playerAttack = GameObject.Find("Player_UI").GetComponent<PAttack>();
        string moveName = moveButtons[1].gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        playerAttack.SetChosenMoveData(playerAttack.ChosenMove(moveName));
        gameObject.transform.Find("Moves_Menu").gameObject.SetActive(false);
        foreach (var button in moveButtons)
        {
            if (button.activeInHierarchy)
            {
                button.SetActive(false);
            }
            else
            {
                break;
            }
        }
        gameObject.transform.Find("Action_Menu").gameObject.SetActive(true);
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.ATTACK);
        ResetActiveButton();
    }

    public void MoveThree()
    {
        PAttack playerAttack = GameObject.Find("Player_UI").GetComponent<PAttack>();
        string moveName = moveButtons[2].gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        playerAttack.SetChosenMoveData(playerAttack.ChosenMove(moveName));
        gameObject.transform.Find("Moves_Menu").gameObject.SetActive(false);
        foreach (var button in moveButtons)
        {
            if (button.activeInHierarchy)
            {
                button.SetActive(false);
            }
            else
            {
                break;
            }
        }
        gameObject.transform.Find("Action_Menu").gameObject.SetActive(true);
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.ATTACK);
        ResetActiveButton();
    }

    public void MoveFour()
    {
        PAttack playerAttack = GameObject.Find("Player_UI").GetComponent<PAttack>();
        string moveName = moveButtons[3].gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        playerAttack.SetChosenMoveData(playerAttack.ChosenMove(moveName));
        gameObject.transform.Find("Moves_Menu").gameObject.SetActive(false);
        foreach (var button in moveButtons)
        {
            if (button.activeInHierarchy)
            {
                button.SetActive(false);
            }
            else
            {
                break;
            }
        }
        gameObject.transform.Find("Action_Menu").gameObject.SetActive(true);
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.ATTACK);
        ResetActiveButton();
    }

    private void ResetActiveButton()
    {
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Move_Button"));
    }

    public void StartGame()
    {
        SceneManager.LoadScene("PlayerCombatMovement");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
