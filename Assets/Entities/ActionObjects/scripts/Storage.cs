using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Storage : Entity, IInteractable, IInventoriable, ILoadable
{
    [field: SerializeField]
    public UnityEvent<Entity> OnInteracEvent { get; private set; }

    [field: SerializeField]
    public UnityEvent OnUserInitializeEvent { get; private set; }

    public bool Interactable => true;

    public Inventory Inventory { get; private set; }

    public int CellCount => Resource.ResourceNames.Count();

    public UnityEvent<ResourceCountPair> OnFailedAddEvent => null;

    public string Name => "storage";

    public bool IsInitialized { get; set; }

    public User User { get; private set; }


    public IEnumerable<string> GetStringParametors()
    {
        return null;
    }

    public Task Initialize()
    {
        return null;
    }
}