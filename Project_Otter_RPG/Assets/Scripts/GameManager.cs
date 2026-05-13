using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    private static GameManager gameManagerInstance;

    [SerializeField] private int playerCombatActions = 2;
    [SerializeField] private int enemyCombatActions = 1;
    [SerializeField] private PMovement playerMovement;
    [SerializeField] private List<Enemy> enemyList = new List<Enemy>();
    [SerializeField] private GridManager gridManager;
    PlayerActions playerActions;
    private bool playersTurn = true;

    List<ActionTypes> playerActionsTypes = new List<ActionTypes>();

    public enum ActionTypes
    {
        NO_ACTION,
        MOVE,
        ATTACK
    }


    public static void CreateInstance()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    public static GameManager GetInstance()
    {
        return gameManagerInstance;
    }

    public static void DestroyInstance()
    {
        if (gameManagerInstance != null)
        {
            gameManagerInstance = null;
        }
    }

    private void Awake()
    {
        CreateInstance();

        // Gets the Player Actions for clicking
        playerActions = new PlayerActions();
        playerActions.MouseActions.LeftClick.performed += PerformAction;
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
        UpdateTurn();
        Debug.Log("Action Count: " + playerActionsTypes.Count);
    }

    private void UpdateTurn()
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
                    continue;
                }
                else
                {
                    break;
                }
            }
            playerMovement.SetPlayerActionCount(playerCombatActions);
            playersTurn = true;
        }

        Debug.Log("Whose turn is it (T=Player | F=Enemy: " + playersTurn);
    }

    private void PerformAction(InputAction.CallbackContext ctx)
    {
        PMovement playerMovement = GameObject.Find("Player").GetComponent<PMovement>();
        if (playerActionsTypes.Count != 0)
        {
            if (playerActionsTypes[0] == ActionTypes.MOVE && (playerMovement.getPlayerActionCount() <= 2 && playerMovement.getPlayerActionCount() > 0))
            {
                MovePlayer();
            }
        }
    }

    public void MovePlayer()
    {
        PMovement playerMovement = GameObject.Find("Player").GetComponent<PMovement>();
        if(playerMovement.MovePlayerOnGrid(gridManager.getTileAtPosition(gridManager.MouseToWorldPosition())))
        {
            playerActionsTypes.RemoveAt(0);
        }
    }

    public void SetPlayerAction(ActionTypes action)
    {
        int performedActions = playerActionsTypes.Count;
        if (performedActions < playerCombatActions)
        {
            playerActionsTypes.Add(action);
        }
    }

    public int GetPlayerActions()
    {
        return playerCombatActions;
    }

    public GridManager GetGridManager()
    {
        return gridManager;
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
}
