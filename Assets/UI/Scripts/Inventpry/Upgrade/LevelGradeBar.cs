using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGradeBar : MonoBehaviour
{
    [SerializeField]
    private List<Image> _cells;

    [SerializeField]
    private Color _enableColor;

    [SerializeField]
    private Color _disableColor;

    public int Level
    {
        set
        {
            var len = Mathf.Clamp(value, 0, _cells.Count);
            foreach (var cell in _cells)
                cell.color = _disableColor;
            for (int i = 0; i < value; i++)
                _cells[i].color = _enableColor;
        }
    }

    private void Awake()
    {
        Level = 0;
    }
}