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
    private Image _upgradeButtonImage;
    private Button _upgradeButton;

    [SerializeField]
    private Color _canUpgradeColor;
    [SerializeField]
    private Color _cantUpgradeColor;

    private Image _hud;

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
                    UpgradeType.Damage => SelectedUnit.Unit.AttackLevel,
                    UpgradeType.Strenght => SelectedUnit.Unit.StrenghtLevel,
                    UpgradeType.Health => SelectedUnit.Unit.HealthLevel,

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
        SelectedUnit.Unit.Storage[needRes.Resource] >= needRes.Count  
    );

    private void Awake()
    {
        _unitSelectCells = GetComponentsInChildren<UnitSelectCell>();
        _upgradePanels = GetComponentsInChildren<SelectingUpgradeButton>();
        _descriptionArea = GetComponentInChildren<DescriptionArea>();
        _levelGradeBar = GetComponentInChildren<LevelGradeBar>();
        _hud = GetComponent<Image>();
        _minAnchor = _hud.rectTransform.anchorMin;
        _maxAnchor = _hud.rectTransform.anchorMax;

        _hud.rectTransform.anchorMin = new(1, _minAnchor.y);

        _upgradeButton = _upgradeButtonImage.GetComponent<Button>();

        foreach (var button in _upgradePanels)
            button.HUD = this;



        _upgradeButton.onClick.AddListener(() =>
        {
            if (!_canUpgrade)
                return;
            foreach (var res in SelectedPanel.ResourceCountPair)
                SelectedUnit.Unit.Storage[res.Resource] -= res.Count;

            SelectedUnit.Unit.Upgrade(SelectedPanel.UpgradeType);
            SelectedPanel = SelectedPanel;
        });
    }

    private void Start()
    {

        int i = 0;

        foreach (var bear in _bearActivityManager.Bears)
        {
            _unitSelectCells[i].Unit = bear;
            _unitSelectCells[i].HUD = this;
            i++;
        }

        SelectedUnit = _unitSelectCells[0];

        SelectedPanel = _upgradePanels[0];
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            _hud.rectTransform.anchorMin = Vector2.Lerp(
                _hud.rectTransform.anchorMin,
                _minAnchor,
                Time.deltaTime * _closedSpeed);
        else
            _hud.rectTransform.anchorMin = Vector2.Lerp(
                _hud.rectTransform.anchorMin,
                new(1, _minAnchor.y),
                Time.deltaTime * _closedSpeed);

        if (_canUpgrade && SelectedUnit.Unit.CanUpgrade(SelectedPanel.UpgradeType))
        {
            _upgradeButtonImage.color = _canUpgradeColor;
        }
        else
        {
            _upgradeButtonImage.color = _cantUpgradeColor;
        }
    }
}