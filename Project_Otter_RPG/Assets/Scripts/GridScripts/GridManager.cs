using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    // Variables for creating enemy grid
    [SerializeField] int eWidth = 3, eHeight = 3;
    [SerializeField] Vector2 enemyGridStart = new Vector2(0.20f, 0.80f);
    [SerializeField] float offset = 10.0f;

    [SerializeField] int pWidth = 3, pHeight = 3;
    [SerializeField] Vector2 playerGridStart = new Vector2(-1.0f, 3.0f);
    
    // Variables for the tiles themselves
    [SerializeField] GameObject tile;
    [SerializeField] LayerMask mask;
    [SerializeField] string enemyTileTag;
    [SerializeField] string playerTileTag;

    // Variables for the player actions
    private PlayerActions playerActions;
    private Vector2 mouseLocation;

    // Variables for storing the tiles; x val = the index number, y val = weight of tile
    private Dictionary<Vector2, GameObject> playerTileDictionary = new Dictionary<Vector2, GameObject>();
    private Dictionary<Vector2, GameObject> enemyTileDictionary = new Dictionary<Vector2, GameObject>();

    private int baseTileWeight = 5;

    private void Awake()
    {
        // Creates a new PlayerActions
        playerActions = new PlayerActions();

        // Gets the current mouse position
        playerActions.MouseActions.MouseLocation.performed += ctx => mouseLocation = ctx.ReadValue<Vector2>();
        
        createGrids();
    }

    private void createGrids()
    {
        enemyGrid();
        playerGrid();
    }

    private void enemyGrid()
    {
        GameObject enemyGridContainer = GameObject.Find("Enemy_Grid_Container");
        int eTileCount = 1;

        RectTransform rt = tile.GetComponent<RectTransform>();
        // Generates a grid based on width and height for the enemy
        for (int i = 0; i < eWidth; i++)
        {
            for (int j = 0; j < eHeight; j++)
            {
                // Instantiates the tile object and renames it based on it's position, then adds it to the dictionary
                var currentTile = Instantiate(tile, GameObject.Find("Attack_Canvas").transform.Find("Enemy_Grid_Container").transform);
                currentTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(enemyGridStart.x + ((rt.rect.width + offset) * i), enemyGridStart.y + ((rt.rect.height + offset) * j));
                currentTile.name = $"EnemyTile({i},{j})";
                currentTile.tag = enemyTileTag;
                Tile tileScript = currentTile.GetComponent<Tile>();
                tileScript.init(true);
                tileScript.SetTileWeight(baseTileWeight);
                enemyTileDictionary.Add(new Vector2(eTileCount, tileScript.GetTileWeight()), currentTile);
                eTileCount++;
            }
        }
    }

    private void playerGrid()
    {
        GameObject playerGridContainer = GameObject.Find("Player_Grid_Container");
        int pTileCount = 1;

        RectTransform rt = tile.GetComponent<RectTransform>();
        // Generates a grid based on width and height for the player
        for (int i = 0; i < pWidth; i++)
        {
            for (int j = 0; j < pHeight; j++)
            {
                // Instantiates the tile object and renames it based on it's position, then adds it to the dictionary
                var currentTile = Instantiate(tile, GameObject.Find("Attack_Canvas").transform.Find("Player_Grid_Container").transform);
                currentTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(playerGridStart.x + ((rt.rect.width + offset) * i), playerGridStart.y + ((rt.rect.height + offset) * j));
                currentTile.name = $"PlayerTile({i},{j})";
                currentTile.tag = playerTileTag;
                Tile tileScript = currentTile.GetComponent<Tile>();
                tileScript.init(false);
                tileScript.SetTileWeight(baseTileWeight);
                playerTileDictionary.Add(new Vector2(pTileCount, tileScript.GetTileWeight()), currentTile);
                pTileCount++;
            }
        }
    }

    public void UpdatePlayerTileWeight()
    {
        //for
    }

    public void ResetPlayerTileWeight()
    {
        foreach (var tile in playerTileDictionary.Values)
        {
            tile.GetComponent<Tile>().SetTileWeight(baseTileWeight);
        }
    }

    // Gets the dictionary containing the enemy grid tiles
    public Dictionary<Vector2, GameObject> GetEnemyTileDictionary()
    {
        return enemyTileDictionary;
    }

    // Gets the dictionary containing the player grid tiles
    public Dictionary<Vector2, GameObject> GetPlayerTileDictionary()
    {
        return playerTileDictionary;
    }

    // Gets the enemy tile tag name
    public string GetEnemyTileTag()
    {
        return enemyTileTag;
    }

    // Gets the player tile tag name
    public string GetPlayerTag()
    {
        return playerTileTag;
    }

    // Gets player grid width
    public int GetPlayerGridWidth()
    {
        return pWidth;
    }

    // Gets player grid height
    public int GetPlayerGridHeight()
    {
        return pHeight;
    }

    // Gets enemy grid width
    public int GetEnemyGridWidth()
    {
        return eWidth;
    }


    // Gets enemy grid height
    public int GetEnemyGridHeight()
    {
        return eHeight;
    }

    public int GetBaseTileWeight()
    {
        return baseTileWeight;
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
