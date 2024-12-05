using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
    private Collider[] _collides;

    private void Awake()
    {
        _collides = GetComponentsInChildren<Collider>();

        foreach (Collider col in _collides)
            col.enabled = false;
    }

    public void SetActive(bool cond)
    {
        foreach (Collider col in _collides) 
            col.enabled = cond;
    }
}