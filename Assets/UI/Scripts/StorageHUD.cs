using System.Collections.Generic;
using UnityEngine;

public class StorageHUD : MonoBehaviour
{
    [SerializeField]
    private byte _inRowCount;
    [SerializeField]
    private float _space;
    [SerializeField]
    private float _size;

    private InventoryCellUI[] _cells;

    private RectTransform _transform;

    private void Awake()
    {
        _cells = GetComponentsInChildren<InventoryCellUI>();
        _transform = GetComponentInChildren<RectTransform>();

    }

    public void UpdateHUD(Storage storage)
    {
        int i = 0;

        foreach (var item in storage.SrorageResources) 
        {
            _cells[i].PairCell = item;
            i++;
        }
    }
}