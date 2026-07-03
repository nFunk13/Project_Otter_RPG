using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PMovement : PlayerManager
{
    private GridManager gridManager;
    [SerializeField] private Canvas attackCanvas;

    private KeyValuePair<int, GameObject> playerTile;
    Dictionary<int, GameObject> potentialMoveTiles = new Dictionary<int, GameObject>();

    private float moveTime = 0.25f;
    [SerializeField] private int playerActionCount = 0;
    private GameObject chosenMoveLocation;

    private enum DirectionToMoveTile
    {
        LEFT = 1,
        DOWN = 2,
        UP = 3,
        RIGHT = 4
    };

    public override void Init(PlayerSystems system)
    {
        // Gets the GridManager script
        base.Init(system);
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        base.playerActions.Combat.AddTileMovement.performed += AddTileMovement;
    }

    public override void Tick()
    {
        base.Tick();
        Debug.Log("PLAYER ACTION COUNT: " + playerActionCount);
    }

    public override void FixedTick()
    {
        base.FixedTick();
        VisualizeMovement();
        GameManager.GetInstance().VisualizeEnemyAttacks();
    }

    public void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        StartSpawn();
    }

    private void AddTileMovement(InputAction.CallbackContext context)
    {
        if (GameManager.GetInstance().GetPlayerActionTypesList().Count > 0 && GameManager.GetInstance().GetPlayerActionTypesList()[0] == GameManager.ActionTypes.MOVE)
        {
            if (context.control.name == PlayerManager.InputKeyNames.upArrow.ToString())
            {
                if(potentialMoveTiles.ContainsKey((int)DirectionToMoveTile.UP))
                {
                    chosenMoveLocation = potentialMoveTiles[(int)DirectionToMoveTile.DOWN];
                }
            }
            else if (context.control.name == PlayerManager.InputKeyNames.downArrow.ToString())
            {
                if (potentialMoveTiles.ContainsKey((int)DirectionToMoveTile.DOWN))
                {
                    chosenMoveLocation = potentialMoveTiles[(int)DirectionToMoveTile.DOWN];
                }
            }
            else if (context.control.name == PlayerManager.InputKeyNames.rightArrow.ToString())
            {
                if (potentialMoveTiles.ContainsKey((int)DirectionToMoveTile.RIGHT))
                {
                    chosenMoveLocation = potentialMoveTiles[(int)DirectionToMoveTile.RIGHT];
                }
            }
            else if (context.control.name == PlayerManager.InputKeyNames.leftArrow.ToString())
            {
                if (potentialMoveTiles.ContainsKey((int)DirectionToMoveTile.LEFT))
                {
                    chosenMoveLocation = potentialMoveTiles[(int)DirectionToMoveTile.LEFT];
                }
            }
            Debug.Log("CHOSEN MOVE LOCATION: " + chosenMoveLocation);
        }
    }

    private void StartSpawn()
    {
        // Places the player on a random tile
        int randomNumber = Random.Range(((gridManager.GetEnemyGridWidth() * gridManager.GetEnemyGridHeight())), gridManager.GetPlayerTileDictionary().Count);
        Vector3 targetPosition;
        GetScreenPosOfTile(GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[randomNumber].gameObject.GetComponent<RectTransform>().position, out targetPosition);
        GameObject startSpawn = GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[randomNumber];
        this.gameObject.transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        startSpawn.GetComponent<Tile>().SetCharacterOn(true);
        startSpawn.GetComponent<Tile>().SetCharacterOnTile(this.gameObject);
        playerTile = new KeyValuePair<int, GameObject>(randomNumber, startSpawn.gameObject);
        
    }

    private void GetScreenPosOfTile(Vector3 worldPos, out Vector3 finalScreenPos)
    {
        Vector3 uiWorldPoint = worldPos;
        finalScreenPos = RectTransformUtility.WorldToScreenPoint(null, uiWorldPoint);
    }

    public void VisualizeMovement()
    {
        if (GameManager.GetInstance().GetPlayerActionTypesList().Count != 0 && GameManager.GetInstance().GetPlayerActionTypesList()[0] == GameManager.ActionTypes.MOVE)
        {
            if (potentialMoveTiles.Count == 0)
            {
                Dictionary<int, GameObject> playerTiles = GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary();
                int playerKey = playerTile.Key;

                // Adds the potential keys to a list of integers
                List<int> potentialKeys = new List<int>();
                potentialKeys.Add(playerKey - gridManager.GetPlayerGridWidth());
                potentialKeys.Add(playerKey - 1);
                potentialKeys.Add(playerKey + 1);
                potentialKeys.Add(playerKey + gridManager.GetPlayerGridWidth());

                // Checks to see if each potential key is valid and adds the tile to the dictionary if the key exists
                int key = 1;
                foreach (var potentailKey in potentialKeys)
                {
                    if (playerTiles.TryGetValue(potentailKey, out GameObject tileObject))
                    {
                        potentialMoveTiles.Add(key, tileObject);
                    }
                    key++;
                }
            }

            foreach (var tile in potentialMoveTiles.Values)
            {
                tile.GetComponent<Image>().color = Color.magenta;
            }
        }
    }

    // Moves the player when a tile is clicked
    public void MovePlayerOnGrid()
    {
        transform.DOMove(chosenMoveLocation.transform.position, moveTime).SetUpdate(UpdateType.Fixed);
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();

        // Changing where the player is on the tiles
        playerTile.Value.gameObject.GetComponent<Tile>().SetCharacterOn(false);
        playerTile.Value.gameObject.GetComponent<Tile>().SetCharacterOnTile(null);
        chosenMoveLocation.gameObject.GetComponent<Tile>().SetCharacterOn(true);
        chosenMoveLocation.gameObject.GetComponent<Tile>().SetCharacterOnTile(this.gameObject);

        playerTile = new KeyValuePair<int, GameObject>(GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary().FirstOrDefault(x => x.Value == chosenMoveLocation.gameObject).Key, chosenMoveLocation.gameObject);
        playerActionCount--;

        List<Enemy> eList = GameManager.GetInstance().GetEnemyList();
        foreach (var removeTile in potentialMoveTiles)
        {
            removeTile.Value.gameObject.GetComponent<Image>().color = Color.green;
            foreach (Enemy enemy in eList)
            {
                foreach (var eAttackTile in enemy.GetAttackTiles())
                {
                    if (removeTile.Value.gameObject == eAttackTile)
                    {
                        removeTile.Value.gameObject.GetComponent<Image>().color = Color.orange;
                        break;
                    }
                }
            }
        }
        potentialMoveTiles = new Dictionary<int, GameObject>();
    }

    public void MovePlayer()
    {
        // Removes the first action from the player action list if the player has moved
        if (chosenMoveLocation != null)
        {
            MovePlayerOnGrid();
            GameManager.GetInstance().GetPlayerActionTypesList().RemoveAt(0);
            chosenMoveLocation = null;
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

    public Dictionary<int, GameObject> GetPotentialMoveTiles()
    {
        return potentialMoveTiles;
    }

    private void OnDisable()
    {
        base.playerActions.Disable();
    }
}
