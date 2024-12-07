using TMPro;
using UnityEngine;

public class HomeHealthUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _health;

    [SerializeField]
    private TextMeshProUGUI _regeneration;

    [SerializeField]
    private RectTransform _valueBar;

    [SerializeField]
    private BreakeableObject _home;

    private GameObject[] _renderers;

    private void Start()
    {
        if (_home == null)
        {
            gameObject.SetActive(false);
            return;
        }

        _renderers = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _renderers[i] = transform.GetChild(i).gameObject;
        }
        _regeneration.text = _home.Regeneration.ToString();
        _health.text = $"{_home.Health} / {_home.MaxHealth}";
        _home.AddListerForHit(() =>
        {
            _valueBar.anchorMax = new(_home.Health / _home.MaxHealth, 1f);
            _health.text = $"{_home.Health} / {_home.MaxHealth}";
        });

        foreach (var r in _renderers)
            r.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            foreach (var r in _renderers)
                r.SetActive(true);
        else
            foreach (var r in _renderers)
                r.SetActive(false);
    }
}