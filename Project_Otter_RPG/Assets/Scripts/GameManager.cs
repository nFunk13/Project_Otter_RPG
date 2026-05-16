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

    private List<ActionTypes> playerActionsTypes = new List<ActionTypes>();
    private List<ActionTypes> enemyActionTypes = new List<ActionTypes>();

    public enum ActionTypes
    {
        NO_ACTION,
        MOVE,
        ATTACK
    }

    // Creates an instance of the Game Manager if it is not set to null
    public static void CreateInstance()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    // Allows other scripts to get the game manager script
    public static GameManager GetInstance()
    {
        return gameManagerInstance;
    }

    // Destroys the instance
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
        PerformEnemyAction();
        UpdateTurn();
        Debug.Log("Action Count: " + playerActionsTypes.Count);
    }

    // Updates whether it is the player or enemies turn
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
            bool shouldBePlayersTurn = false;
            foreach (var enemyScript in enemyList)
            {
                if (enemyScript.GetEnemyActionCount() <= 0 && enemyScript.GetEnemyActionCount() == 0)
                {
                    shouldBePlayersTurn = true;
                }
                else
                {
                    shouldBePlayersTurn = false;
                }
            }
            if (shouldBePlayersTurn)
            {
                playerMovement.SetPlayerActionCount(playerCombatActions);
                playersTurn = true;
            }
        }

        Debug.Log("Whose turn is it (T=Player | F=Enemy: " + playersTurn);
    }

    // Performs actions based on the player's action list
    private void PerformAction(InputAction.CallbackContext ctx)
    {
        PMovement playerMovement = GameObject.Find("Player").GetComponent<PMovement>();
        PAttack playerAttack = GameObject.Find("Player").GetComponent<PAttack>();
        
        // Checks to make sure list is not empty
        if (playerActionsTypes.Count != 0 && playersTurn)
        {
            bool actionRange = (playerMovement.getPlayerActionCount() <= 2 && playerMovement.getPlayerActionCount() > 0); // Range for acceptable actions
            // What to do with the movement action
            if (playerActionsTypes[0] == ActionTypes.MOVE && actionRange)
            {
                playerMovement.MovePlayer();

            }
            // What to do with the attack action
            else if (playerActionsTypes[0] == ActionTypes.ATTACK && actionRange)
            {
                if (playerAttack.Attack(gridManager.getTileAtPosition(gridManager.MouseToWorldPosition())))
                {
                    playerMovement.SetPlayerActionCount(-1);
                    ResetEnemyGrid();
                    playerActionsTypes.RemoveAt(0);
                }
            }
        }
    }

    private void PerformEnemyAction()
    {
        if (!playersTurn)
        {
            foreach (var enemy in enemyList)
            {
                int randomAction = Random.Range((int)ActionTypes.ATTACK, (int)ActionTypes.ATTACK);
                enemyActionTypes.Add((ActionTypes)randomAction);
                if (enemyActionTypes[0] == ActionTypes.MOVE)
                {
                    enemy.MoveEnemyOnGrid();
                }
                else if (enemyActionTypes[0] == ActionTypes.ATTACK)
                {
                    enemy.visualizeAttack();
                }
                enemy.SetEnemyActionCount(-1);
            }

        }
    }

    // Resets the color of the enemy grid
    public void ResetEnemyGrid()
    {
        foreach (var tile in gridManager.GetEnemyTileDictionary().Values)
        {
            if (tile.GetComponent<SpriteRenderer>().color != Color.red)
            {
                tile.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    public void ResetPlayerGrid()
    {
        foreach (var tile in gridManager.GetPlayerTileDictionary().Values)
        {
            if (tile.GetComponent<SpriteRenderer>().color != Color.green)
            {
                tile.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }

    // Sets the next action the player will perform
    public void SetPlayerAction(ActionTypes action)
    {
        int performedActions = playerActionsTypes.Count;
        if (performedActions < playerCombatActions)
        {
            playerActionsTypes.Add(action);
        }
    }

    public void SetEnemyAction(ActionTypes action)
    {
        enemyActionTypes.Add(action);
    }

    public int GetPlayerActions()
    {
        return playerCombatActions;
    }

    public GridManager GetGridManager()
    {
        return gridManager;
    }

    public List<ActionTypes> GetPlayerActionTypesList()
    {
        return playerActionsTypes;
    }

    public List<ActionTypes> GetEnemyActionTypesList()
    {
        return enemyActionTypes;
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
