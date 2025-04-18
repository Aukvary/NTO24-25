using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float _cameraSpeed;

        [SerializeField]
        private float _followSpeed;

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

        [field: HideInInspector]
        public UnityEvent OnMouseMove { get; private set; } = new();

        [field: HideInInspector]
        public UnityEvent OnKeyBoardMove { get; private set; } = new();

        private EntitySelector _selector;

        private bool _follow;
        private Entity _target;

        public static CameraController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _selector = GetComponent<EntitySelector>();
        }

        private void Start()
        {
            _selector.OnRepeatSelectEvent.AddListener(e =>
            {
                if (e == null)
                    return;
                Vector3 position = e.transform.position;
                _follow = true;
                _target = e;
                transform.position = new(position.x, transform.position.y, position.z);
            });
        }

        private void Update()
        {
            MouseMove();
            KeyboardMove();
            Follow();
            Rotate();
            Zooming();
        }

        private void KeyboardMove()
        {
            if (Input.GetKey(KeyCode.Mouse2))
                return;
            Move(Input.GetAxis(Stuff.HORIZONTAL), Input.GetAxis(Stuff.VERTICAL));
            if (Input.GetAxis(Stuff.HORIZONTAL) + Input.GetAxis(Stuff.VERTICAL) > 0)
                OnKeyBoardMove.Invoke();
        }

        private void MouseMove()
        {
            if (!Input.GetKey(KeyCode.Mouse2))
                return;

            Move(-Input.GetAxis(Stuff.MOUSEX), -Input.GetAxis(Stuff.MOUSEY));
            OnMouseMove.Invoke();
        }

        private void Follow()
        {
            if(!_follow)
                return;
            
            Vector3 position = new(
                _target.transform.position.x,
                transform.position.y,
                _target.transform.position.z + _hotkeyChangeZCameraPosition
                );

            transform.position = Vector3.Lerp(
                transform.position,
                position,
                Time.deltaTime * _followSpeed
            );
        }

        private void Move(float x, float z)
        {
            if (x + z != 0)
                _follow = false;
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
            transform.Rotate(0, Input.GetAxis(Stuff.MOUSEX) * _rotateSpeed, 0);
        }

        private void Zooming()
        {
            var newFow = Mathf.Clamp(Camera.main.fieldOfView - Input.mouseScrollDelta.y, _minFOV, _maxFOV);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, newFow, Time.deltaTime * _changeFOVSpeed);
        }
    }
}