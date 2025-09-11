using System;
using System.Collections.Generic;
using UnityEngine;

public static class ItemSort
{
    public static void Bubble(List<Item> list, Comparison<Item> comparison)
    {
        if (list == null || list.Count <= 1) return;

        bool swapped = true;
        int n = list.Count;

        while (swapped)
        {
            swapped = false;
            for (int i = 1; i < n; i++)
            {
                if (comparison(list[i - 1], list[i]) > 0)
                {
                    (list[i - 1], list[i]) = (list[i], list[i - 1]);
                    swapped = true;
                }
            }
            n--;
        }
    }
}