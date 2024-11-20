using UnityEngine;

public class PickableItem : ActionObject
{
    [SerializeField]
    private Resource _resource;

    private void Start()
        => Destroy(gameObject, 20);
    public void Spawn(Vector3 spawnPos)
        => Instantiate(this, spawnPos, Quaternion.identity);
    public override void Interact(Unit unit)
    {
        if (unit.Inventory.TryToAdd(_resource))
            Destroy(gameObject);
    }
}