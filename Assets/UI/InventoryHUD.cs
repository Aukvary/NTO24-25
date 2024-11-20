using System.Collections;
using UnityEngine;

public class InventoryHUD : MonoBehaviour
{
    private InventoryCellUI[] inventoryCells;

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
                inventoryCells[i].Cell = cell;
                i++;
            }
        }
    }

    private void Awake()
    {
        inventoryCells = GetComponentsInChildren<InventoryCellUI>();
    }

    private void UpdateCells(Inventory inventory)
    {
        int i = 0;
        foreach (var cell in inventory.Resources)
        {
            inventoryCells[i].Cell = cell;
            i++;
        }
    }
}