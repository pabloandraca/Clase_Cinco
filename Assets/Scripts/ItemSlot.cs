using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemImage, frameImage;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Color highlightColor;

    Color defaultColor;

    void Awake()
    {
        defaultColor = frameImage.color;
    }

    public Item CurrentItem { get; private set; }

    public void SetItem(Item item)
    {
        CurrentItem = item;
        itemImage.sprite = CurrentItem.Sprite;
        itemNameText.text = $"{CurrentItem.Name} (#:{CurrentItem.Id})";
    }

    public void SetHighlight(bool on)
    {
        frameImage.color = on ? highlightColor : defaultColor;
    }
}