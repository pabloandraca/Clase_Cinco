using UnityEngine;
using TMPro;
using System;
using System.Diagnostics;

public class ItemSearch : MonoBehaviour
{
    [SerializeField] ItemManager itemManager;
    [SerializeField] TMP_InputField searchInput;
    [SerializeField] TMP_Text resultText;

    bool sortedById = false;

    public void Search()
    {
        string query = searchInput.text;

        if (string.IsNullOrWhiteSpace(query))
        {
            resultText.text = "Please enter a search query.";
            return;
        }

        string output = $"Searching for '{query}'...\n";

        int id;
        if (int.TryParse(query, out id))
        {
            var sw = Stopwatch.StartNew();
            int indexLineal = LinealIdSearch(id);
            sw.Stop();
            output += $"Lineal ID Search: {FormatTime(sw)}\n";

            sw.Restart();
            int indexBinary = sortedById ? BinaryIdSearch(id) : -1;
            sw.Stop();
            output += $"Binary ID Search: {FormatTime(sw)}\n";

            if (indexLineal >= 0)
            {
                output += $"Lineal encontro : {itemManager.items[indexLineal].Name} (#:{itemManager.items[indexLineal].Id})\n";
                itemManager.HighlithSearcResult(indexLineal);
            }
            else
            {
                output += "Item not found.";
                itemManager.ClearHighlights();
            }

            if (indexBinary >= 0)
            {
                output += $"Binary encontro : {itemManager.items[indexBinary].Name} (#:{itemManager.items[indexBinary].Id})\n";
            }
            else if (sortedById)
            {
                output += "Item not found in binary search.";
            }
            else
            {
                output += "List not sorted by ID. Binary search skipped.";
            }
        }
        else
        {
            var sw = Stopwatch.StartNew();
            int index = LinealNameSearch(query);
            sw.Stop();
            output += $"Lineal Name Search: {FormatTime(sw)}\n";

            if (index >= 0)
            {
                output += $"Lineal encontro : {itemManager.items[index].Name} (#:{itemManager.items[index].Id})\n";
                itemManager.HighlithSearcResult(index);
            }
            else
            {
                output += "Item not found.";
                itemManager.ClearHighlights();
            }
        }

        resultText.text = output;
    }


    int LinealIdSearch(int id)
    {
        for (int i = 0; i < itemManager.items.Count; i++)
            if (itemManager.items[i].Id == id) return i;
        return -1;
    }

    int LinealNameSearch(string name)
    {
        for (int i = 0; i < itemManager.items.Count; i++)
            if (itemManager.items[i].Name == name) return i;
        return -1;
    }

    string FormatTime(Stopwatch sw)
    {
        return $"{sw.Elapsed.TotalMilliseconds:F3} ms";
    }

    int BinaryIdSearch(int id)
    {
        int left = 0;
        int right = itemManager.items.Count - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int midId = itemManager.items[mid].Id;
            if (midId == id) return mid;
            if (midId < id) left = mid + 1;
            else right = mid - 1;
        }
        return -1;
    }

    public void SortById()
    {
        var sw = Stopwatch.StartNew();
        ItemSort.Bubble(itemManager.items, (a, b) => a.Id.CompareTo(b.Id));
        sw.Stop();
        resultText.text = $"Sorted by ID in {FormatTime(sw)}";

        sortedById = true;
    }
}