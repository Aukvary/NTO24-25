using System.Collections.Generic;
using UnityEngine.Events;

namespace NTO24
{
    public interface ISavableComponent
    {
        UnityEvent OnDataChangeEvent { get; }

        string Name { get; }

        string[] Data { get; }

        void ServerInitialize(IEnumerable<string> data);
    }
}