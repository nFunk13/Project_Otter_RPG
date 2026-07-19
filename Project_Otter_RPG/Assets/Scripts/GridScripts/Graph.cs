using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Rendering;

public class Graph
{
    private Dictionary<int, List<int>> adjacencyList;

    public Graph()
    {
        adjacencyList = new Dictionary<int, List<int>>();
    }

    public void AddEdge(int vertex1, int vertex2)
    {
        if (!adjacencyList.ContainsKey(vertex1))
        {
            adjacencyList.Add(vertex1, new List<int>());
        }
        if (!adjacencyList.ContainsKey(vertex2))
        {
            adjacencyList.Add(vertex2, new List<int>());
        }

        adjacencyList[vertex1].Add(vertex2);
    }

    public List<int> BFS(int startVertex, int goalVertex)
    {
        Dictionary<int, int> visited = new Dictionary<int, int>();
        visited[startVertex] = startVertex;
        List<int> paths = new List<int>();
        Queue<int> frontier = new Queue<int>();
        frontier.Enqueue(startVertex);

        while (frontier.Count > 0)
        {
            int current = frontier.Dequeue();

            if (current == goalVertex)
            {
                return pathDicToList(ref visited, ref goalVertex);
            }

            var neighbors = adjacencyList.ContainsKey(current) ? adjacencyList[current] : new List<int>();

            foreach (var neighbor in neighbors)
            {
                if (visited.ContainsKey(neighbor))
                {
                    continue;
                }
                else
                {
                    frontier.Enqueue(neighbor);
                    visited[neighbor] = current;
                }
            }
        }

        return null;
    }

    public void BFS(int startVertex, int goalVertex, out int tileKeyWant, out int weightWant, Enemy enemy)
    {
        Dictionary<int, int> visited = new Dictionary<int, int>();
        visited[startVertex] = startVertex;
        List<int> paths = new List<int>();
        Queue<int> frontier = new Queue<int>();
        frontier.Enqueue(startVertex);

        tileKeyWant = 0;
        weightWant = 0;

        while (frontier.Count > 0)
        {
            int current = frontier.Dequeue();

            if (current == goalVertex)
            {
                pathDicToList(ref visited, ref goalVertex, out int tileKey, out int weight, enemy);
                tileKeyWant = tileKey;
                weightWant = weight;
            }

            var neighbors = adjacencyList.ContainsKey(current) ? adjacencyList[current] : new List<int>();

            foreach (var neighbor in neighbors)
            {
                if (visited.ContainsKey(neighbor))
                {
                    continue;
                }
                else
                {
                    frontier.Enqueue(neighbor);
                    visited[neighbor] = current;
                }
            }
        }
    }

    public void ChangeTileWeights(int startInt, bool playerGraph, int decreaseWeight)
    {
        Dictionary<int, int> visited = new Dictionary<int, int>();
        visited[startInt] = startInt;
        Queue<int> frontier = new Queue<int>();
        frontier.Enqueue(startInt);
        int weightValue = 5;

        while (frontier.Count > 0)
        {
            int current = frontier.Dequeue();

            if (playerGraph)
            {
                GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[current].GetComponent<Tile>().AddTileWeight(weightValue * 2);
            }
            else
            {
                GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary()[current].GetComponent<Tile>().AddTileWeight(weightValue * 2);
            }

            var neighbors = adjacencyList.ContainsKey(current) ? adjacencyList[current] : new List<int>();

            foreach (var neighbor in neighbors)
            {
                if (visited.ContainsKey(neighbor))
                {
                    continue;
                }
                else
                {
                    frontier.Enqueue(neighbor);
                    visited[neighbor] = current;
                }
            }
            ChangeWeightHelpFunction(weightValue, ref frontier, ref visited, playerGraph, decreaseWeight);
        }
        Debug.Log("VISITED COUNT: " + visited.Count);
    }

    public void ChangeTileWeights(int startInt, bool playerGraph, int desiredWeight, int decreaseWeight, ref int moveTile)
    {
        Dictionary<int, int> visited = new Dictionary<int, int>();
        visited[startInt] = startInt;
        Queue<int> frontier = new Queue<int>();
        frontier.Enqueue(startInt);
        int weightValue = desiredWeight;

        while (frontier.Count > 0)
        {
            int current = frontier.Dequeue();

            if (playerGraph)
            {
                GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[current].GetComponent<Tile>().AddTileWeight(weightValue * 2);
            }
            else
            {
                GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary()[current].GetComponent<Tile>().AddTileWeight(weightValue * 2);
            }

            moveTile = current;

            var neighbors = adjacencyList.ContainsKey(current) ? adjacencyList[current] : new List<int>();

            foreach (var neighbor in neighbors)
            {
                if (visited.ContainsKey(neighbor))
                {
                    continue;
                }
                else
                {
                    frontier.Enqueue(neighbor);
                    visited[neighbor] = current;
                }
            }
            ChangeWeightHelpFunction(weightValue, ref frontier, ref visited, playerGraph, decreaseWeight, ref moveTile);
        }
        Debug.Log("VISITED COUNT: " + visited.Count);
    }

