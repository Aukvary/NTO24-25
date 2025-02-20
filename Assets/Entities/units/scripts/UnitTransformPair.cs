using System;
using UnityEngine;

[Serializable]
public struct UnitTransformPair<T>
{
    [field: SerializeField]
    public T Unit { get; private set; }

    [field: SerializeField]
    public Transform Position { get; private set; }
}