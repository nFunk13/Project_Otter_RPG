using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Rendering;

public class Graph : MonoBehaviour
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

    public void ChangePlayerTileWeights(int startInt)
    {
        Dictionary<int, int> visited = new Dictionary<int, int>();
        visited[startInt] = startInt;
        Queue<int> frontier = new Queue<int>();
        frontier.Enqueue(startInt);
        int weightValue = 5;
        int parced = visited.Count;

        while (frontier.Count > 0)
        {
            if (parced == (visited.Count + 1))
            {
                parced++;
                weightValue--;
            }
            int current = frontier.Dequeue();
            GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[current].GetComponent<Tile>().AddTileWeight(weightValue);
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
                    //PlayerWeightChangeHelperFunction(weightValue, ref frontier, ref visited);
                }
            }
        }
        Debug.Log("VISITED COUNT: " + visited.Count);
    }

    private void PlayerWeightChangeHelperFunction(int weight, ref Queue<int> frontier, ref Dictionary<int, int> visited)
    {
        if (weight > 0)
        {
            weight--;
        }

        int current = frontier.Dequeue();
        GameManager.GetInstance().GetGridManager().GetPlayerTileDictionary()[current].GetComponent<Tile>().AddTileWeight(current);
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
        PlayerWeightChangeHelperFunction(weight, ref frontier, ref visited);
        
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
}
