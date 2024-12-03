using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageHUD : MonoBehaviour
{
    [SerializeField]
    private float _closedSpeed;

    [SerializeField]
    private RectTransform _hud;

    private InventoryCellUI[] _cells;

    private RectTransform _transform;


    private Vector2 _minAnchor;
    private Vector2 _maxAnchor;

    private void Awake()
    {
        _cells = GetComponentsInChildren<InventoryCellUI>();
        _transform = GetComponentInChildren<RectTransform>();

        _minAnchor = _hud.anchorMin;
        _maxAnchor = _hud.anchorMax;

        _hud.anchorMin = new(1, _minAnchor.y);
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

    }

    public void UpdateHUD(Storage storage)
    {
        int i = 0;
        foreach (var item in storage.SrorageResources) 
        {
            _cells[i].PairCell = item;
            i++;
        }
    }
}