using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    [Header("Entity settings")]
    [SerializeField]
    private UnityEvent<Entity> _onInteractEvent;

    public abstract bool Interactable { get; }

    public Entity EntityReference => this;

    protected virtual void Awake()
    {
        GetComponent<ILoadable>()?.InitilizeUserInfo();
        GetComponent<IInventoriable>()?.InitializeInventory();
    }

    public bool InteractBy(Entity by)
    {
        if (!Interactable)
            return false;

        Interact(by);
        _onInteractEvent.Invoke(by);
        return true;
    }

    protected abstract void Interact(Entity by);
}