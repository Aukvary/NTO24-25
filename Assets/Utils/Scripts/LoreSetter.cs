using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoreSetter : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _loreObjects;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private UnityEvent _afterEndEvent;

    private int _index = 0;

    private void Awake()
    {
        _loreObjects.ForEach(o => o.SetActive(false));
        _loreObjects[0].SetActive(true);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        _loreObjects[_index].SetActive(false);
        _index++;
        if (_index < _loreObjects.Count)
        {
            _loreObjects[_index].SetActive(true);
        }
        else
        {
            Camera.SetupCurrent(_camera);
            _afterEndEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}