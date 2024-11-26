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

        public static implicit operator KeyValuePair<Resource, int>(ResourseCountPair pair)
            => new(pair.Resource, pair.Count);
    }

    [SerializeField]
    private UpgradeType _upgradeType;

    [SerializeField]
    private List<ResourseCountPair> _resourcesLevel1;

    [SerializeField]
    private List<ResourseCountPair> _resourcesLevel2;

    [SerializeField]
    private List<ResourseCountPair> _resourcesLevel3;

    [SerializeField]
    private List<ResourseCountPair> _resourcesLevel4;

    [SerializeField]
    private Color _selectedColor;
    [SerializeField]
    private Color _unselectedColor;

    [SerializeField]
    private string _title;

    [SerializeField, TextArea]
    private string _upgradeDescription;

    private List<ResourseCountPair> _empty = new();

    private Image _hud;
    private Vector2 _minAnchor;
    private Vector2 _maxAnchor;

    private Vector2[] _closedPoses;

    private bool _isSelected;

    public UnitUpgradeHUD HUD { get; set; }

    public UpgradeType UpgradeType => _upgradeType;

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

    public int Level { get; set; }

    public List<ResourseCountPair> ResourceCountPair => Level switch
    {
        0 => _resourcesLevel1,
        1 => _resourcesLevel2,
        2 => _resourcesLevel3,
        3 => _resourcesLevel4,
        4 => _empty,
        _ => throw new System.Exception("idi naxui")
    };


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
    public void Select()
    {
        HUD.SelectedPanel = this;
    }
}