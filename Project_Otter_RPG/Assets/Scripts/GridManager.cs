using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    [SerializeField] int width = 3, height = 3;
    [SerializeField] Vector2 enemyGridStart = new Vector2(3.0f, 3.0f);
    [SerializeField] float offset = 1.0f;
    
    [SerializeField] Tile tile;
    [SerializeField] LayerMask mask;

    private PlayerActions playerActions;
    private Vector2 mouseLocation;

    private Dictionary<Vector2, Tile> tileDictionary = new Dictionary<Vector2, Tile>();

    private void Awake()
    {
        playerActions = new PlayerActions();

        playerActions.MouseActions.MouseLocation.performed += ctx => mouseLocation = ctx.ReadValue<Vector2>();
    }

    private void onMouseMove(InputAction.CallbackContext ctx)
    {
        mouseLocation = ctx.ReadValue<Vector2>();
    }

    private void Start()
    {
        createEnemyGrid();
    }

    private void createEnemyGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
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
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseLocation);

        return worldPosition;
    }

    private Tile getTileAtPosition(Vector3 pos)
    {
        Vector3 newPos = pos;
        newPos.z = 0.0f;
        RaycastHit2D hit = Physics2D.Raycast(newPos, Vector2.right, 1.0f);
        
        foreach (var tile in tileDictionary.Values)
        {
            if (hit.collider != null && hit.collider.gameObject == tile.gameObject)
            {
                Debug.Log("Tile: " + tile.name);
                return tile;
            }
        }

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
