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

                #region Edge Draw
                if(Mathf.RoundToInt(i / size) == 0 || Mathf.RoundToInt(i / size) == 5)
                {
                    if(Mathf.RoundToInt(k / size) == 0 || Mathf.RoundToInt(k / size) == 5)
                    {
                        texture.SetPixel(i, k, colorGradient.GetColor(pivot[y][x]));
                    }
                    else
                    {
                        var yy = Mathf.RoundToInt(k / size);

                        var l0 = Mathf.Lerp(pivot[yy - 1][x], pivot[yy][x], Mathf.InverseLerp(30 + 60 * (yy - 1), 30 + 60 * (yy), k));
                        texture.SetPixel(i, k, colorGradient.GetColor(l0));
                    }
                    continue;
                }

                if(Mathf.RoundToInt(k / size) == 0 || Mathf.RoundToInt(k / size) == 5)
                {
                    if (Mathf.RoundToInt(i / size) != 0 && Mathf.RoundToInt(i / size) != 5)
                    {
                        var xx = Mathf.RoundToInt(i / size);

                        var l0 = Mathf.Lerp(pivot[y][xx - 1], pivot[y][xx], Mathf.InverseLerp(30 + 60 * (xx - 1), 30 + 60 * (xx), i));
                        texture.SetPixel(i, k, colorGradient.GetColor(l0));
                    }
                    continue;
                }
                #endregion


            }
        }
    }
}