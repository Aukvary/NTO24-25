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

    [SerializeField]
    private ConstructionObject _bridge;

    [SerializeField]
    private Storage _storage;

    Dictionary<Func<bool>, string> CondAdvicePairs;

    private AdviceField _advice;

    private SceneChanger _sceneChanger = new();

    private Vector3 _basePosition;
    private float _baseRotation;

    private bool _layOut;
    private bool _attack;
    private bool _broke;
    private bool _build;

    private void Start()
    {
        _basePosition = _bearManager.transform.position;
        _baseRotation = _bearManager.transform.rotation.y;

        _bearManager.OnHotKeySelect += () => _basePosition = _bearManager.transform.position;

        _apire.AddListerForHit(AttackApire);
        _apire.AddListnerForDeath(BrokeApire);
        _bridge.AddListnerToBuild(BuildBridge);
        _storage.OnLayOutItems.AddListener(LayOut);

        _advice = GetComponent<AdviceField>();

        CondAdvicePairs = new Dictionary<Func<bool>, string>()
        {
            {
                () => _bearManager.SelectedUnits.Any(),
                "Нажмите ПКМ в любое место, чтобы медведь начал передвигаться"
            },
            {
                () => _bearManager.Bears.Any(b => b.HasPath),
                "Нажмите на колёсико мышки и двигайте мышь, чтобы перемещать камеру"
            },
            {
                () => _basePosition != _bearManager.transform.position,
                "Левый ALT + ПКМ, чтобы вращать камеру"
            },
            {
                () => _baseRotation != _bearManager.transform.rotation.y,
                "Выбрав медведя, нажмите на дерево или камень, чтобы начать его добывать"
            },
            {
                () => _bearManager.Bears.Any(b => !b.HasPath && b.Behaviour.Target is ResourceObjectSpawner),
                "Как только в инвентаре медведя появятся ресурсы, вы сможете сложить их на склад (конструкция с коробками внутри), нажав по нему правой кнопкой мыши"
            },
            {
                () => _layOut,
                "Зажмите TAB, чтобы увидеть информацию о ресурсах на складе и прокачке медведей"
            },
            {
                () => Input.GetKey(KeyCode.Tab),
                "Чтобы построить мост, надо положить ресурсы на склад, а потом подойти к коробке, на которой изображены ресурсы для строительтва, и нажать по ней ПКМ"
            },
            {
                () => _build,
                "Атакуйте кибер-улей, нажав на него"
            },
            {
                () => _attack,
                "Осторожно, хоть ульи и не очень прочные, каждый удар спавнит кибер-осу!"
            },
            {
                () => _broke,
                "Чтобы легче расправляться со своими врагами, можно прокачивать медведей. Для этого нужно зажать TAB, выбрать медведя и характеристику, а потом нажать \"прокачать\""
            },
            {
                () => _bearManager.Bears.Any(b =>
                {
                    bool attack = b.AttackLevel > 0;
                    bool strranght = b.StrenghtLevel > 0;
                    bool health = b.HealthLevel > 0;

                    return attack || strranght || health;
                }),
                "Удачной игры!"
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
        yield return new WaitForSeconds(10);
        _sceneChanger.ExitToMenu();
    }

    private void LayOut(Storage _)
        => _layOut = true;

    private void AttackApire()
        => _attack = true;

    private void BrokeApire()
        => _broke = true;

    private void BuildBridge()
        => _build = true;

}