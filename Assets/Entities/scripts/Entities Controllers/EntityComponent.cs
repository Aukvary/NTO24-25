using UnityEngine;

public abstract class EntityComponent : MonoBehaviour
{
    public Entity Entity { get; private set; }

    protected virtual void Awake()
    {
        Entity = GetComponent<Entity>();
    }
    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }
}