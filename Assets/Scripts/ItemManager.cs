using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public struct NameAndSprite
{
    public string name;
    public Sprite sprite;
}

[System.Serializable]
public class Item
{
    public int Id;
    public string Name;
    public Sprite Sprite;
}


public class ItemManager : MonoBehaviour
{
    public NameAndSprite[] nameAndSprites;
    public List<Item> items = new();
    [SerializeField] int itemAmount = 500;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform inventoryPanel;
    [SerializeField] ScrollRect scrollRect;

    List<ItemSlot> itemSlots = new();

    void Start()
    {
        GenerateItems();
        PopulateInventory();
    }

    void GenerateItems()
    {
        for (int i = 0; i < itemAmount; i++)
        {
            var randomIndex = Random.Range(0, nameAndSprites.Length);
            var item = new Item
            {
                Id = i,
                Name = nameAndSprites[randomIndex].name + " " + Random.Range(1000, 9999),
                Sprite = nameAndSprites[randomIndex].sprite
            };
            items.Add(item);
        }
    }

    public void PopulateInventory()
    {
        for (int i = inventoryPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(inventoryPanel.GetChild(i).gameObject);
        }

        itemSlots.Clear();

        for (int i = 0; i < items.Count; i++)
        {
            ItemSlot _slot = Instantiate(itemPrefab, inventoryPanel).GetComponent<ItemSlot>();
            _slot.SetItem(items[i]);
            itemSlots.Add(_slot);
        }

        ClearHighlights();
        ScrollToTop();
    }

    public void ClearHighlights()
    {
        foreach (Transform child in inventoryPanel)
        {
            child.GetComponent<ItemSlot>().SetHighlight(false);
        }
    }

    void ScrollToTop()
    {
        scrollRect.verticalNormalizedPosition = 1f;
        Canvas.ForceUpdateCanvases();
    }

    void ScrollToIndex(int index)
    {
        float f = Mathf.Clamp01(index / Mathf.Max(1f, items.Count - 1f));
        float normalizedPosition = 1f - f;

        scrollRect.verticalNormalizedPosition = normalizedPosition;
        Canvas.ForceUpdateCanvases();

        StartCoroutine(SmoothScrollTo(normalizedPosition, 0.15f));
    }

    IEnumerator SmoothScrollTo(float target, float duration)
    {
        if (!scrollRect) yield break;
        float start = scrollRect.verticalNormalizedPosition;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / duration);
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, target, k);
            yield return null;
        }
        scrollRect.verticalNormalizedPosition = target;
    }

    public void HighlithSearcResult(int index)
    {
        ClearHighlights();

        if (index >= 0 && index < itemSlots.Count)
        {
            itemSlots[index].SetHighlight(true);
            ScrollToIndex(index);
        }
    }
}