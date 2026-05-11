using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject enemyScriptableObject;

    private GridManager gridManager;

    private Dictionary<int, GameObject> enemyTileDictionary = new Dictionary<int, GameObject>();
    private KeyValuePair<int, GameObject> enemyTile;

    private bool canMove = false;
    private float moveTime = 0.25f;

    private void Awake()
    {
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();

    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        MoveEnemyOnGrid();
    }

    private void StartGame()
    {
        PopulateTiles();
        startSpawn();
        StartCoroutine(DelayMove(5.0f));
    }

    private void PopulateTiles()
    {
        foreach (var tile in gridManager.GetTileDictionary())
        {
            if (tile.Value.gameObject.tag == gridManager.GetEnemyTileTag())
            {
                enemyTileDictionary.Add(tile.Key, tile.Value.gameObject);
            }
        }
    }

    private void startSpawn()
    {
        int randomNumber = Random.Range(0, (gridManager.GetEnemyGridHeight() * gridManager.GetEnemyGridHeight()) - 1);
        GameObject startSpawn = enemyTileDictionary[randomNumber];
        this.gameObject.transform.position = new Vector3(startSpawn.transform.position.x, startSpawn.transform.position.y, -1.0f);
        enemyTile = new KeyValuePair<int, GameObject>(randomNumber, startSpawn);
    }

    private void MoveEnemyOnGrid()
    {
        if (canMove)
        {
            canMove = false;
            Dictionary<int, GameObject> potentialSpots = new Dictionary<int, GameObject>();
            int enemyKey = enemyTile.Key;

            List<int> potentialKeys = new List<int>();
            potentialKeys.Add(enemyKey - gridManager.GetEnemyGridWidth());
            potentialKeys.Add(enemyKey + gridManager.GetEnemyGridWidth());
            potentialKeys.Add(enemyKey - 1);
            potentialKeys.Add(enemyKey + 1);

            for (int i = potentialKeys.Count - 1; i >= 0; i--)
            {
                if (potentialKeys[i] < 0)
                {
                    potentialKeys.Remove(potentialKeys[i]);
                }
            }

            foreach (var potentialKey in potentialKeys)
            {
                if (enemyTileDictionary.TryGetValue(potentialKey, out GameObject tileObject))
                {
                    potentialSpots.Add(potentialKey, tileObject);
                }
            }

            if (enemyTile.Key % gridManager.GetEnemyGridWidth() == 0)
            {
                potentialSpots.Remove(enemyKey - 1);
                potentialKeys.Remove(enemyKey - 1);
            }
            else if (enemyTile.Key % gridManager.GetEnemyGridWidth() == gridManager.GetEnemyGridWidth() - 1)
            {
                potentialSpots.Remove(enemyKey + 1);
                potentialKeys.Remove(enemyKey + 1);
            }

            int randomNumber = Random.Range(0, potentialSpots.Count);
            potentialSpots.TryGetValue(potentialKeys[randomNumber], out GameObject desiredTile);
            
            Vector3 endPosition = new Vector3(desiredTile.transform.position.x, desiredTile.gameObject.transform.position.y, -1.0f);
            transform.DOMove(endPosition, moveTime);
            enemyTile = new KeyValuePair<int, GameObject>(potentialKeys[randomNumber], desiredTile);
            StartCoroutine(DelayMove(2.0f));
        }
    }

    private IEnumerator DelayMove(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
        Debug.Log("CAN MOVE NOW");
    }
}