    private void ChangeWeightHelpFunction(int weight, ref Queue<int> oldFrontier, ref Dictionary<int, int> visited, bool playerGraph, int weightDecreaseValue)
    {
        weight -= weightDecreaseValue;
        Queue<int> newFrontier = new Queue<int>();
        
        while (oldFrontier.Count > 0)
        {
            int current = oldFrontier.Dequeue();

            if (playerGraph)
            {
                GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[current].GetComponent<Tile>().AddTileWeight(weight);
            }
            else
            {
                GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary()[current].GetComponent<Tile>().AddTileWeight(weight);
            }

            var neighbors = adjacencyList.ContainsKey(current) ? adjacencyList[current] : new List<int>();

            foreach (var neighbor in neighbors)
            {
                if (visited.ContainsKey(neighbor))
                {
                    continue;
                }
                else
                {
                    newFrontier.Enqueue(neighbor);
                    visited[neighbor] = current;
                }
            }
        }
        if (newFrontier.Count > 0)
            ChangeWeightHelpFunction(weight, ref newFrontier, ref visited, playerGraph, weightDecreaseValue);
    }

    private void ChangeWeightHelpFunction(int weight, ref Queue<int> oldFrontier, ref Dictionary<int, int> visited, bool playerGraph, int weightDecreaseValue, ref int tileToMoveTo)
    {
        weight -= weightDecreaseValue;
        Queue<int> newFrontier = new Queue<int>();

        while (oldFrontier.Count > 0)
        {
            int current = oldFrontier.Dequeue();

            if (playerGraph)
            {
                GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[current].GetComponent<Tile>().AddTileWeight(weight);
            }
            else
            {
                GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary()[current].GetComponent<Tile>().AddTileWeight(weight);
            }

            if (GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[tileToMoveTo].GetComponent<Tile>().GetTileWeight() > GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[current].GetComponent<Tile>().GetTileWeight())
            {
                tileToMoveTo = current;
            }

            var neighbors = adjacencyList.ContainsKey(current) ? adjacencyList[current] : new List<int>();

            foreach (var neighbor in neighbors)
            {
                if (visited.ContainsKey(neighbor))
                {
                    continue;
                }
                else
                {
                    newFrontier.Enqueue(neighbor);
                    visited[neighbor] = current;
                }
            }
        }
        if (newFrontier.Count > 0)
            ChangeWeightHelpFunction(weight, ref newFrontier, ref visited, playerGraph, weightDecreaseValue, ref tileToMoveTo);
    }

    public void enemyAttackDecision(int startIndex, int endIndex, List<int> attackTiles, out int totalWeight, out int tileAddition)
    {
        totalWeight = 0;
        int tempWeight = 0;
        tileAddition = 0;
        int addition = 0;

        for (int i = startIndex; i <= endIndex; i++)
        {
            for (int j = 1; j <= attackTiles.Count; j++)
            {
                tempWeight += GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[j + addition].gameObject.GetComponent<Tile>().GetTileWeight();
            }
            if (tempWeight > totalWeight)
            {
                totalWeight = tempWeight;
                tileAddition = (i - 1);
            }
            tempWeight = 0;
            addition++;
        }
    }

    private List<int> pathDicToList(ref Dictionary<int, int> previousDic, ref int goal)
    {
        List<int> pathway = new List<int>();
        int current, previous;
        current = goal;
        do
        {
            pathway.Insert(0, current);
            previous = current;
            current = previousDic[current];
        } while (current != previous);
        return pathway;
    }

    private void pathDicToList(ref Dictionary<int, int> previousDic, ref int goal, out int tileKey, out int weight, Enemy enemy)
    {
        Dictionary<int, GameObject> enemyTileDictionary = GameManager.GetInstance().GetGridManager().GetEnemyTileDictionary();
        List<int> pathway = new List<int>();
        int current, previous;
        current = goal;

        weight = 0;
        tileKey = 0;
        bool firstTime = true;

        do
        {
            weight += enemyTileDictionary[current].gameObject.GetComponent<Tile>().GetTileWeight();
            if (current != enemy.GetEnemyTileData().Key || firstTime)
            {
                pathway.Insert(0, current);
            }
            firstTime = false;
            previous = current;
            current = previousDic[current];
        } while (current != previous);
        tileKey = pathway[0];
    }
}
