using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

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

    // Change this to go through each tile in the dictionary to change the value of each tile
    public List<int> DFS(int startVertex, int goalVertex)
    {
        List<int> visited = new List<int>();
        List<int> paths = new List<int>();
        List<int> shortestPath = new List<int>();

        if (DFSHelperFunction(startVertex, goalVertex, visited, paths, ref shortestPath))
        {
            return shortestPath;
        }

        return null;
    }

    // Change this to go through each tile in the dictionary to change the value of each tile
    public bool DFSHelperFunction(int current, int goalVertex, List<int> visited, List<int> path, ref List<int> shortPath)
    {
        visited.Add(current);
        path.Add(current);

        if (current == goalVertex)
        {
            return true;
        }

        foreach (int neighbor in adjacencyList[current])
        {
            if (!visited.Contains(neighbor))
            {
                if (DFSHelperFunction(neighbor, goalVertex, visited, path, ref shortPath))
                {
                    if (shortPath.Count <= 0)
                    {
                        shortPath = path;
                    }
                    if (path.Count < shortPath.Count)
                    {
                        shortPath = path;
                    }
                }
            }
        }

        path.RemoveAt(path.Count - 1);
        return true;
    }

    public Dictionary<int, List<int>> GetAdjacencyList()
    {
        return adjacencyList;
    }
}
