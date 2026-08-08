using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();

    public int Count
    {
        get
        {
            return elements.Count;
        }
    }

    /// <summary>
    /// This function takes in any element along with its priority. A lower value results in a higher priority
    /// </summary>
    /// <param name="item"></param>
    /// <param name="priority"></param>
    public void Enqueue(T item, int priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    /// <summary>
    /// This function finds the item with the highest priority, stores it, removes it from the list, and then returns that element
    /// </summary>
    /// <returns></returns>
    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestElement = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestElement;
    }
}
