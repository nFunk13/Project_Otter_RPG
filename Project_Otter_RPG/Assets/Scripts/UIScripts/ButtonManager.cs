using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void MoveAction()
    {
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.MOVE);
    }

    public void AttackAction()
    {
        GameManager.GetInstance().SetPlayerAction(GameManager.ActionTypes.ATTACK);
    }
}
