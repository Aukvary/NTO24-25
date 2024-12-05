using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdviceSetter : MonoBehaviour
{
    [SerializeField]
    private BearActivityManager _bearManager;

    Dictionary<Func<bool>, string> CondAdvicePairs;

    private AdviceField _advice;

    private Vector3 _basePosition;
    private float _baseRotation;

    private void Awake()
    {
        _basePosition = _bearManager.transform.position;
        _baseRotation = _bearManager.transform.rotation.y;

        _advice = GetComponent<AdviceField>();

        CondAdvicePairs = new Dictionary<Func<bool>, string>()
        {
            {
                () => _bearManager.SelectedUnits.Any(),
                "Нажмите левой кнопкой мыши в любое место, чтобы медведь начал передвигаться"
            },
            {
                () => _bearManager.Bears.Any(b => b.HasPath),
                "Нажмите на колёсико мышки и двигайте мышь, чтобы перемещать камеру"
            },
            {
                () => _basePosition != _bearManager.transform.position,
                "..."
            },
            {
                () => _baseRotation != _bearManager.transform.rotation.y,
                "..."
            },
            {
                () => _bearManager.Bears.Any(b => b.ExtractionController.Extracting),
                "..."
            },
            {
                () => true, //lay out items
                "..."
            },
            {
                () => true, //build bridge
                "..."
            },
            {
                () => true, //apire attack
                "..."
            },
            {
                () => true, //apire broke
                "..."
            },
            {
                () => true, //unit upgrade
                "..."
            }

        };

        StartCoroutine(StartGiveTask());
    }

    private System.Collections.IEnumerator StartGiveTask()
    {
        foreach (var pair in CondAdvicePairs)
        {
            while (!pair.Key.Invoke())
                yield return null;
            _advice.SetAdvice(pair.Value);
        }
        gameObject.SetActive(false);
    }
}