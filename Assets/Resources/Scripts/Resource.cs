using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewResource", menuName = "Resources", order = 51)]
public class Resource : ScriptableObject
{
    [SerializeField]
    private string _resourceName;
}
