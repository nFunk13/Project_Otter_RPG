using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // Adds move action to the actionTypes List in GameManager
    public void MoveAction()
    {
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.MOVE);
    }

    // Adds attack action to the actionTypes List in GameManager
    public void AttackAction()
    {
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.ATTACK);
    }
}
