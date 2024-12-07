using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradeHUD : MonoBehaviour
{
    [SerializeField]
    private float _closedSpeed;

    [SerializeField]
    private BearActivityManager _bearActivityManager;

    [SerializeField]
    private Button _upgradeButton;
    private Image[] _upgradeButtonImages;

    [SerializeField]
    private Color _canUpgradeColor;
    [SerializeField]
    private Color _cantUpgradeColor;

    private RectTransform _hud;

    private UnitSelectCell[] _unitSelectCells;
    private SelectingUpgradeButton[] _upgradePanels;
    private DescriptionArea _descriptionArea;
    private LevelGradeBar _levelGradeBar;

    private Vector2 _minAnchor;
    private Vector2 _maxAnchor;

    private UnitSelectCell _selectedUnit;
    private SelectingUpgradeButton _selectedUpgradePanel;

    public UnitSelectCell SelectedUnit
    {
        get => _selectedUnit;
        set
        {
            if (_selectedUnit != null)
                _selectedUnit.IsSelected = false;
            _selectedUnit = value;
            if (value != null)
            {

                value.IsSelected = true;
                SelectedPanel = SelectedPanel;
            }
        }
    }

    public SelectingUpgradeButton SelectedPanel
    {
        get => _selectedUpgradePanel;

        set
        {
            if (_selectedUpgradePanel != null)
                _selectedUpgradePanel.IsSeleced = false;
            _selectedUpgradePanel = value;
            if (value != null)
            {
                value.IsSeleced = true;

                value.Level = value.UpgradeType switch
                {
                    UpgradeType.Damage => SelectedUnit.Bear.AttackLevel,
                    UpgradeType.Strenght => SelectedUnit.Bear.StrenghtLevel,
                    UpgradeType.Health => SelectedUnit.Bear.HealthLevel,

                    _ => 0
                };
                _levelGradeBar.Level = value.Level;

                _descriptionArea.Title = value.Title;
                _descriptionArea.Description = value.Description;
                _descriptionArea.Resourse = value.ResourceCountPair;
            }
        }
    }

    private bool _canUpgrade => SelectedPanel.ResourceCountPair.All(needRes => 
        SelectedUnit.Bear.Storage[needRes.Resource] >= needRes.Count  
    );

    private void Awake()
    {
        _unitSelectCells = GetComponentsInChildren<UnitSelectCell>();
        _upgradePanels = GetComponentsInChildren<SelectingUpgradeButton>();
        _descriptionArea = GetComponentInChildren<DescriptionArea>();
        _levelGradeBar = GetComponentInChildren<LevelGradeBar>();
        _hud = GetComponent<RectTransform>();
        _minAnchor = _hud.anchorMin;
        _maxAnchor = _hud.anchorMax;

        _hud.anchorMin = new(1, _minAnchor.y);

        _upgradeButtonImages = _upgradeButton.GetComponentsInChildren<Image>();

        foreach (var button in _upgradePanels)
            button.HUD = this;



        _upgradeButton.onClick.AddListener(() =>
        {
            if (!_canUpgrade)
                return;
            foreach (var res in SelectedPanel.ResourceCountPair)
                SelectedUnit.Bear.Storage[res.Resource] -= res.Count;

            SelectedUnit.Bear.Upgrade(SelectedPanel.UpgradeType);
            SelectedPanel = SelectedPanel;
        });
    }

    private void Start()
    {
        int i = 0;
        foreach (var bear in _bearActivityManager.Bears)
        {
            _unitSelectCells[i].Bear = bear;
            _unitSelectCells[i].HUD = this;
            i++;
        }

        SelectedUnit = _unitSelectCells[0];

        SelectedPanel = _upgradePanels[0];
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            _hud.anchorMin = Vector2.Lerp(
                _hud.anchorMin,
                _minAnchor,
                Time.deltaTime * _closedSpeed);
        else
            _hud.anchorMin = Vector2.Lerp(
                _hud.anchorMin,
                new(1, _minAnchor.y),
                Time.deltaTime * _closedSpeed);

        if (_canUpgrade && SelectedUnit.Bear.CanUpgrade(SelectedPanel.UpgradeType))
        {
            foreach (Image image in _upgradeButtonImages)
                image.color = _canUpgradeColor;
        }
        else
        {
            foreach (Image image in _upgradeButtonImages)
                image.color = _cantUpgradeColor;
        }
    }
}