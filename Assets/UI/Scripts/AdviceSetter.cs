using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdviceSetter : MonoBehaviour
{
    [SerializeField]
    private BearActivityManager _bearManager;

    [SerializeField]
    private BreakeableObject _apire;

    Dictionary<Func<bool>, string> CondAdvicePairs;

    private AdviceField _advice;

    private Vector3 _basePosition;
    private float _baseRotation;

    private void Awake()
    {
        _basePosition = _bearManager.transform.position;
        _baseRotation = _bearManager.transform.rotation.y;

        _apire._onHitEvent.AddListener(Attack);

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
                "левый ALT + ПКМ, чтобы вращать камеру"
            },
            {
                () => _baseRotation != _bearManager.transform.rotation.y,
                "Выбрав медведя, нажмите на деверо или камень, чтобы начать его добывать"
            },
            {
                () => _bearManager.Bears.Any(b => b.ExtractionController.Extracting),
                "Как только в инвентаря медведя появятся ресурсы, вы сможете сложить их в склад, нажав по нему правой кнопкой мыши"
            },
            {
                () => true, //lay out items
                "Зажмите на TAB, чтобы увидеть, какие ресурсы у вас есть"
            },
            {
                () => Input.GetKey(KeyCode.Tab),
                "Собирите достаточное количество ресурсов, чтобы построить мост, чтобы его построить положите ресурсы в склад и нажмите на коробку"
            },
            {
                () => true, //build bridge
                "Атакуйте кибер-улей нажав на него"
            },
            {
                () => true, //apire attack
                "Осторожно, хоть ульи не особо прочный, но каждый удар спаснит кибер-осу"
            },
            {
                () => true, //apire broke
                "Чтобы легче рассправлятся со своими врагами, можно прокачивать медведей, для этого необходимо зажать TAB вырать медведя и характеристику, которую вы хотите ему прокачать и нажать \"прокачать\""
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

    private void Attack()
    {

    }
}