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
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();

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
        int randomNumber = Random.Range(16, gridManager.GetTileDictionary().Count - 1);
        GameObject startSpawn = playerTileDictionary[randomNumber];
        this.gameObject.transform.position = new Vector3(startSpawn.transform.position.x, startSpawn.transform.position.y, -1.0f);
        playerTile = new KeyValuePair<int, GameObject>(randomNumber, startSpawn);
    }

    private void MovePlayerOnGrid(Tile tile)
    {
        if (tile != null && tile.gameObject.tag == gridManager.GetPlayerTag())
        {
            foreach (var tileObj in playerTileDictionary)
            {
                if (tile.gameObject == tileObj.Value.gameObject)
                {
                    if (tileObj.Key == playerTile.Key - 1 || tileObj.Key == playerTile.Key + 1 || tileObj.Key == playerTile.Key - 4 || tileObj.Key == playerTile.Key + 4)
                    {
                        if ((playerTile.Key % 4 == 0 && tileObj.Key == playerTile.Key - 1) || (tileObj.Key % 4 == 0 && (playerTile.Key - 1) != tileObj.Key))
                        {
                            break;
                        }
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
        Debug.Log("CLICKED!");
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
