using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PAttack : PlayerManager
{
    [SerializeField] List<MoveData> moves = new List<MoveData>();
    private List<GameObject> attackTiles = new List<GameObject>();

    private List<MoveData> chosenMove = new List<MoveData>();
    private int tileAddition = 0;

    public override void Init(PlayerSystems system)
    {
        base.Init(system);
        base.playerActions.Combat.AddTileAtk.performed += ChangeTileValue;
    }

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

    private void ChangeTileValue(InputAction.CallbackContext context)
    {
        if (GameManager.GetInstance().GetPlayerActionTypesList().Count > 0 && GameManager.GetInstance().GetPlayerActionTypesList()[0] == GameManager.ActionTypes.ATTACK && chosenMove.Count != 0)
        {
            MoveData atk = chosenMove[0];
            if (context.control.name == PlayerManager.InputKeyNames.upArrow.ToString() && (atk.rightMostTileKey + tileAddition) % 4 != 0)
            {
                tileAddition += 1;
            }
            else if (context.control.name == PlayerManager.InputKeyNames.downArrow.ToString() && chosenMove.Count != 0 && (((atk.leftMostTileKey + tileAddition) - 1) % 4) != 0)
            {
                tileAddition -= 1;
            }
            else if (context.control.name == PlayerManager.InputKeyNames.rightArrow.ToString() && chosenMove.Count != 0 && (atk.rightMostTileKey + tileAddition) <= (16 - 4))
            {
                tileAddition += 4;
            }
            else if (context.control.name == PlayerManager.InputKeyNames.leftArrow.ToString() && chosenMove.Count != 0 && (atk.leftMostTileKey + tileAddition) > 4)
            {
                tileAddition -= 4;
            }
        }
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
            //int keyAddition = 0; // Added value to the base tile index
            GridManager gridManager = GameManager.GetInstance().GetGridManager();
            GameObject testTile = gridManager.getTileAtPosition(gridManager.MouseToWorldPosition());

            // Gets the tiles based on the mouse's position
            if (chosenMove[0].tileKeys[0] >= 1 && tileAddition <= gridManager.GetEnemyTileDictionary().Count)
            {
                attackTiles.Clear();

                // Resets tile color to red
                GameManager.GetInstance().ResetEnemyGrid();

                // Sets the desired tiles to hotpink for visualization purposes
                foreach (var tileKey in gridManager.GetEnemyTileDictionary().Keys)
                {
                    foreach (var moveKey in chosenMove[0].tileKeys)
                    {

                        attackTiles.Add(gridManager.GetEnemyTileDictionary()[(moveKey + tileAddition)]);
                        gridManager.GetEnemyTileDictionary()[(moveKey + tileAddition)].gameObject.GetComponent<Image>().color = Color.hotPink;
                        continue;
                    }
                    break;
                }
            }
        }
    }

    public bool Attack()
    {
        Debug.Log("Chosen Move: " + chosenMove.FirstOrDefault().name);
        // Checks to make sure the tile is acceptable
        if (GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary().ContainsValue(attackTiles.FirstOrDefault()))
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
            tileAddition = 0;
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

    private void OnDisable()
    {
        base.playerActions.Disable();
    }
}
