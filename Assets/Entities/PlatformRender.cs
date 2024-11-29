using UnityEngine;

public class PlatformRender : MonoBehaviour 
{
    private MeshRenderer _renderer;

    private void Awake()
        => _renderer = GetComponentInChildren<MeshRenderer>();

    private void OnBecameInvisible()
        => _renderer.enabled = false;

    private void OnBecameVisible()
        => _renderer.enabled = true;
}