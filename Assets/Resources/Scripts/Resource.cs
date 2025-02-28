using UnityEngine;

namespace NTO24
{
    [CreateAssetMenu(fileName = "NewResource", menuName = "Resources", order = 51)]
    public class Resource : ScriptableObject
    {
        [field: SerializeField]
        public string ResourceName { get; private set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }
    }
}
