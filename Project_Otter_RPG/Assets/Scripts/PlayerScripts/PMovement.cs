using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMovement : PlayerManager
{
    private GridManager gridManager;

    private Dictionary<int, GameObject> playerTileDictionary = new Dictionary<int, GameObject>();
    private KeyValuePair<int, GameObject> playerTile;

    private PlayerActions playerActions;

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
        int randomNumber = Random.Range(16, gridManager.GetTileDictionary().Count - 1);
        GameObject startSpawn = playerTileDictionary[randomNumber];
        this.gameObject.transform.position = new Vector3(startSpawn.transform.position.x, startSpawn.transform.position.y, -1.0f);
        playerTile = new KeyValuePair<int, GameObject>(randomNumber, startSpawn);
    }

    // Moves the player when a tile is clicked
    private void MovePlayerOnGrid(Tile tile)
    {
        // Checks to see if the click was on the player's grid
        if (tile != null && tile.gameObject.tag == gridManager.GetPlayerTag())
        {
            // Gets the specific dictionary entry of the tile that the player clicked on
            foreach (var tileObj in playerTileDictionary)
            {
                if (tile.gameObject == tileObj.Value.gameObject)
                {
                    // Checks if the desired tile to move to is good
                    if (tileObj.Key == playerTile.Key - 1 || tileObj.Key == playerTile.Key + 1 || tileObj.Key == playerTile.Key - 4 || tileObj.Key == playerTile.Key + 4)
                    {
                        // Prevents from going from bottom tile to the left column's left tile and the top tile to the left column's bottom tile
                        if ((playerTile.Key % 4 == 0 && tileObj.Key == playerTile.Key - 1) || (tileObj.Key % 4 == 0 && (playerTile.Key - 1) != tileObj.Key))
                        {
                            break;
                        }

                        // Moves the player
                        this.gameObject.transform.position = new Vector3(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y, -1.0f);
                        playerTile = new KeyValuePair<int, GameObject>(tileObj.Key, tileObj.Value.gameObject);
                        break;
                    }
                }
            }
        }
    }

    private void MovePlayer(InputAction.CallbackContext ctx)
    {
        MovePlayerOnGrid(gridManager.getTileAtPosition(gridManager.MouseToWorldPosition()));
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
