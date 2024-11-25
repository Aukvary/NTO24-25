using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SelectingUpgradeButton : MonoBehaviour
{
    [System.Serializable]
    public class ResourseCountPair
    {
        [SerializeField]
        private Resource _resource;
        [SerializeField]
        private int _count;

        public Resource Resource => _resource;
        public int Count => _count;
    }


    [SerializeField]
    private List<ResourseCountPair> _resources;

    [SerializeField]
    private Color _selectedColor;
    [SerializeField]
    private Color _unselectedColor;

    [SerializeField]
    private string _title;

    [SerializeField, TextArea]
    private string _upgradeDescription;

    private Image _hud;
    private Vector2 _minAnchor;
    private Vector2 _maxAnchor;

    private Vector2[] _closedPoses;

    private bool _isSelected;

    public UnitUpgradeHUD HUD { get; set; }

    public string Title => _title;
    public string Description => _upgradeDescription;

    public bool IsSeleced
    {
        get => _isSelected;

        set
        {
            _isSelected = value;
            if (value)
                _hud.color = _selectedColor;
            else
                _hud.color = _unselectedColor;
        }
    }

    public IEnumerable<ResourseCountPair> ResourceCountPair => _resources;

    private void Awake()
    {
        _hud = GetComponent<Image>();

        _closedPoses = new Vector2[]{
            _minAnchor,
            new Vector2((_maxAnchor.x + _minAnchor.x) / 2, _minAnchor.y),
            new Vector2(_maxAnchor.x, _minAnchor.y)
        };

        IsSeleced = false;
    }
    private void Update()
    {
        
    }
    public void Select()
    {
        HUD.SelectedPanel = this;
    }
}