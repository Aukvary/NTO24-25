using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    public Entity EntityReference => this;

    protected virtual void Awake()
    {
        if (this is ILoadable loadable)
            loadable.InitilizeUserInfo();
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }
}