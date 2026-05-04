using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    // Variables for creating enemy grid
    [SerializeField] int width = 3, height = 3;
    [SerializeField] Vector2 enemyGridStart = new Vector2(3.0f, 3.0f);
    [SerializeField] float offset = 1.0f;
    
    // Variables for the tiles themselves
    [SerializeField] Tile tile;
    [SerializeField] LayerMask mask;

    // Variables for the player actions
    private PlayerActions playerActions;
    private Vector2 mouseLocation;

    // Variables for storing the tiles
    private Dictionary<Vector2, Tile> tileDictionary = new Dictionary<Vector2, Tile>();

    private void Awake()
    {
        // Creates a new PlayerActions
        playerActions = new PlayerActions();

        // Gets the current mouse position
        playerActions.MouseActions.MouseLocation.performed += ctx => mouseLocation = ctx.ReadValue<Vector2>();
    }

    private void Start()
    {
        createEnemyGrid();
    }

    private void createEnemyGrid()
    {
        // Generates a grid based on width and height
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                // Instantiates the tile object and renames it based on it's position, then adds it to the dictionary
                var currentTile = Instantiate(tile, new Vector2(enemyGridStart.x + ((tile.GetComponent<SpriteRenderer>().bounds.size.x + offset) * i), enemyGridStart.y + ((tile.GetComponent<SpriteRenderer>().bounds.size.y + offset) * j)), Quaternion.identity);
                currentTile.name = $"EnemyTile({i},{j})";
                currentTile.init(true);
                tileDictionary.Add(new Vector2(i, j), currentTile);
            }
        }
    }

    private void FixedUpdate()
    {
        getTileAtPosition(MouseToWorldPosition());
    }

    private Vector3 MouseToWorldPosition()
    {
        // Converst he mouse's position to it's world position value
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseLocation);

        return worldPosition;
    }

    private Tile getTileAtPosition(Vector3 pos)
    {
        // Takes the mouse position and sets the z component to zero
        Vector3 newPos = pos;
        newPos.z = 0.0f;

        // Takes the position to be a RaycastHit2D, with the direction going to the right
        RaycastHit2D hit = Physics2D.Raycast(newPos, Vector2.right, 1.0f);
        
        // Checks each tile in the dictionary
        foreach (var tile in tileDictionary.Values)
        {
            // Checks if the hit collider has something and if the game objects are the same
            if (hit.collider != null && hit.collider.gameObject == tile.gameObject)
            {
                Debug.Log("Tile: " + tile.name);
                return tile;
            }
        }

        // returns nothing otherwise
        return null;
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
