using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject enemyTile;
    [SerializeField] int width = 3, height = 3;
    [SerializeField] Vector2 enemyGridStart = new Vector2(3.0f, 3.0f);
    [SerializeField] float offset = 1.0f;

    private void Start()
    {
        createGrid();
    }

    private void createGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Instantiate(enemyTile, new Vector2(enemyGridStart.x + ((enemyTile.GetComponent<SpriteRenderer>().bounds.size.x + offset) * i), enemyGridStart.y + ((enemyTile.GetComponent<SpriteRenderer>().bounds.size.y + offset) * j)), Quaternion.identity);
            }
        }
    }
}
