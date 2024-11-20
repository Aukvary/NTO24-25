using UnityEngine;
[CreateAssetMenu(fileName = "NewResource", menuName = "Resources", order = 51)]
public class Resource : ScriptableObject
{
    [SerializeField]
    private string _resourceName;
    [SerializeField]
    private Sprite _sprite;

    public string ResourceName => _resourceName;
    public Sprite Sprite => _sprite;
}
