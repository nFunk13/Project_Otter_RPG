using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    // Variables for creating enemy grid
    [SerializeField] int eWidth = 3, eHeight = 3;
    [SerializeField] Vector2 enemyGridStart = new Vector2(3.0f, 3.0f);
    [SerializeField] float offset = 1.0f;

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

    //int playerActionCount = 0;

    // Variables for storing the tiles
    private Dictionary<int, GameObject> playerTileDictionary = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> enemyTileDictionary = new Dictionary<int, GameObject>();

    private void Awake()
    {
        // Creates a new PlayerActions
        playerActions = new PlayerActions();

        // Gets the current mouse position
        playerActions.MouseActions.MouseLocation.performed += ctx => mouseLocation = ctx.ReadValue<Vector2>();
        playerActions.TestActions.PlayerHealthTest.performed += emty => DamagePlayer();

        createGrids();
    }

    private void createGrids()
    {
        enemyGrid();
        playerGrid();
    }

    private void enemyGrid()
    {
        GameObject enemyGridContainer = new GameObject("Enemy_Grid");
        int eTileCount = 1;
        // Generates a grid based on width and height for the enemy
        for (int i = 0; i < eWidth; i++)
        {
            for (int j = 0; j < eHeight; j++)
            {
                // Instantiates the tile object and renames it based on it's position, then adds it to the dictionary
                Vector2 currentPos = new Vector2(enemyGridStart.x + ((tile.GetComponent<SpriteRenderer>().bounds.size.x + offset) * i), enemyGridStart.y + ((tile.GetComponent<SpriteRenderer>().bounds.size.y + offset) * j));
                var currentTile = Instantiate(tile, currentPos, Quaternion.identity);
                currentTile.name = $"EnemyTile({i},{j})";
                currentTile.transform.parent = enemyGridContainer.transform;
                currentTile.tag = enemyTileTag;
                currentTile.GetComponent<Tile>().init(true);
                enemyTileDictionary.Add(eTileCount, currentTile);
                eTileCount++;
            }
        }
    }

    private void playerGrid()
    {
        GameObject playerGridContainer = new GameObject("Player_Grid");
        int pTileCount = 1;
        // Generates a grid based on width and height for the player
        for (int i = 0; i < pWidth; i++)
        {
            for (int j = 0; j < pHeight; j++)
            {
                // Instantiates the tile object and renames it based on it's position, then adds it to the dictionary
                Vector2 currentPos = new Vector2(playerGridStart.x + ((tile.GetComponent<SpriteRenderer>().bounds.size.x + offset) * i), playerGridStart.y + ((tile.GetComponent<SpriteRenderer>().bounds.size.y + offset) * j));
                var currentTile = Instantiate(tile, currentPos, Quaternion.identity);
                currentTile.name = $"PlayerTile({i},{j})";
                currentTile.transform.parent = playerGridContainer.transform;
                currentTile.tag = playerTileTag;
                currentTile.GetComponent<Tile>().init(false);
                playerTileDictionary.Add(pTileCount, currentTile);
                pTileCount++;
            }
        }
    }

    public Vector3 MouseToWorldPosition()
    {
        // Converst he mouse's position to it's world position value
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseLocation);

        return worldPosition;
    }

    public GameObject getTileAtPosition(Vector3 pos)
    {
        // Takes the mouse position and sets the z component to zero
        Vector3 newPos = pos;
        newPos.z = 0.0f;

        // Takes the position to be a RaycastHit2D, with the direction going to the right
        RaycastHit2D hit = Physics2D.Raycast(newPos, Vector2.right, 1.0f);
        
        // Checks each tile in the dictionary
        foreach (var tile in enemyTileDictionary.Values)
        {
            // Checks if the hit collider has something and if the game objects are the same
            if (hit.collider != null && hit.collider.gameObject == tile.gameObject)
            {
                return tile;
            }
        }

        foreach (var tile in playerTileDictionary.Values)
        {
            // Checks if the hit collider has something and if the game objects are the same
            if (hit.collider != null && hit.collider.gameObject == tile.gameObject)
            {
                return tile;
            }
        }

        // returns nothing otherwise
        return null;
    }

    // Gets the key of the tile
    public int getTileKeyAtPosition(Vector3 pos)
    {
        // Takes the mouse position and sets the z component to zero
        Vector3 newPos = pos;
        newPos.z = 0.0f;

        // Takes the position to be a RaycastHit2D, with the direction going to the right
        RaycastHit2D hit = Physics2D.Raycast(newPos, Vector2.right, 1.0f);

        // Checks each tile in the dictionary
        foreach (var tile in enemyTileDictionary)
        {
            // Checks if the hit collider has something and if the game objects are the same
            if (hit.collider != null && hit.collider.gameObject == tile.Value.gameObject)
            {
                // Returns the key of the tile
                return tile.Key;
            }
        }

        foreach (var tile in playerTileDictionary)
        {
            // Checks if the hit collider has something and if the game objects are the same
            if (hit.collider != null && hit.collider.gameObject == tile.Value.gameObject)
            {
                // Returns the key of the tile
                return tile.Key;
            }
        }

        // returns nothing otherwise
        return 0;
    }

    // Decreases player health by a certain amount
    private void DamagePlayer()
    {
        PHealth playerHealth = GameObject.Find("Player").GetComponent<PHealth>();
        playerHealth.OnDamage(3);
    }

    // Gets the dictionary containing the enemy grid tiles
    public Dictionary<int, GameObject> GetEnemyTileDictionary()
    {
        return enemyTileDictionary;
    }

    // Gets the dictionary containing the player grid tiles
    public Dictionary<int, GameObject> GetPlayerTileDictionary()
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

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
}
