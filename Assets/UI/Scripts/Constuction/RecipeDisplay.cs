using System.Linq;
using UnityEngine;

public class RecipeDisplay : MonoBehaviour
{
    [SerializeField]
    private float _xCount;

    [SerializeField]
    private float _yCount;

    [SerializeField]
    private float _offset;

    private ConstructionObject _constructionObject;

    private MaterialCell[] _materialCells;

    private void Awake()
    {
        _constructionObject = GetComponentInParent<ConstructionObject>();
        var cell = GetComponentInChildren<MaterialCell>();

        _materialCells = new MaterialCell[_constructionObject.Materials.Count()];

        for (int i = 0, x = 0, y = 0; i < _materialCells.Length; i++, x++)
        {
            if (x > _xCount)
            {
                y++;
                x = 0;
            }
            
            _materialCells[i] = Instantiate(cell , transform);
            _materialCells[i].transform.position = cell.transform.position + new Vector3(x * _offset, y * _offset);
        }
        Destroy(cell.gameObject);
        int c = 0;
        foreach (var material in _constructionObject.Materials)
        {
            _materialCells[c].SetMaterial(material.Resource, material.Count);
            c++;
        }
    }
}