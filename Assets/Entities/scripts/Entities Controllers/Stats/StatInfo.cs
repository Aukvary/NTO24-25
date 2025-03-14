using UnityEngine;

namespace NTO24
{
    [CreateAssetMenu(fileName = "New Stats", menuName = "Stats Types")]
    public class StatInfo : ScriptableObject
    {
        [field: SerializeField]
        public StatNames Type { get; private set; }

        [field: SerializeField]
        public string Title { get; private set; }

        [field: SerializeField, TextArea]
        public string Description { get; private set; }

        [field: SerializeField]
        public Sprite Icon { get; private set; }

        public override string ToString()
            => Type.ToString();
    }
}