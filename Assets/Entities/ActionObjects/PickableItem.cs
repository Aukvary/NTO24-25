using UnityEngine;

public class PickableItem : ActionObject
{
    [SerializeField]
    private Resource _resource;

    private void Start()
        => Destroy(gameObject, 20);
    public void Spawn(Vector3 spawnPos)
    {
        var item = Instantiate(this, spawnPos, Quaternion.identity);
        GetComponent<Rigidbody>()
            .AddForce((-item.transform.forward + item.transform.up)*10, ForceMode.Force);
    }
    public override void Interact(Unit unit)
    {
        if (unit.Inventory.TryToAdd(_resource))
            Destroy(gameObject);
    }
}