using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager gameManagerInstance;

    [SerializeField] private int playerCombatActions = 2;
    [SerializeField] private int enemyCombatActions = 1;
    [SerializeField] private PMovement playerMovement;
    [SerializeField] private List<Enemy> enemyList;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private float enemyActionDelayTime = 1.0f;
    [SerializeField] private GameObject enemyPrefab;

    [Tooltip("X = min amount of enemies, Y = Max amount of enemies")]
    [SerializeField] private Vector2 enemyRange;
    
    PlayerActions playerActions;
    private bool playersTurn = true;

    private List<ActionTypes> playerActionsTypes = new List<ActionTypes>();

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
        playerActions.Quit.QuitGame.performed += EndGame;
    }

    private void Start()
    {
        enemyList = new List<Enemy>();
        playerMovement.SetPlayerActionCount(GetPlayerActions());
        List<GameObject> enemyObjects = new List<GameObject>();
        float numOfEnemies = Random.Range((int)enemyRange.x, ((int)enemyRange.y) + 1);
        for (int i = 0; i < numOfEnemies; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, new Vector2(0, 0), Quaternion.identity);
            enemyObjects.Add(enemy);
        }
        foreach (GameObject enemy in enemyObjects)
        {
            enemyList.Add(enemy.GetComponent<Enemy>());
        }
       

        foreach (var enemy in enemyList)
        {
            if (enemy.GetEnemyActionTypes().Count != enemyCombatActions)
            {
                int randomAction = UnityEngine.Random.Range((int)ActionTypes.MOVE, ((int)ActionTypes.ATTACK + 1));
                enemy.SetEnemyActionTypes((ActionTypes)randomAction);
                enemy.visualizeAttack();
            }
        }
    }

    public void VisualizeEnemyAttacks()
    {
        foreach (var enemy in enemyList)
        {
            enemy.visualizeAttack();
        }
    }

    private void Update()
    {
        UpdateTurn();

        VisualizeEnemyAttacks();
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
            Invoke("PerformEnemyAction", enemyActionDelayTime);
            Debug.Log("ACTION DELAYED");
        }
        if (playersTurn == false)
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
                    break;
                }
            }
            if (shouldBePlayersTurn)
            {
                foreach (var enemy in enemyList)
                {
                    if (enemy.GetEnemyActionTypes().Count != enemyCombatActions)
                    {
                        int randomAction = UnityEngine.Random.Range((int)ActionTypes.MOVE, ((int)ActionTypes.ATTACK + 1));
                        enemy.SetEnemyActionTypes((ActionTypes)randomAction);
                        enemy.visualizeAttack();
                    }
                }
                playersTurn = true;
                playerMovement.SetPlayerActionCount(playerCombatActions);
            }
        }
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
                    foreach (var enemy in enemyList)
                    {
                        enemy.Death();
                    }
                    enemyList.RemoveAll(enemy => enemy.GetEnemyScriptableObject().enemyCurrentHealth <= 0);
                    playerMovement.SetPlayerActionCount(-1);
                    ResetEnemyGrid();
                    playerActionsTypes.RemoveAt(0);
                    if (enemyList.Count <= 0)
                    {
                        SceneManager.LoadScene("EndScene");
                    }
                }
            }
        }
    }

    private void PerformEnemyAction()
    {
        DetermineAction();
    }
    
    private void DetermineAction()
    {
        if (!playersTurn)
        {
            foreach (var enemy in enemyList)
            {
                enemy.visualizeAttack();
                if (enemy.GetEnemyActionTypes().Count != 0)
                {
                    if (enemy.GetEnemyActionTypes()[0] == ActionTypes.MOVE)
                    {
                        enemy.MoveEnemyOnGrid();
                        enemy.GetEnemyActionTypes().RemoveAt(0);
                    }
                    else if (enemy.GetEnemyActionTypes()[0] == ActionTypes.ATTACK)
                    {
                        enemy.Attack();
                        
                        ResetPlayerGrid();
                        
                        enemy.GetEnemyActionTypes().RemoveAt(0);
                    }
                    enemy.SetEnemyActionCount(-1);
                }
            }
            ResetEnemyGrid();
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

    private void EndGame(InputAction.CallbackContext context)
    {
        Debug.Log("Quit Game");
        Application.Quit();
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

    public int GetPlayerActions()
    {
        return playerCombatActions;
    }

    public GridManager GetGridManager()
    {
        return gridManager;
    }

    public bool GetPlayersTurn()
    {
        return playersTurn;
    }

    public List<ActionTypes> GetPlayerActionTypesList()
    {
        return playerActionsTypes;
    }

    public List<Enemy> GetEnemyList()
    {
        return enemyList;
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
