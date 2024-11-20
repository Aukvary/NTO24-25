using System.Collections;
using System.Linq;
using UnityEngine;

public class InventoryHUD : MonoBehaviour
{
    private InventoryCellUI[] _inventoryCells = new InventoryCellUI[6];

    public Unit _unit;

    public Unit Unit
    {
        get => _unit;

        set
        {
            if (_unit != null)
            {
                value.Inventory.OnInventoryChanged -= UpdateCells;
            }

            value.Inventory.OnInventoryChanged += UpdateCells;


            int i = 0;
            foreach (var cell in value.Inventory.Resources)
            {
                _inventoryCells[i].Cell = cell;
                i++;
            }
        }
    }

    private void Awake()
    {
        _inventoryCells = GetComponentsInChildren<InventoryCellUI>();
    }

    private void UpdateCells(Inventory inventory)
    {
        int i = 0;
        foreach (var cell in inventory.Resources)
        {
            _inventoryCells[i].Cell = cell;
            i++;
        }
    }
}