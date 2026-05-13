using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject enemyScriptableObject;

    private GridManager gridManager;

    private KeyValuePair<int, GameObject> enemyTile;

    private float moveTime = 0.25f;
    private int enemyActionCount = 0;

    private void Awake()
    {
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();

    }

    private void Start()
    {
        StartSpawn();
    }

    private void Update()
    {
        MoveEnemyOnGrid();
    }

    // Places the enemy on a random spot on their grid
    private void StartSpawn()
    {
        int randomNumber = Random.Range(1, (gridManager.GetEnemyGridHeight() * gridManager.GetEnemyGridHeight()));
        GameObject startSpawn = GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary()[randomNumber];
        this.gameObject.transform.position = new Vector3(startSpawn.transform.position.x, startSpawn.transform.position.y, -1.0f);
        enemyTile = new KeyValuePair<int, GameObject>(randomNumber, startSpawn);
    }

    private void MoveEnemyOnGrid()
    {
        if (enemyActionCount > 0)
        {
            // Dictionary of enemy Tiles
            Dictionary<int, GameObject> enemyTiles = GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary();

            // Dictionary of potentialSpots for the enemy to move to
            int enemyKey = enemyTile.Key;

            // List of potential keys
            List<int> potentialKeys = new List<int>();
            potentialKeys.Add(enemyKey - gridManager.GetEnemyGridWidth());
            potentialKeys.Add(enemyKey + gridManager.GetEnemyGridWidth());
            potentialKeys.Add(enemyKey - 1);
            potentialKeys.Add(enemyKey + 1);

            // Checks to see if any of the potential keys are out of scope
            for (int i = potentialKeys.Count - 1; i >= 0; i--)
            {
                if (potentialKeys[i] < 1 || potentialKeys[i] > 16)
                {
                    potentialKeys.Remove(potentialKeys[i]);
                }
            }

            // Checks to see if the current enemy tile is in the top or bottom row and removes the key forward or back one respectively
            if (enemyTile.Key % gridManager.GetEnemyGridWidth() == 0)
            {
                potentialKeys.Remove(enemyKey + 1);
            }
            else if (enemyTile.Key % gridManager.GetEnemyGridWidth() == 1)
            {
                potentialKeys.Remove(enemyKey - 1);
            }

            // Picks a random potential key and tries to get the gameobject tied to it
            int randomNumber = Random.Range(0, potentialKeys.Count);
            enemyTiles.TryGetValue(potentialKeys[randomNumber], out GameObject desiredTile);
            
            // Moves the enemy object to the desired tile
            Vector3 endPosition = new Vector3(desiredTile.transform.position.x, desiredTile.gameObject.transform.position.y, -1.0f);
            transform.DOMove(endPosition, moveTime).SetUpdate(UpdateType.Fixed);
            enemyTile = new KeyValuePair<int, GameObject>(potentialKeys[randomNumber], desiredTile);
            enemyActionCount--;
        }
    }

    // Gets the enemy action count
    public int GetEnemyActionCount()
    {
        return enemyActionCount;
    }

    // Sets the enemy action count
    public void SetEnemyActionCount(int actionCount)
    {
        enemyActionCount = actionCount;
    }
}
