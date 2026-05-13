using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PAttack : PlayerManager
{
    private KeyValuePair<int, GameObject> lastTile = new KeyValuePair<int, GameObject>();

    [SerializeField] List<MoveData> moves = new List<MoveData>();
    private List<GameObject> attackTiles = new List<GameObject>();

    public override void Tick()
    {
        base.Tick();
        SeeAttackPattern();
    }

    private void SeeAttackPattern()
    {
        // Sets up variables for setting the correct colors
        int keyAddition = 0; // Added value to the base tile index
        GridManager gridManager = GameManager.GetInstance().GetGridManager();
        GameObject testTile = gridManager.getTileAtPosition(gridManager.MouseToWorldPosition());

        // Gets the number to add to the move tile keys and makes checks
        if (testTile != null && testTile.tag == gridManager.GetEnemyTileTag())
        {
            keyAddition = gridManager.getTileKeyAtPosition(gridManager.MouseToWorldPosition());
        }
        if (keyAddition > gridManager.GetPlayerTileDictionary().Count)
        {
            keyAddition = gridManager.GetPlayerTileDictionary().Count;
        }
        if (keyAddition > 0)
        {
            keyAddition -= 1;
        }
        int backOne = 0; // Variable to move back one

        // Gets the tiles based on the mouse's position
        if (moves[0].tileKeys[0] >= 1 && keyAddition <= gridManager.GetEnemyTileDictionary().Count && testTile != lastTile.Value && testTile != null)
        {
            attackTiles.Clear();
            
            // Resets tile color to red
            foreach (GameObject tile in gridManager.GetEnemyTileDictionary().Values)
            {
                tile.GetComponent<SpriteRenderer>().color = Color.red;
            }

            // Sets the desired tiles to hotpink for visualization purposes
            foreach (var tileKey in gridManager.GetEnemyTileDictionary().Keys)
            {
                bool firstTime = true; // Checks to see if the moveKey is the first one in the moves array
                foreach (var moveKey in moves[0].tileKeys)
                {
                    // Sets the lastTile key value pair
                    if (firstTime)
                    {
                        lastTile = new KeyValuePair<int, GameObject>((moveKey + keyAddition) + backOne, gridManager.GetEnemyTileDictionary()[(moveKey + keyAddition) + backOne]);
                        firstTime = false;
                    }
                    if ((keyAddition + 1) % GameManager.GetInstance().GetGridManager().GetEnemyGridWidth() == 0 && keyAddition > 0)
                    {
                        backOne = -1;
                    }

                    attackTiles.Add(gridManager.GetEnemyTileDictionary()[(moveKey + keyAddition) + backOne]);
                    gridManager.GetEnemyTileDictionary()[(moveKey + keyAddition) + backOne].gameObject.GetComponent<SpriteRenderer>().color = Color.hotPink;
                    continue;
                }
            }
        }
    }
}
