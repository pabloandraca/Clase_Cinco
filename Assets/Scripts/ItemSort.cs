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

    public static void Insertion(List<Item> list, Comparison<Item> comparison)
    {
        if (list == null || list.Count <= 1) return;

        for (int i = 1; i < list.Count; i++)
        {
            Item key = list[i];
            int j = i - 1;

            while (j >= 0 && comparison(list[j], key) > 0)
            {
                list[j + 1] = list[j];
                j--;
            }
            list[j + 1] = key;
        }
    }

    public static void Quick(List<Item> list, Comparison<Item> comparison)
    {
        if (list == null || list.Count <= 1) return;
        QuickSort(list, 0, list.Count - 1, comparison);
    }

    static void QuickSort(List<Item> list, int low, int high, Comparison<Item> comparison)
    {
        if (low >= high) return;
        int p = Partition(list, low, high, comparison);
        QuickSort(list, low, p - 1, comparison);
        QuickSort(list, p + 1, high, comparison);
    }

    static int Partition(List<Item> list, int low, int high, Comparison<Item> comparison)
    {
        Item pivot = list[high];
        int i = low;
        for (int j = low; j < high; j++)
        {
            if (comparison(list[j], pivot) < 0)
            {
                (list[i], list[j]) = (list[j], list[i]);
                i++;
            }
        }
        (list[i], list[high]) = (list[high], list[i]);
        return i;
    }
}