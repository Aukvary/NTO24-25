using TMPro;
using UnityEngine;

public class DescriptionArea : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;

    [SerializeField] 
    private TextMeshProUGUI _description;

    public string Title
    {
        get => _title.text;
        set => _title.text = value;
    }

    public string Description
    {
        get => _description.text;
        set => _description.text = value;
    }
}