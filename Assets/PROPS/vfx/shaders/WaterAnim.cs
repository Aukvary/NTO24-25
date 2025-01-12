using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnim : MonoBehaviour
{
    public Material MyMaterial;

    public Vector2 newOffset;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        newOffset.y += speed * Time.deltaTime;
        if (newOffset.y > 1f)
        {
            newOffset.y -= 1f;
        }
        if (newOffset.y < 1f)
        {
            newOffset.y += 1f;
        }
        MyMaterial.mainTextureOffset = newOffset;
    }
}
