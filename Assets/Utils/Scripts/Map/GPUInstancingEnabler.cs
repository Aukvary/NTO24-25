using UnityEngine;

namespace NTO24
{
    public class GPUInstancingEnabler : MonoBehaviour
    {
        private void Awake()
        {
            MaterialPropertyBlock propertyBlock = new();

            var meshRenderer = GetComponentInChildren<MeshRenderer>();

            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}