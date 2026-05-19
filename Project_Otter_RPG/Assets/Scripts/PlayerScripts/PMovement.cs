using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMovement : PlayerManager
{
    private GridManager gridManager;

    private KeyValuePair<int, GameObject> playerTile;
    
    private float moveTime = 0.25f;
    private int playerActionCount = 0;

    private void Awake()
    {
        // Gets the GridManager script
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
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
        StartSpawn();
    }

    private void StartSpawn()
    {
        // Places the player on a random tile
        int randomNumber = Random.Range(((gridManager.GetEnemyGridWidth() * gridManager.GetEnemyGridHeight())), gridManager.GetPlayerTileDictionary().Count);
        GameObject startSpawn = GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[randomNumber];
        this.gameObject.transform.position = new Vector3(startSpawn.transform.position.x, startSpawn.transform.position.y, -1.0f);
        startSpawn.GetComponent<Tile>().SetCharacterOn(true);
        startSpawn.GetComponent<Tile>().SetCharacterOnTile(this.gameObject);
        playerTile = new KeyValuePair<int, GameObject>(randomNumber, startSpawn.gameObject);
        
    }

    // Moves the player when a tile is clicked
    public bool MovePlayerOnGrid(GameObject tile)
    {
        Dictionary<int, GameObject> playerTiles = GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary();
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
            if (playerTiles.TryGetValue(potentailKey, out GameObject tileObject))
            {
                potentialSpots.Add(potentailKey, tileObject);
            }
        }

        // Removes any tiles that the plaeyr shouldn't move to if the player is on the top row or bottom row
        if (playerTile.Key % gridManager.GetPlayerGridWidth() == 0)
        {
            potentialSpots.Remove(playerKey + 1);
        }
        else if (playerTile.Key % gridManager.GetPlayerGridWidth() == gridManager.GetPlayerGridWidth() - 3)
        {
            potentialSpots.Remove(playerKey - 1);
        }

        // Checks if the selected tile is valid and moves the player if so
        foreach (var potentialTile in potentialSpots)
        {
            if (tile != null && tile.gameObject == potentialTile.Value.gameObject)
            {
                // Moves the player
                Vector3 endPosition = new Vector3(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y, -1.0f);
                transform.DOMove(endPosition, moveTime).SetUpdate(UpdateType.Fixed);
                CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
                playerTile.Value.gameObject.GetComponent<Tile>().SetCharacterOn(false);
                potentialTile.Value.gameObject.GetComponent<Tile>().SetCharacterOn(true);
                potentialTile.Value.gameObject.GetComponent<Tile>().SetCharacterOnTile(this.gameObject);
                playerTile = new KeyValuePair<int, GameObject>(potentialTile.Key, potentialTile.Value.gameObject);
                playerActionCount--;
                return true;
            }
        }
        return false;
    }

    public void MovePlayer()
    {
        // Removes the first action from the player action list if the player has moved
        if (MovePlayerOnGrid(gridManager.getTileAtPosition(gridManager.MouseToWorldPosition())))
        {
            GameManager.GetInstance().GetPlayerActionTypesList().RemoveAt(0);
        }
    }

    // Gets the Player Action Count
    public int getPlayerActionCount()
    {
        return playerActionCount;
    }

    // Sets the Player Action Count
    public void SetPlayerActionCount(int countValue)
    {
        playerActionCount += countValue;
    }
}
