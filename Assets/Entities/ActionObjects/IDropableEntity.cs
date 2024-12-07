using System.Collections.Generic;
using UnityEngine;

public interface IDropableEntity
{
    IEnumerable<ResourceCountPair> DropableItems { get; }

    public void Drop(Bear unit);
}