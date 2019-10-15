using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    public InterpolationType type;

    public List<Pivot> pivot;
    public ColorGradient colorGradient;

    private int width = 300;
    private int height = 300;

    private float size = 60;

    private void Update()
    {
        Draw();
    }

    private void Draw()
    {
        var texture = new Texture2D(width, height);
        switch (type)
        {
            case InterpolationType.Nearest:
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            texture.SetPixel(i, k, colorGradient.GetColor(pivot[Mathf.FloorToInt(k / size)][Mathf.FloorToInt(i / size)]));
                        }
                    }
                }
                break;
        }
        texture.Apply();
        GetComponent<RawImage>().texture = texture;
    }
}