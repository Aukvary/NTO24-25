using UnityEngine;
using UnityEngine.UI;

public class UnitSelectCell : MonoBehaviour
{
    [SerializeField]
    private Image _unitHead;

    [SerializeField]
    private Color _selectedColor;

    private Color _unselectedColor;

    private Unit _unit;

    private Image _cell;

    private bool _isSelected;

    public Unit Unit 
    {
        get => _unit;
        set
        {
            _unit = value;
            _unitHead.sprite = _unit.HeadSprite;
        } 
    }

    public bool IsSelected
    {
        get => _isSelected;

        set
        {
            _isSelected = value;
            if (value)
            {
                _cell.color = _selectedColor;
            }
            else
            {
                _cell.color = _unselectedColor;
            }
        }
    }

    public UnitUpgradeHUD HUD { get; set; }

    private void Awake()
    {
        _cell = GetComponent<Image>();
        _unselectedColor = _cell.color;
    }

    public void Select()
    {
        HUD.SelectedUnit = this;

    }
}