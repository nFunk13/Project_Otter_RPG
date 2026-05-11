using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    private static GameManager gameManagerInstance;

    [SerializeField] private int playerCombatActions = 2;
    [SerializeField] private int enemyCombatActions = 1;
    [SerializeField] private PMovement playerMovement;
    [SerializeField] private List<Enemy> enemyList = new List<Enemy>();
    private bool playersTurn = true;

    public void CreateInstance()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    public GameManager GetInstance()
    {
        return gameManagerInstance;
    }

    public void DestroyInstance()
    {
        if (gameManagerInstance != null)
        {
            gameManagerInstance = null;
        }
    }

    private void Start()
    {
        playerMovement.SetPlayerActionCount(GetPlayerActions());
        
        List<GameObject> enemyObjects = new List<GameObject>();
        enemyObjects.Add(GameObject.FindWithTag("Enemy"));
        foreach (GameObject enemy in enemyObjects)
        {
            enemyList.Add(enemy.GetComponent<Enemy>());
        }
    }

    private void Update()
    {
        if (playersTurn == true && playerMovement.getPlayerActionCount() <= 0)
        {
            foreach (var enemyScript in enemyList)
            {
                enemyScript.SetEnemyActionCount(enemyCombatActions);
                playersTurn = false;
            }
        }
        else if (playersTurn == false)
        {
            foreach (var enemyScript in enemyList)
            {
                if (enemyScript.GetEnemyActionCount() <= 0)
                {
                    playerMovement.SetPlayerActionCount(playerCombatActions);
                    playersTurn = true;
                }
            }
        }

        Debug.Log("Whose turn is it (T=Player | F=Enemy: " + playersTurn);
    }

    public int GetPlayerActions()
    {
        return playerCombatActions;
    }
}
