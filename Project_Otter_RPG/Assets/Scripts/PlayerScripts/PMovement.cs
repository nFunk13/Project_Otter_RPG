using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMovement : PlayerManager
{
    private GridManager gridManager;

    private Dictionary<int, GameObject> playerTileDictionary = new Dictionary<int, GameObject>();
    private KeyValuePair<int, GameObject> playerTile;

    private PlayerActions playerActions;
    
    private float moveTime = 0.25f;
    private int playerActionCount = 0;

    private void Awake()
    {
        // Gets the GridManager script
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();

        // Gets the Player Actions for clicking
        playerActions = new PlayerActions();
        playerActions.MouseActions.LeftClick.performed += MovePlayer;
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        PopulateTiles();
        StartSpawn();
    }

    private void PopulateTiles()
    {
        // Places each player tile from the grid manager into the player tile dictionary
        foreach (var tile in gridManager.GetTileDictionary())
        {
            if (tile.Value.gameObject.tag == gridManager.GetPlayerTag())
            {
                playerTileDictionary.Add(tile.Key, tile.Value.gameObject);
            }
        }
    }

    private void StartSpawn()
    {
        // Places the player on a random tile
        int randomNumber = Random.Range(((gridManager.GetEnemyGridWidth() * gridManager.GetEnemyGridHeight())), gridManager.GetTileDictionary().Count - 1);
        GameObject startSpawn = playerTileDictionary[randomNumber];
        this.gameObject.transform.position = new Vector3(startSpawn.transform.position.x, startSpawn.transform.position.y, -1.0f);
        playerTile = new KeyValuePair<int, GameObject>(randomNumber, startSpawn);
    }

    // Moves the player when a tile is clicked
    private void MovePlayerOnGrid(Tile tile)
    {
        // Dictionary contains the potential tiles the player can move to
        Dictionary<int, GameObject> potentialSpots = new Dictionary<int, GameObject>();
        int playerKey = playerTile.Key;

        // Adds the potential keys to a list of integers
        List<int> potentialKeys = new List<int>();
        potentialKeys.Add(playerKey - gridManager.GetPlayerGridWidth());
        potentialKeys.Add(playerKey + gridManager.GetPlayerGridWidth());
        potentialKeys.Add(playerKey - 1);
        potentialKeys.Add(playerKey + 1);

        // Checks to see if each potential key is valid and adds the tile to the dictionary if the key exists
        foreach (var potentailKey in potentialKeys)
        {
            if (playerTileDictionary.TryGetValue(potentailKey, out GameObject tileObject))
            {
                potentialSpots.Add(potentailKey, tileObject);
            }
        }

        // Removes any tiles that the plaeyr shouldn't move to if the player is on the top row or bottom row
        if (playerTile.Key % gridManager.GetPlayerGridWidth() == 0)
        {
            potentialSpots.Remove(playerKey - 1);
        }
        else if (playerTile.Key % gridManager.GetPlayerGridWidth() == gridManager.GetPlayerGridWidth() - 1)
        {
            potentialSpots.Remove(playerKey + 1);
        }

        // Checks if the selected tile is valid and moves the player if so
        foreach (var potentialTile in potentialSpots)
        {
            if (tile != null && tile.gameObject == potentialTile.Value.gameObject)
            {
                // Moves the player
                Vector3 endPosition = new Vector3(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y, -1.0f);
                transform.DOMove(endPosition, moveTime);
                playerTile = new KeyValuePair<int, GameObject>(potentialTile.Key, potentialTile.Value.gameObject);
                playerActionCount--;
                break;
            }
        }
    }

    private void MovePlayer(InputAction.CallbackContext ctx)
    {
        if (playerActionCount > 0)
        {
            MovePlayerOnGrid(gridManager.getTileAtPosition(gridManager.MouseToWorldPosition()));
        }
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    public int getPlayerActionCount()
    {
        return playerActionCount;
    }

    public void SetPlayerActionCount(int countValue)
    {
        playerActionCount = countValue;
    }
}
