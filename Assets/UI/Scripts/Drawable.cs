using UnityEngine;
using UnityEngine.EventSystems;

namespace NTO24.UI
{
    public abstract class Drawable : MonoBehaviour
    {
        protected bool UIRayCast => EventSystem.current.IsPointerOverGameObject();
        protected Ray Direction => Camera.main.ScreenPointToRay(Input.mousePosition);

        public new RectTransform transform { get; private set; }

        protected virtual void Awake() 
        { 
            transform = GetComponent<RectTransform>();
        }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { }
    }
}