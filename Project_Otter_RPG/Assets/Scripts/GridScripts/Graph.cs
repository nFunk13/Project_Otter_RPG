using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private Dictionary<Vector2, List<Vector2>> adjacencyList;

    public Graph()
    {
        adjacencyList = new Dictionary<Vector2, List<Vector2>>();
    }

    public void AddEdge(Vector2 vertex1, Vector2 vertex2)
    {
        if (!adjacencyList.ContainsKey(vertex1))
        {
            adjacencyList.Add(vertex1, new List<Vector2>());
        }
        if (!adjacencyList.ContainsKey(vertex2))
        {
            adjacencyList.Add(vertex2, new List<Vector2>());
        }

        adjacencyList[vertex1].Add(vertex2);
    }

    // Change this to go through each tile in the dictionary to change the value of each tile
    public List<Vector2> DFS(Vector2 startVertex, Vector2 goalVertex)
    {
        List<Vector2> visited = new List<Vector2>();
        List<Vector2> paths = new List<Vector2>();

        if (DFSHelperFunction(startVertex, goalVertex, visited, paths))
        {
            return paths;
        }

        return null;
    }

    // Change this to go through each tile in the dictionary to change the value of each tile
    public bool DFSHelperFunction(Vector2 current, Vector2 goalVertex, List<Vector2> visited, List<Vector2> path)
    {
        visited.Add(current);
        path.Add(current);

        if (current == goalVertex)
        {
            return true;
        }

        foreach (Vector2 neighbor in adjacencyList[current])
        {
            if (!visited.Contains(neighbor))
            {
                if (DFSHelperFunction(neighbor, goalVertex, visited, path))
                {
                    return true;
                }
            }
        }

        path.RemoveAt(path.Count - 1);
        return false;
    }
}
