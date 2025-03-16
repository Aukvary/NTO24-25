using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class PickableObject : Entity, IInteractable
    {
        [SerializeField]
        private Resource _resource;

        private int _count;

        public Interactable Interactable { get; private set; }


        protected override void Awake()
        {
            Interactable = GetComponent<Interactable>();

            Interactable.OnInteractEvent.AddListener(entity =>
            {
                var inventory = entity.EntityReference as IInventoriable;

                inventory.TryAddItems(new(_resource, _count), out var overflowItems);

                _count = overflowItems;

                if (_count == 0)
                    Destroy(gameObject, 0.1f);
            });
        }

        public bool IsInteractable(IInteractor interactor)
            => _count > 0 && interactor.EntityReference is IInventoriable;

        public void Drop(int count, Vector3 spawnPosition, float radius, float strength)
        {
            var item = Instantiate(this, spawnPosition, Quaternion.identity);

            item._count = count;

            var angle = Random.Range(0f, 360f);

            Vector3 direction = new(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                0,
                Mathf.Sin(angle * Mathf.Deg2Rad)
                );

            var endPosition = spawnPosition + direction * radius;

            item.transform.DOJump(endPosition, strength, 1, 0.3f);
        }

        public static explicit operator Resource(PickableObject obj)
            => obj._resource;
    }
}