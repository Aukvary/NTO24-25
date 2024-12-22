using System.Collections.Generic;

public interface IDropable : IEntity
{
    IEnumerable<ResourceCountPair> DropableItems { get; }

    public void Drop(IInventoriable unit);
}