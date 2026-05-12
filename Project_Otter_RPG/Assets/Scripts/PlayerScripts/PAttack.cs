using System.Collections.Generic;
using UnityEngine;

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
}
