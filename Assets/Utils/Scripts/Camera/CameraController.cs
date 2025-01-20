using System.Linq;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _cameraSpeed;

    [SerializeField]
    private float _rotateSpeed;

    [SerializeField]
    private float _changeFOVSpeed;
    [SerializeField]
    private float _minFOV;

    [SerializeField]
    private float _maxFOV;

    [SerializeField]
    private float _hotkeyChangeZCameraPosition;

    private ContollableActivityManager _activityManager;

    private void Start()
    {
        _activityManager = GetComponentInParent<ContollableActivityManager>();
    }

    private void Update()
    {
        MouseMove();
        KeyboardMove();
        Rotate();
        Zooming();
    }

    private void KeyboardMove()
    {
        if (Input.GetKey(KeyCode.Mouse2))
            return;
        Move(Input.GetAxis(Stuff.HORIZONTAL), Input.GetAxis(Stuff.VERTICAL));
    }

    private void MouseMove()
    {
        if (!Input.GetKey(KeyCode.Mouse2))
            return;
        Move(-Input.GetAxis(Stuff.MOUSEX), -Input.GetAxis(Stuff.MOUSEY));
    }

    private void Move(float x, float z)
    {
        var xOffset = transform.right * x;
        var zOffset = transform.forward * z;

        transform.position = Vector3.Lerp(
            transform.position,
            transform.position + xOffset + zOffset,
            _cameraSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (!Input.GetKey(KeyCode.LeftAlt) || !Input.GetKey(KeyCode.Mouse1))
            return;
        transform
            .Rotate(0, Input.GetAxis(Stuff.MOUSEX) * _rotateSpeed, 0);
    }

    private void Zooming()
    {
        var newFow = Mathf.Clamp(Camera.main.fieldOfView - Input.mouseScrollDelta.y, _minFOV, _maxFOV);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, newFow, Time.deltaTime * _changeFOVSpeed);
    }
}