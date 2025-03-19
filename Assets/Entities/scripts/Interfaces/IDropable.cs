using System.Collections.Generic;

namespace NTO24
{
    public interface IDropable : IEntity
    {
        DropController DropController { get; }

        IEnumerable<Pair<PickableObject, int>> Resources
            => DropController?.Resources;

        void Drop()
            => DropController.Drop();
    }
}