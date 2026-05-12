using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PAttack : PlayerManager
{
    private KeyValuePair<int, GameObject> lastTile = new KeyValuePair<int, GameObject>();

    [SerializeField] List<MoveData> moves = new List<MoveData>();

    public override void Tick()
    {
        base.Tick();
        SeeAttackPattern();
    }

    private void SeeAttackPattern()
    {
        GridManager gridManager = GameManager.GetInstance().GetGridManager();
        int keyAddition = gridManager.getTileKeyAtPosition(GameManager.GetInstance().GetGridManager().MouseToWorldPosition());
        if (keyAddition > gridManager.GetPlayerTileDictionary().Count)
        {
            keyAddition = gridManager.GetPlayerTileDictionary().Count;
        }
        if (keyAddition > 0)
        {
            keyAddition -= 1;
        }
        int backOne = 0;

        foreach (GameObject tile in gridManager.GetEnemyTileDictionary().Values)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (moves[0].tileKeys[0] >= 1 && keyAddition <= gridManager.GetEnemyTileDictionary().Count)
        {
            foreach (var tileKey in gridManager.GetEnemyTileDictionary().Keys)
            {
                foreach (var moveKey in moves[0].tileKeys)
                {
                    if ((keyAddition + 1) % GameManager.GetInstance().GetGridManager().GetEnemyGridWidth() == 0 && keyAddition > 0)
                    {
                        backOne = -1;
                    }

                    gridManager.GetEnemyTileDictionary()[(moveKey + keyAddition) + backOne].gameObject.GetComponent<SpriteRenderer>().color = Color.hotPink;
                    lastTile = new KeyValuePair<int, GameObject>((moveKey + keyAddition) + backOne, gridManager.GetEnemyTileDictionary()[(moveKey + keyAddition) + backOne]);
                    continue;
                }
                //gridManager.GetEnemyTileDictionary()[tileKey].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
}
