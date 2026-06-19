using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PAttack : PlayerManager
{
    private KeyValuePair<int, GameObject> lastTile = new KeyValuePair<int, GameObject>();

    [SerializeField] List<MoveData> moves = new List<MoveData>();
    private List<GameObject> attackTiles = new List<GameObject>();

    private List<MoveData> chosenMove = new List<MoveData>();

    public override void Tick()
    {
        base.Tick();
        SeeAttackPattern();
        Death();
    }

    public MoveData ChosenMove(string nameOfMove)
    {
        foreach (var move in moves)
        {
            if (move.moveName == nameOfMove)
            {
                return move;
            }
        }
        return null;
    }

    public void SeeAttackPattern()
    {
        if (GameManager.GetInstance().GetPlayerActionTypesList().Count != 0 && GameManager.GetInstance().GetPlayerActionTypesList()[0] == GameManager.ActionTypes.ATTACK)
        {
            if (attackTiles.Count == 0)
            {
                foreach (var key in chosenMove[0].tileKeys)
                {
                    GameObject tile = GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary()[key];
                    tile.GetComponent<Image>().color = Color.hotPink;
                    attackTiles.Add(tile);
                }
            }

            // Sets up variables for setting the correct colors
            int keyAddition = 0; // Added value to the base tile index
            GridManager gridManager = GameManager.GetInstance().GetGridManager();
            GameObject testTile = gridManager.getTileAtPosition(gridManager.MouseToWorldPosition());

            // Gets the tiles based on the mouse's position
            if (chosenMove[0].tileKeys[0] >= 1 && keyAddition <= gridManager.GetEnemyTileDictionary().Count && testTile != lastTile.Value && testTile != null && testTile.gameObject.tag == gridManager.GetEnemyTileTag())
            {
                attackTiles.Clear();

                // Resets tile color to red
                GameManager.GetInstance().ResetEnemyGrid();

                // Sets the desired tiles to hotpink for visualization purposes
                foreach (var tileKey in gridManager.GetEnemyTileDictionary().Keys)
                {
                    bool firstTime = true; // Checks to see if the moveKey is the first one in the moves array
                    foreach (var moveKey in chosenMove[0].tileKeys)
                    {
                        bool keyModified = false;
                        keyAddition = gridManager.getTileKeyAtPosition(gridManager.MouseToWorldPosition());

                        if (keyAddition < chosenMove[0].centerTileKey)
                        {
                            keyAddition = 0;
                            keyModified = true;
                        }

                        if ((chosenMove[0].rightMostTileKey + (keyAddition - 1)) > gridManager.GetEnemyTileDictionary().Count)
                        {
                            keyAddition -= (chosenMove[0].rightMostTileKey - chosenMove[0].centerTileKey) + 1;
                            while (chosenMove[0].rightMostTileKey + keyAddition > 16)
                            {
                                keyAddition--;
                            } 
                            keyModified = true;
                        }

                        if (!keyModified)
                        {
                            keyAddition = keyAddition - chosenMove[0].centerTileKey;
                        }

                        if ((keyAddition + 1) % gridManager.GetEnemyGridWidth() == 0 && chosenMove[0].tileSpillage && keyAddition != 0)
                        {
                            keyAddition -= 1;
                        }

                        // Sets the lastTile key value pair
                        if (firstTime)
                        {
                            lastTile = new KeyValuePair<int, GameObject>((moveKey + keyAddition), gridManager.GetEnemyTileDictionary()[(moveKey + keyAddition)]);
                            firstTime = false;
                        }

                        attackTiles.Add(gridManager.GetEnemyTileDictionary()[(moveKey + keyAddition)]);
                        gridManager.GetEnemyTileDictionary()[(moveKey + keyAddition)].gameObject.GetComponent<Image>().color = Color.hotPink;
                        continue;
                    }
                    break;
                }
            }
        }
    }

    public bool Attack(GameObject attackTile)
    {
        Debug.Log("Chosen Move: " + chosenMove.FirstOrDefault().name);
        // Checks to make sure the tile is acceptable
        if (GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary().ContainsValue(attackTile))
        {
            foreach (var tile in attackTiles)
            {
                if (tile.GetComponent<Tile>().GetCharacterOn())
                {
                    tile.GetComponent<Tile>().GetCharacterOnTile().GetComponent<Enemy>().GetEnemyScriptableObject().enemyCurrentHealth -= chosenMove.FirstOrDefault().attackDamage;
                    attackTiles = new List<GameObject>();
                }
            }
            chosenMove.RemoveAt(chosenMove.IndexOf(chosenMove.FirstOrDefault()));

            return true;
        }

        return false;
    }

    public void Death()
    {
        if (this.gameObject.GetComponent<PlayerManager>().GetPlayableCharacterData().characterCurrentHealth <= 0)
        {
            SceneManager.LoadScene("EndScene");
        }
    }

    public void SetChosenMoveData(MoveData move)
    {
        chosenMove.Add(move);
    }

    public List<MoveData> GetMoves()
    {
        return moves;
    }
}
