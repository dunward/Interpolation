using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ColorGradient : MonoBehaviour
{
    public Gradient color;

    private void Update()
    {
        var rawImage = GetComponent<RawImage>();

        var gradientBar = new Texture2D(250, 30);

        for (int i = 0; i < 250; i++)
        {
            for (int k = 0; k < 30; k++)
            {
                gradientBar.SetPixel(i, k, color.Evaluate(Mathf.InverseLerp(0, 250, i)));
            }
        }

        gradientBar.Apply();

        rawImage.texture = gradientBar;
    }

    public Color GetColor(float t)
    {
        return color.Evaluate(t);
    }
}