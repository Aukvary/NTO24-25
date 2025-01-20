using System.Linq;
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

        var bear = _activityManager.Bears.First();
        transform.rotation = Quaternion.Euler(0, 180, 0);
        transform.position = new(
            bear.transform.position.x,
            transform.position.y,
            bear.transform.position.z - _hotkeyChangeZCameraPosition);
    }

    private void Update()
    {
        Move();
        Rotate();
        Zooming();
        HotKeySelect();
    }

    private void Move()
    {
        if (!Input.GetKey(KeyCode.Mouse2))
            return;

        var xOffset = transform.right * -Input.GetAxis(Stuff.MouseX);
        var zOffset = transform.forward * -Input.GetAxis(Stuff.MouseY);

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
            .Rotate(0, Input.GetAxis(Stuff.MouseX) * _rotateSpeed, 0);
    }

    private void Zooming()
    {
        var newFow = Mathf.Clamp(Camera.main.fieldOfView - Input.mouseScrollDelta.y, _minFOV, _maxFOV);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, newFow, Time.deltaTime * _changeFOVSpeed);
    }

    private void HotKeySelect()
    {
        var hotkey = KeyCode.Alpha1;
        int count = _activityManager.Bears.Count();

        for (int i = 0; i < count; i++)
        {
            if (!Input.GetKeyDown(hotkey + i))
                continue;

            var bear = _activityManager.Bears.ElementAt(i);

            /*if (bear == _activityManager.InventoryUnit)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.position = new(
                    bear.transform.position.x,
                    transform.position.y,
                    bear.transform.position.z - _hotkeyChangeZCameraPosition);
            }*/

            _activityManager.HotKeySelectBear(bear);
        }
    }
}