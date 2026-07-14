using System.Collections.Generic;
using UnityEngine;

public class GraphBehavior : MonoBehaviour
{
    private static Graph playerTileConnection;
    private static Graph enemyTileConnection;

    private static void ConnectEnemyTiles()
    {
        enemyTileConnection = new Graph();
        Dictionary<int, GameObject> enemyDictionary = GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary();
        foreach (var obj in enemyDictionary)
        {
            if (enemyDictionary.ContainsKey(obj.Key - 4))
            {
                enemyTileConnection.AddEdge(obj.Key, obj.Key - 4);
            }
            if (enemyDictionary.ContainsKey(obj.Key - 1))
            {
                enemyTileConnection.AddEdge(obj.Key, obj.Key - 1);
            }
            if (enemyDictionary.ContainsKey(obj.Key + 1))
            {
                enemyTileConnection.AddEdge(obj.Key, obj.Key + 1);
            }
            if (enemyDictionary.ContainsKey(obj.Key + 4))
            {
                enemyTileConnection.AddEdge(obj.Key, obj.Key + 4);
            }
        }
    }

    private static void ConnectPlayerTiles()
    {
        playerTileConnection = new Graph();
        Dictionary<int, GameObject> enemyDictionary = GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary();
        foreach (var obj in enemyDictionary)
        {
            if (enemyDictionary.ContainsKey(obj.Key - 4))
            {
                playerTileConnection.AddEdge(obj.Key, obj.Key - 4);
            }
            if (enemyDictionary.ContainsKey(obj.Key - 1) && ((obj.Key - 1) % 4 != 0))
            {
                playerTileConnection.AddEdge(obj.Key, obj.Key - 1);
            }
            if (enemyDictionary.ContainsKey(obj.Key + 1) && (obj.Key % 4 != 0))
            {
                playerTileConnection.AddEdge(obj.Key, obj.Key + 1);
            }
            if (enemyDictionary.ContainsKey(obj.Key + 4))
            {
                playerTileConnection.AddEdge(obj.Key, obj.Key + 4);
            }
        }
    }

    //public static List<int> GetPlayerGridPath(int start, int end)
    //{
    //    if (enemyTileConnection == null && enemyTileConnection == null)
    //    {
    //        ConnectEnemyTiles();
    //        ConnectPlayerTiles();
    //    }
    //    return playerTileConnection.BFS(start, end);
    //}

    public static void ChangePlayerTileWeights(int start)
    {
        if (enemyTileConnection == null && enemyTileConnection == null)
        {
            ConnectEnemyTiles();
            ConnectPlayerTiles();
        }
        playerTileConnection.ChangePlayerTileWeights(start);
    }

}
