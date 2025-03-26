using System.Linq;
using UnityEngine;

namespace NTO24
{
    public class Bear : Unit, IRestoreable, IIconable, IControllable, IAttacker, IInventoriable,
        IInteractor
    {
        [field: SerializeField]
        public Sprite[] Icon { get; private set; }

        public AttackController AttackController { get; private set; }
        public Inventory Inventory { get; private set; }
        public RestoreController RestoreController { get; private set; }
        public InteractingController InteractingController { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            AttackController = GetComponent<AttackController>();
            Inventory = GetComponent<Inventory>();
            Inventory.Initialize(6, (int)StatsController[StatNames.CellCapacity].StatValue);


            RestoreController = GetComponent<RestoreController>();
            InteractingController = GetComponent<InteractingController>();
        }

        public void Init(BearInfo source)
        {
            if (source == null)
                return;

            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.mainTexture = source.Sprite;
            }

            Icon = new Sprite[] { Sprite.Create(source.Icon, 
                new Rect(new(0f, 0f), new (source.Icon.width, source.Icon.height)), 
                new(0.5f, 0.5f)) };


            foreach (var stat in StatsController._stats)
            {
                try
                {
                    stat._statValues = source.Stats.First(s => s.StatInfo == stat.StatInfo)._statValues;

                }
                catch
                {
                }
            }
        }
    }
}
