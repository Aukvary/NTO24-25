using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCellUI : MonoBehaviour
{
    private Image _itemImage;
    private TextMeshProUGUI _count;

    public RectTransform Transform { get; private set; } 

    public Image Back { get; private set; }

    public Cell Cell
    {
        set
        {
            if (value.Resource == null)
            {
                _itemImage.enabled = false;
                _count.text = "";
                return;
            }
            _itemImage.enabled = true;
            _itemImage.sprite = value.Resource.Sprite;
            _count.text = value.Count.ToString();
        }
    }

    public KeyValuePair<Resource, uint> PairCell
    {
        set
        {
            _itemImage.enabled = true;
            _itemImage.sprite = value.Key.Sprite;
            _count.text = value.Value.ToString();
        }
    }

    private void Awake()
    {
        Transform = GetComponent<RectTransform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).TryGetComponent<Image>(out var image))
                continue;
            _itemImage = image;
        }
        _count = GetComponentInChildren<TextMeshProUGUI>();
        Back = GetComponent<Image>();
    }
}