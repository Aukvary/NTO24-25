using System.Collections.Generic;
using UnityEngine;

public class StorageHUD : MonoBehaviour
{
    private InventoryCellUI[] _cells = new InventoryCellUI[4];

    private void Awake()
    {
        _cells = GetComponentsInChildren<InventoryCellUI>();
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