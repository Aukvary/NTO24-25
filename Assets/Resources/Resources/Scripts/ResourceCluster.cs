using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24
{
    public class ResourceCluster : MonoBehaviour
    {
        [SerializeField]
        private List<DropController> _spawners;

        [SerializeField]
        private float _range;

        private void Awake()
        {
            Spawn(574, 2, 4);
        }

        public Resource Spawn(int seed, int pos, int circleCount, List<Resource> deniedResources)
        {
            foreach (var res in deniedResources)
            {
                _spawners.Remove(_spawners.FindAll(s => s.));
            }
            var spawner = _spawners[seed %  pos];
            var random = new System.Random(seed);
            for (int i = 1, j = 4; i <= 4;  i++, j--)
            {
                for (int k = 0; k < j; k++)
                {
                    var xPos = transform.position.x + Mathf.Cos(random.Next(0, 360)) * _range * i;
                    var zPos = transform.position.z + Mathf.Sin(random.Next(0, 360)) * _range * i;

                    var resource = Instantiate(spawner, new(xPos, transform.position.y, zPos), Quaternion.identity);
                    resource.transform.parent = transform;
                }
            }
            return ((Resource)spawner.GetComponent<DropController>().Resources.First().Value1);
        }
    }
}