using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public static class Shuffle
{
    public static void Randomize<T>(this IList<T> list)  
    {  
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}