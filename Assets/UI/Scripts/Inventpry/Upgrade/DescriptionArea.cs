using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionArea : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;

    [SerializeField] 
    private TextMeshProUGUI _description;

    private InventoryCellUI[] _cells;

    public string Title
    {
        get => _title.text;
        set => _title.text = value;
    }

    public string Description
    {
        get => _description.text;
        set => _description.text = value;
    }

    public List<SelectingUpgradeButton.ResourseCountPair> Resourse
    {
        set
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                try
                {
                    _cells[i].PairCell = value[i];
                }
                catch(ArgumentOutOfRangeException)
                {
                    _cells[i].Clear();
                }
            }
        }
    }

    private void Awake()
    {
        _cells = GetComponentsInChildren<InventoryCellUI>();
    }
}