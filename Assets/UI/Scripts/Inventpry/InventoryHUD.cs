using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    [SerializeField]
    private float _closedSpeed;

    [SerializeField]
    private Image _healthBar;

    [SerializeField]
    private TextMeshProUGUI _healthText;

    [SerializeField]
    private TextMeshProUGUI _regenerationText;

    [SerializeField]
    private Image _bearHeadRenderer;

    private Image _hud;
    private Vector2 _minAnchor;
    private Vector2 _maxAnchor;

    private InventoryCellUI[] _inventoryCells = new InventoryCellUI[6];

    private Unit _unit;

    public Unit Unit
    {
        get => _unit;

        set
        {
            if (_unit != null)
            {
                _unit.Inventory.OnInventoryChanged -= UpdateHUD;
                _unit.OnHealthChangeEvent -= UpdateHUD;
            }

            _unit = value;
            if (value == null)
            {
                _healthBar.rectTransform.anchorMax = Vector2.right + Vector2.up;
                _healthText.text = "";
                return;
            }
            value.Inventory.OnInventoryChanged += UpdateHUD;
            value.OnHealthChangeEvent += UpdateHUD;

            _bearHeadRenderer.sprite = value.HeadSprite;

            UpdateHUD(Unit);
        }
    }

    private void Awake()
    {
        _inventoryCells = GetComponentsInChildren<InventoryCellUI>();
        _hud = GetComponent<Image>();

        _minAnchor = _hud.rectTransform.anchorMin;
        _maxAnchor = _hud.rectTransform.anchorMax;

        _hud.rectTransform.anchorMax = _minAnchor;

        Unit = null;
    }

    private void Update()
    {
        if (Unit != null)
            _hud.rectTransform.anchorMax = Vector2.Lerp(
                _hud.rectTransform.anchorMax,
                _maxAnchor,
                Time.deltaTime * _closedSpeed);
        else
            _hud.rectTransform.anchorMax = Vector2.Lerp(
                _hud.rectTransform.anchorMax,
                _minAnchor,
                Time.deltaTime * _closedSpeed);
    }

    private void UpdateHUD(Unit unit)
    {
        int i = 0;
        foreach (var cell in unit.Inventory.Resources)
        {
            _inventoryCells[i].Cell = cell;
            i++;
        }

        _healthBar.rectTransform.anchorMax = new(Unit.Health / Unit.MaxHealth, 1);
        _healthText.text = $"{System.Math.Round(unit.Health, 1)} / {unit.MaxHealth}";
        _regenerationText.text = $"+{unit.Regeneration}";
    }
}