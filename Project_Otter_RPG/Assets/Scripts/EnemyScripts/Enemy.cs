using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject baseEnemyData;
    private EnemyScriptableObject instanceEnemyData;

    private GridManager gridManager;

    private KeyValuePair<int, GameObject> enemyTile;

    private List<GameObject> attackTiles = new List<GameObject>();
    [SerializeField] private List<MoveData> moves = new List<MoveData>();
    private List<MoveData> chosenMove = new List<MoveData>();
    private bool attackVisualized = false;

    private List<ActionTypes> enemyActionTypes = new List<ActionTypes>();

    private float moveTime = 0.25f;
    private int enemyActionCount = 0;

    private void Awake()
    {
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();

        instanceEnemyData = Instantiate(baseEnemyData);

        instanceEnemyData.enemyCurrentHealth = instanceEnemyData.enemyMaxHealth;
    }

    private void Start()
    {
        StartSpawn();
    }

    // Places the enemy on a random spot on their grid
    private void StartSpawn()
    {
        GameObject startSpawn;
        int randomNumber;
        do
        {
            randomNumber = Random.Range(1, (gridManager.GetEnemyGridHeight() * gridManager.GetEnemyGridHeight()));
            startSpawn = GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary()[randomNumber];
        } while (startSpawn.GetComponent<Tile>().GetCharacterOn());
        this.gameObject.transform.position = new Vector3(startSpawn.transform.position.x, startSpawn.transform.position.y, -1.0f);
        startSpawn.GetComponent<Tile>().SetCharacterOn(true);
        startSpawn.GetComponent<Tile>().SetCharacterOnTile(this.gameObject);
        enemyTile = new KeyValuePair<int, GameObject>(randomNumber, startSpawn);
    }

    public void MoveEnemyOnGrid()
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
            enemyTile.Value.gameObject.GetComponent<Tile>().SetCharacterOn(false);
            enemyTile.Value.gameObject.GetComponent<Tile>().SetCharacterOnTile(null);
            desiredTile.GetComponent<Tile>().SetCharacterOn(true);
            desiredTile.GetComponent<Tile>().SetCharacterOnTile(this.gameObject);
            enemyTile = new KeyValuePair<int, GameObject>(potentialKeys[randomNumber], desiredTile);
        }
    }

    public void visualizeAttack()
    {
        if (enemyActionTypes.Count != 0 && enemyActionTypes[0] == GameManager.ActionTypes.ATTACK && !attackVisualized)
        {

            if (attackTiles.Count == 0)
            {
                SetChosenMove();
                foreach (var key in chosenMove[0].tileKeys)
                {
                    GameObject tile = GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[key];
                    attackTiles.Add(tile);
                }
            }

            // Sets up variables for setting the correct colors
            GridManager gridManager = GameManager.GetInstance().GetGridManager();
            int keyAddition = Random.Range(1, gridManager.GetPlayerTileDictionary().Count); // Added value to the base tile index

            if (keyAddition > gridManager.GetPlayerTileDictionary().Count)
            {
                keyAddition = gridManager.GetPlayerTileDictionary().Count;
            }
            if (keyAddition < chosenMove[0].centerTileKey)
            {
                keyAddition = -(chosenMove[0].centerTileKey - keyAddition);
            }
            if (keyAddition > 0)
            {
                keyAddition -= 1;
            }

            // Gets the tiles based on the mouse's position
            if (chosenMove[0].tileKeys[0] >= 1 && keyAddition <= gridManager.GetPlayerTileDictionary().Count)
            {
                attackTiles.Clear();

                // Resets tile color to red
                //GameManager.GetInstance().ResetPlayerGrid();

                // Sets the desired tiles to hotpink for visualization purposes
                foreach (var tileKey in gridManager.GetPlayerTileDictionary().Keys)
                {
                    foreach (var moveKey in chosenMove[0].tileKeys)
                    {
                        if (chosenMove[0].rightMostTileKey + keyAddition > 16)
                        {
                            keyAddition -= chosenMove[0].rightMostTileKey - chosenMove[0].centerTileKey;
                        }
                        else if (chosenMove[0].leftMostTileKey + keyAddition <= 0)
                        {
                            keyAddition = keyAddition - chosenMove[0].centerTileKey;
                        }
                        else if ((keyAddition + 1) % gridManager.GetPlayerGridWidth() == 0 && chosenMove[0].tileSpillage && keyAddition != 0)
                        {
                            keyAddition--;
                        }

                        attackTiles.Add(gridManager.GetPlayerTileDictionary()[(moveKey + keyAddition)]);
                        gridManager.GetPlayerTileDictionary()[(moveKey + keyAddition)].gameObject.GetComponent<Image>().color = Color.orange;
                        continue;
                    }
                    break;
                }
                attackVisualized = true;
            }
        }
    }

    public bool Attack()
    {
        bool hitPlayableCharacter = false;
        foreach (var tile in attackTiles)
        {
            if (tile.GetComponent<Tile>().GetCharacterOn())
            {
                tile.GetComponent<Tile>().GetCharacterOnTile().GetComponent<PlayerManager>().GetPlayableCharacterData().characterCurrentHealth -= chosenMove.FirstOrDefault().attackDamage;
                hitPlayableCharacter = true;
            }
        }
        attackTiles = new List<GameObject>();
        attackVisualized = false;
        chosenMove.RemoveAt(chosenMove.IndexOf(chosenMove.FirstOrDefault()));

        if (hitPlayableCharacter)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Death()
    {
        if (instanceEnemyData.enemyCurrentHealth <= 0)
        {
            enemyTile.Value.gameObject.GetComponent<Tile>().SetCharacterOn(false);
            enemyTile.Value.gameObject.GetComponent<Tile>().SetCharacterOnTile(null);
            Destroy(this.gameObject);
        }
    }

    public EnemyScriptableObject GetEnemyScriptableObject()
    {
        return instanceEnemyData;
    }

    // Gets the enemy action count
    public int GetEnemyActionCount()
    {
        return enemyActionCount;
    }

    // Sets the enemy action count
    public void SetEnemyActionCount(int actionCount)
    {
        enemyActionCount += actionCount;
    }

    private void SetChosenMove()
    {
        int moveKey = Random.Range(0, moves.Count - 1);
        chosenMove.Add(moves[moveKey]);
    }

    public void SetAttackVisualized(bool truthValue)
    {
        attackVisualized = truthValue;
    }

    public List<GameObject> GetAttackTiles()
    {
        return attackTiles;
    }

    public void SetEnemyActionTypes(GameManager.ActionTypes action)
    {
        enemyActionTypes.Add(action);
    }

    public List<GameManager.ActionTypes> GetEnemyActionTypes()
    {
        return enemyActionTypes;
    }
}
