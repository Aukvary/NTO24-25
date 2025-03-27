using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24
{
    public class ResourceCluster : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _objects;

        [SerializeField]
        private float _range;

        private List<DropController> _spawners;

        private void Awake()
        {
            _spawners = _objects.Select(x => x.GetComponentInChildren<DropController>(true)).ToList();
            print(_spawners.Count);
        }

        public Resource Spawn(int seed, int pos, int circleCount,
            List<Resource> deniedResources, System.Random random)
        {
            foreach (var res in deniedResources)
            {
                var s = _spawners.Where(s => s.Resources.Any(r => (Resource)r.Value1 == res));
                for (int i = s.Count() - 1; i >= 0; i--)
                {
                    _spawners.Remove(s.ElementAt(i));
                }
            }

            if (_spawners.Count == 0)
                return null;
            var spawner = _spawners[(seed / pos) % _spawners.Count];
            for (int i = 1, j = 4; i <= circleCount; i++, j--)
            {
                for (int k = 0; k < j; k++)
                {
                    var xPos = transform.position.x + Mathf.Cos(random.Next(0, 360)) * _range * i;
                    var zPos = transform.position.z + Mathf.Sin(random.Next(0, 360)) * _range * i;

                    Quaternion angle = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                    DropController resource = null;
                    Instantiate(spawner, new(xPos, transform.position.y, zPos), angle);
                    if (spawner.gameObject.activeSelf)
                    {
                        var s = Instantiate(spawner, new(xPos, transform.position.y, zPos), angle);
                        resource.transform.parent = transform;
                    }
                    else
                    {
                        var s = Instantiate(spawner.transform.parent, new(xPos, transform.position.y, zPos), angle);
                        s.parent = transform;
                    }
                    //resource.transform.parent = transform;
                }
            }
            return (Resource)spawner.Resources.First().Value1;
        }
    }
}