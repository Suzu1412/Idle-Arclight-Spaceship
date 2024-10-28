using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[HelpURL("https://github.com/adammyhre/Unity-Utils/blob/master/UnityUtils/Scripts/Extensions/ListExtensions.cs")]
public static class ListExtensions
{
    /// <summary>
    /// Determines whether a collection is null or has no elements
    /// without having to enumerate the entire collection to get a count.
    ///
    /// Uses LINQ's Any() method to determine if the collection is empty,
    /// so there is some GC overhead.
    /// </summary>
    /// <param name="list">List to evaluate</param>
    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list == null || !list.Any();
    }

    /// <summary>
    /// Creates a new list that is a copy of the original list.
    /// </summary>
    /// <param name="list">The original list to be copied.</param>
    /// <returns>A new list that is a copy of the original list.</returns>
    public static List<T> Clone<T>(this IList<T> list)
    {
        List<T> newList = new();
        foreach (T item in list)
        {
            newList.Add(item);
        }

        return newList;
    }

    /// <summary>
    /// Swaps two elements in the list at the specified indices.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="indexA">The index of the first element.</param>
    /// <param name="indexB">The index of the second element.</param>
    public static void Swap<T>(this IList<T> list, int indexA, int indexB)
    {
        (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
    }

    public static void ReplaceOrAdd<T>(this IList<T> list, T oldValue, T newValue)
    {
        var index = list.IndexOf(oldValue);
        if (index != -1)
        {
            list[index] = newValue;
        }
        else
        {
            list.Add(newValue);
        }
    }
}