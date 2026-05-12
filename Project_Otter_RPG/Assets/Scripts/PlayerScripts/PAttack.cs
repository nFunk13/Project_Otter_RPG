using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PAttack : PlayerManager
{
    private Dictionary<int, GameObject> enemyTileDictionary = new Dictionary<int, GameObject>();
    private KeyValuePair<int, GameObject> lastTile = new KeyValuePair<int, GameObject>();

    [SerializeField] List<MoveData> moves = new List<MoveData>();

    private void Start()
    {
        PopulateTiles();
    }

    private void PopulateTiles()
    {
        foreach (var tile in GameManager.GetInstance().GetGridManager().GetTileDictionary())
        {
            if (tile.Value.gameObject.tag == GameManager.GetInstance().GetGridManager().GetEnemyTileTag())
            {
                enemyTileDictionary.Add(tile.Key, tile.Value.gameObject);
            }
        }
    }

    public override void Tick()
    {
        base.Tick();
        SeeAttackPattern();
    }

    private void SeeAttackPattern()
    {
        int keyAddition = GameManager.GetInstance().GetGridManager().getTileKeyAtPosition(GameManager.GetInstance().GetGridManager().MouseToWorldPosition());
        if (keyAddition > 15)
        {
            keyAddition = 15;
        }
        else if (keyAddition < 0)
        {
            keyAddition = 0;
        }
        int backOne = 0;

        if (moves[0].tileKeys[0] >= 0 && moves[0].tileKeys[moves[0].tileKeys.Count - 1] < (enemyTileDictionary.Count - 1) && keyAddition < 16)
        {
            foreach (var tileKey in enemyTileDictionary.Keys)
            {
                foreach (var moveKey in moves[0].tileKeys)
                {
                    if ((keyAddition + 1) % GameManager.GetInstance().GetGridManager().GetEnemyGridWidth() == 0 && keyAddition > 0)
                    {
                        backOne = -1;
                    }
                    enemyTileDictionary[(moveKey + keyAddition) + backOne].gameObject.GetComponent<SpriteRenderer>().color = Color.hotPink;
                    continue;
                }
                enemyTileDictionary[tileKey].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                lastTile = new KeyValuePair<int, GameObject>(keyAddition, enemyTileDictionary[keyAddition]);
            }
        }
        else
        {
            foreach (var moveKey in moves[0].tileKeys)
            {
                enemyTileDictionary[(moveKey + lastTile.Key) - backOne].gameObject.GetComponent<SpriteRenderer>().color = Color.hotPink;
            }
        }
    }
}
