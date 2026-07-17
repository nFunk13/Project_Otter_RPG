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
    private List<MoveData> moves = new List<MoveData>();
    private List<MoveData> chosenMove = new List<MoveData>();
    private bool attackVisualized = false;
    private int tileAttackAddition;

    private List<ActionTypes> enemyActionTypes = new List<ActionTypes>();

    private float moveTime = 0.25f;
    private int tileKeyToMoveTo;
    private int enemyActionCount = 0;

    private float baseDecisionValue = 10000.0f;
    private float moveMultiplier;
    private float attackMultiplier;

    private void Awake()
    {
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();

        instanceEnemyData = Instantiate(baseEnemyData);

        instanceEnemyData.enemyCurrentHealth = instanceEnemyData.enemyMaxHealth;

        moves = instanceEnemyData.moveList;

        moveMultiplier = instanceEnemyData.attackRate;
        attackMultiplier = (1.0f - instanceEnemyData.attackRate);

        StartSpawn();
    }

    private void Start()
    {
        
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
        GraphBehavior.ChangeWeights(gridManager.FindTileKey(startSpawn, false), false, instanceEnemyData.weight ,instanceEnemyData.weightDecreaseValue);
        enemyTile = new KeyValuePair<int, GameObject>(randomNumber, startSpawn);
    }

    public float DetermineAttackWeightModifier()
    {
        KeyValuePair<MoveData, int> highestWeight = new KeyValuePair<MoveData, int>();
        foreach (var move in moves)
        {
            GraphBehavior.GetEnemyAttackWeight(move.centerTileKey, (16 - (move.rightMostTileKey - move.leftMostTileKey)), move.tileKeys, out int weight, out int addToTile);
            if (highestWeight.Key != move)
            {
                highestWeight = new KeyValuePair<MoveData, int>(move, weight);
                tileAttackAddition = addToTile;
            }
        }
        chosenMove.Add(highestWeight.Key);
        return (baseDecisionValue - (attackMultiplier * highestWeight.Value));
    }

    public float DetermineMoveWeightModifier()
    {
        KeyValuePair<int, int> wantedMove = new KeyValuePair<int, int>(0, 10000);

        gridManager.ResetEnemyTileWeight();
        foreach (var enemy in GameManager.GetInstance().GetEnemyList())
        {
            if (enemy != this.gameObject)
            {
                GraphBehavior.ChangeWeights(enemy.GetComponent<Enemy>().GetEnemyTileData().Key, false, instanceEnemyData.weight, instanceEnemyData.weightDecreaseValue);
            }
        }

        foreach (var enemy in GameManager.GetInstance().GetEnemyList())
        {
            if (enemy.gameObject != this.gameObject)
            {
                GraphBehavior.GetEnemyMoveWeight(this.enemyTile.Key, enemy.GetComponent<Enemy>().GetEnemyTileData().Key, out int tileKey, out int weight, this);
                if (wantedMove.Value > weight)
                {
                    wantedMove = new KeyValuePair<int, int>(tileKey, weight);
                }
            }
        }
        tileKeyToMoveTo = wantedMove.Key;
        return baseDecisionValue - (moveMultiplier * wantedMove.Value);
    }

    public void MoveEnemyOnGrid()
    {
        if (enemyActionCount > 0)
        {
            // Dictionary of enemy Tiles
            Dictionary<int, GameObject> enemyTiles = GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary();

            // Dictionary of potentialSpots for the enemy to move to
            //int enemyKey = enemyTile.Key;

            //// List of potential keys
            //List<int> potentialKeys = new List<int>();
            //potentialKeys.Add(enemyKey - gridManager.GetEnemyGridWidth());
            //potentialKeys.Add(enemyKey + gridManager.GetEnemyGridWidth());
            //potentialKeys.Add(enemyKey - 1);
            //potentialKeys.Add(enemyKey + 1);

            //// Checks to see if any of the potential keys are out of scope
            //for (int i = potentialKeys.Count - 1; i >= 0; i--)
            //{
            //    if (potentialKeys[i] < 1 || potentialKeys[i] > 16)
            //    {
            //        potentialKeys.Remove(potentialKeys[i]);
            //    }
            //}

            //// Checks to see if the current enemy tile is in the top or bottom row and removes the key forward or back one respectively
            //if (enemyTile.Key % gridManager.GetEnemyGridWidth() == 0)
            //{
            //    potentialKeys.Remove(enemyKey + 1);
            //}
            //else if (enemyTile.Key % gridManager.GetEnemyGridWidth() == 1)
            //{
            //    potentialKeys.Remove(enemyKey - 1);
            //}

            //// Picks a random potential key and tries to get the gameobject tied to it
            //int randomNumber = Random.Range(0, potentialKeys.Count);
            enemyTiles.TryGetValue(tileKeyToMoveTo, out GameObject desiredTile);
            
            // Moves the enemy object to the desired tile
            Vector3 endPosition = new Vector3(desiredTile.transform.position.x, desiredTile.gameObject.transform.position.y, -1.0f);
            transform.DOMove(endPosition, moveTime).SetUpdate(UpdateType.Fixed);
            enemyTile.Value.gameObject.GetComponent<Tile>().SetCharacterOn(false);
            enemyTile.Value.gameObject.GetComponent<Tile>().SetCharacterOnTile(null);
            desiredTile.GetComponent<Tile>().SetCharacterOn(true);
            desiredTile.GetComponent<Tile>().SetCharacterOnTile(this.gameObject);
            enemyTile = new KeyValuePair<int, GameObject>(tileKeyToMoveTo, desiredTile);
            tileKeyToMoveTo = 0;
        }
    }

    public void visualizeAttack()
    {
        if (enemyActionTypes.Count != 0 && enemyActionTypes[0] == GameManager.ActionTypes.ATTACK && !attackVisualized)
        {

            if (attackTiles.Count == 0)
            {
                //SetChosenMove();
                foreach (var key in chosenMove[0].tileKeys)
                {
                    GameObject tile = GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[key + tileAttackAddition];
                    attackTiles.Add(tile);
                }
            }

            foreach (var tile in attackTiles)
            {
                tile.gameObject.GetComponent<Image>().color = Color.orange;
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

    public KeyValuePair<int, GameObject> GetEnemyTileData()
    {
        return enemyTile;
    }
}
