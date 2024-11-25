using UnityEngine;

public class MaterialCell : MonoBehaviour
{
    private SpriteRenderer _render;

    private TextMesh _textMesh;

    private void Awake()
    {
        _render = GetComponent<SpriteRenderer>();
        _textMesh = GetComponentInChildren<TextMesh>();
    }

    public void SetMaterial(Resource resource, uint count)
    {
        _render.sprite = resource.Sprite;
        _textMesh.text = count.ToString();
    }
}