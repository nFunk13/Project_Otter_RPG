using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PAttack : PlayerManager
{
    private Dictionary<int, GameObject> enemyTileDictionary = new Dictionary<int, GameObject>();

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
        if (moves[0].tileKeys[0] >= 0 && moves[0].tileKeys[moves[0].tileKeys.Count - 1] < (enemyTileDictionary.Count - 1))
        {
            int keyAddition = GameManager.GetInstance().GetGridManager().getTileKeyAtPosition(GameManager.GetInstance().GetGridManager().MouseToWorldPosition());
            foreach (var tileKey in enemyTileDictionary.Keys)
            {
                foreach (var moveKey in moves[0].tileKeys)
                {
                    if (tileKey == moveKey + keyAddition && keyAddition >= 0)
                    {
                        enemyTileDictionary[tileKey].gameObject.GetComponent<SpriteRenderer>().color = Color.hotPink;
                        continue;
                    }
                    else
                    {
                        enemyTileDictionary[tileKey].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
            }
        }
    }
}
