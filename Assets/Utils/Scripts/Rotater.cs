using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotateDirection;


    private void Update()
        => transform.Rotate(_rotateDirection);
}