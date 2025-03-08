using UnityEngine;

namespace NTO24
{
    public class MeshGroup : MonoBehaviour
    {
        private void Start()
        {
            var meshFilters = GetComponentsInChildren<MeshFilter>();
            var combine  = new CombineInstance[meshFilters.Length];

            for (int i = 0; i < meshFilters.Length; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }

            var meshFilter = GetComponent<MeshFilter>();

            meshFilter.mesh.CombineMeshes(combine);


            GetComponent<Renderer>().sharedMaterial = transform.GetChild(0).GetComponentInChildren<Renderer>().sharedMaterial;
    
             foreach (var child in meshFilters)
		        child.GetComponentInChildren<Renderer>().enabled = false;
        }
    }
}