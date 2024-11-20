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
                _unit.OnPickItem -= UpdateCells;
            }

            value.OnPickItem += UpdateCells;


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

    private void UpdateCells(Unit unit)
    {
        int i = 0;
        foreach (var cell in unit.Inventory.Resources)
        {
            inventoryCells[i].Cell = cell;
            i++;
        }
    }
}