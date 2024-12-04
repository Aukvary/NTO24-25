using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LoadingAwaiter : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _afterLoadingEvent;

    [SerializeField]
    private List<GameObject> _loadingObject;

    private void Start()
        => StartCoroutine(Await(_loadingObject.Select(o => o.GetComponent<ILoadable>()).Where(o => o != null)));
    private System.Collections.IEnumerator Await(IEnumerable<ILoadable> objects)
    {
        while (objects.Any(o => !o.Loaded))
            yield return null;
        _afterLoadingEvent?.Invoke();
    }
}