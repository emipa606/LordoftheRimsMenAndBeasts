using System;
using System.Collections.Generic;

namespace AR_PawnShields;

// Code copied between JecsTools and PawnShields - keep them in sync!
public static class ListExtensions
{
    public static List<T> AsList<T>(this IEnumerable<T> enumerable)
    {
        return enumerable as List<T> ?? [..enumerable];
    }

    public static List<T> PopAll<T>(this ICollection<T> collection)
    {
        var list = new List<T>(collection);
        collection.Clear();
        return list;
    }

    public static int FindSequenceIndex<T>(this List<T> list, params Predicate<T>[] sequenceMatches)
    {
        return list.FindSequenceIndex(0, list.Count, sequenceMatches);
    }

    private static int FindSequenceIndex<T>(this List<T> list, int startIndex, int count,
        params Predicate<T>[] sequenceMatches)
    {
        if (sequenceMatches is null)
        {
            throw new ArgumentNullException(nameof(sequenceMatches));
        }

        if (sequenceMatches.Length == 0)
        {
            throw new ArgumentException("sequenceMatches must not be empty");
        }

        if (count - sequenceMatches.Length < 0)
        {
            return -1;
        }

        count -= sequenceMatches.Length - 1;
        var index = list.FindIndex(startIndex, count, sequenceMatches[0]);
        while (index != -1)
        {
            var allMatched = true;
            for (var matchIndex = 1; matchIndex < sequenceMatches.Length; matchIndex++)
            {
                if (sequenceMatches[matchIndex](list[index + matchIndex]))
                {
                    continue;
                }

                allMatched = false;
                break;
            }

            if (allMatched)
            {
                break;
            }

            startIndex++;
            count--;
            index = list.FindIndex(startIndex, count, sequenceMatches[0]);
        }

        return index;
    }
}