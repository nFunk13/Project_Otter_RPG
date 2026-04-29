using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int width = 3, height = 3;
    [SerializeField] Vector2 enemyGridStart = new Vector2(3.0f, 3.0f);
    [SerializeField] float offset = 1.0f;
    [SerializeField] Tile tile;

    private Dictionary<Vector2, Tile> tileDictionary = new Dictionary<Vector2, Tile>();

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

    private void Update()
    {
        getTileAtPosition(MouseToWorldPosition());
    }

    private Vector3 MouseToWorldPosition()
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = 10f;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        return worldPosition;
    }

    private Tile getTileAtPosition(Vector3 pos)
    {
        if (tileDictionary.ContainsKey(pos))
        {
            Debug.Log("Tile name: " + tileDictionary[pos]);
            return tileDictionary[pos];
        }
        else
        {
            Debug.Log("No Tile!");
            return null;
        }
    }
}
