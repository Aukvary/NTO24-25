using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _cameraSpeed;

    [SerializeField]
    private float _rotateSpeed;

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        if (!Input.GetKey(KeyCode.Mouse2))
            return;

        var xOffset = transform.right * -Input.GetAxis(Ñonstants.MouseX);
        var zOffset = transform.forward * -Input.GetAxis(Ñonstants.MouseY);

        transform.position = Vector3.Lerp(
            transform.position,
            transform.position + xOffset + zOffset,
            _cameraSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (!Input.GetKey(KeyCode.LeftAlt))
            return;
        transform
            .Rotate(0, Input.GetAxis(Ñonstants.MouseX) * _rotateSpeed, 0);
    }
}