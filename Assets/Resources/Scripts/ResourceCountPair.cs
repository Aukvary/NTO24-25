using UnityEngine;
[System.Serializable]
public struct ResourceCountPair
{
    [SerializeField]
    private Resource _resorce;

    [SerializeField, Min(0)]
    private int _count;

    public Resource Resource => _resorce;

    public int Count => _count; 
}