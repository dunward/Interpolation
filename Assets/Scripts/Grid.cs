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
                Nearest(texture);
                break;

            case InterpolationType.Bilinear:
                Bilinear(texture);
                break;
        }
        texture.Apply();
        GetComponent<RawImage>().texture = texture;
    }

    private void Nearest(Texture2D texture)
    {
        for (int i = 0; i < width; i++)
        {
            for (int k = 0; k < height; k++)
            {
                texture.SetPixel(i, k, colorGradient.GetColor(pivot[Mathf.FloorToInt(k / size)][Mathf.FloorToInt(i / size)]));
            }
        }
    }

    private void Bilinear(Texture2D texture)
    {
        for (int i = 0; i < width; i++)
        {
            for (int k = 0; k < height; k++)
            {
                var dx = Mathf.Repeat(i / size, 1);
                var dy = Mathf.Repeat(k / size, 1);

                var x = Mathf.FloorToInt(i / size);
                var y = Mathf.FloorToInt(k / size);

                try
                {

                    var t0 = Mathf.Lerp(pivot[y][x], pivot[y][x + 1], dx);
                    var t1 = Mathf.Lerp(pivot[y + 1][x], pivot[y + 1][x + 1], dx);

                    var t2 = Mathf.Lerp(t0, t1, dy);
                    texture.SetPixel(i, k, colorGradient.GetColor(t0));
                }
                catch
                {

                }
            }
        }
    }
}