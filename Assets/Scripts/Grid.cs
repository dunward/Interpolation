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

            case InterpolationType.Bicubic:
                texture = Bicubic();
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

                var rx = Mathf.RoundToInt(i / size);
                var ry = Mathf.RoundToInt(k / size);

                #region Edge Draw
                if(rx == 0 || rx == 5)
                {
                    if(ry == 0 || ry == 5)
                    {
                        texture.SetPixel(i, k, colorGradient.GetColor(pivot[y][x]));
                    }
                    else
                    {
                        var l = Mathf.Lerp(pivot[ry - 1][x], pivot[ry][x], Mathf.InverseLerp(30 + 60 * (ry - 1), 30 + 60 * (ry), k));
                        texture.SetPixel(i, k, colorGradient.GetColor(l));
                    }
                    continue;
                }

                if(ry == 0 || ry == 5)
                {
                    if (rx != 0 && rx != 5)
                    {
                        var l = Mathf.Lerp(pivot[y][rx - 1], pivot[y][rx], Mathf.InverseLerp(30 + 60 * (rx - 1), 30 + 60 * (rx), i));
                        texture.SetPixel(i, k, colorGradient.GetColor(l));
                    }
                    continue;
                }
                #endregion
                
                var l0 = Mathf.Lerp(pivot[ry - 1][rx - 1], pivot[ry - 1][rx], Mathf.InverseLerp(30 + 60 * (rx - 1), 30 + 60 * (rx), i));
                var l1 = Mathf.Lerp(pivot[ry][rx - 1], pivot[ry][rx], Mathf.InverseLerp(30 + 60 * (rx - 1), 30 + 60 * (rx), i));
                var l2 = Mathf.Lerp(l0, l1, Mathf.InverseLerp(30 + 60 * (ry - 1), 30 + 60 * (ry), k));
                texture.SetPixel(i, k, colorGradient.GetColor(l2));
            }
        }
    }

    private Texture2D Bicubic()
    {
        Texture2D temp = new Texture2D(width - 60, height - 60);

        for (int i = 0; i < temp.width; i++)
        {
            for (int k = 0; k < temp.height; k++)
            {
                var dx = Mathf.Repeat(i / size, 1);
                var dy = Mathf.Repeat(k / size, 1);

                var x = Mathf.FloorToInt(i / size);
                var y = Mathf.FloorToInt(k / size);

                var rx = Mathf.RoundToInt(i / size);
                var ry = Mathf.RoundToInt(k / size);

                float l0, l1, l2, l3;

                l0 = CubicInterpolation(
                    pivot[Mathf.Clamp(y - 1, 0, pivot.Count - 1)][Mathf.Clamp(x - 1, 0, pivot.Count - 1)],
                    pivot[y][Mathf.Clamp(x - 1, 0, pivot.Count - 1)],
                    pivot[Mathf.Clamp(y + 1, 0, pivot.Count - 1)][Mathf.Clamp(x - 1, 0, pivot.Count - 1)],
                    pivot[Mathf.Clamp(y + 2, 0, pivot.Count - 1)][Mathf.Clamp(x - 1, 0, pivot.Count - 1)], dy);
                l1 = CubicInterpolation(
                    pivot[Mathf.Clamp(y - 1, 0, pivot.Count - 1)][Mathf.Clamp(x, 0, pivot.Count - 1)],
                    pivot[y][Mathf.Clamp(x, 0, pivot.Count - 1)],
                    pivot[Mathf.Clamp(y + 1, 0, pivot.Count - 1)][Mathf.Clamp(x, 0, pivot.Count - 1)],
                    pivot[Mathf.Clamp(y + 2, 0, pivot.Count - 1)][Mathf.Clamp(x, 0, pivot.Count - 1)], dy);
                l2 = CubicInterpolation(
                    pivot[Mathf.Clamp(y - 1, 0, pivot.Count - 1)][Mathf.Clamp(x + 1, 0, pivot.Count - 1)],
                    pivot[y][Mathf.Clamp(x + 1, 0, pivot.Count - 1)],
                    pivot[Mathf.Clamp(y + 1, 0, pivot.Count - 1)][Mathf.Clamp(x + 1, 0, pivot.Count - 1)],
                    pivot[Mathf.Clamp(y + 2, 0, pivot.Count - 1)][Mathf.Clamp(x + 1, 0, pivot.Count - 1)], dy);
                l3 = CubicInterpolation(
                    pivot[Mathf.Clamp(y - 1, 0, pivot.Count - 1)][Mathf.Clamp(x + 2, 0, pivot.Count - 1)],
                    pivot[y][Mathf.Clamp(x + 2, 0, pivot.Count - 1)],
                    pivot[Mathf.Clamp(y + 1, 0, pivot.Count - 1)][Mathf.Clamp(x + 2, 0, pivot.Count - 1)],
                    pivot[Mathf.Clamp(y + 2, 0, pivot.Count - 1)][Mathf.Clamp(x + 2, 0, pivot.Count - 1)], dy);

                var t = CubicInterpolation(l0, l1, l2, l3, dx);
                temp.SetPixel(i, k, colorGradient.GetColor(t));
            }
        }

        temp.Apply();

        return temp;
    }

    private float CubicInterpolation(float p0, float p1, float p2, float p3, float t)
    {
        float a0 = -0.5f * p0 + 1.5f * p1 - 1.5f * p2 + 0.5f * p3;
        float a1 = p0 - 2.5f * p1 + 2 * p2 - 0.5f * p3;
        float a2 = -0.5f * p0 + 0.5f * p2;
        float a3 = p1;
        return a0 * t * t * t + a1 * t * t + a2 * t + a3;
    }
}