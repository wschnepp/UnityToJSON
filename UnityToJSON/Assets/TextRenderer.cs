using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TextRenderer : MonoBehaviour
{
    public string text;
    public int fontIndex;
    public int fontSize;
    public Color textColor;

    public float hres;

    public int vres;

    GameObject childPlane = null;

    public void OnValidate()
    {
        childPlane = transform.GetChild(0)?.gameObject;

        if (childPlane)
        {
            var textAspectRatio = hres / (float)vres;
            const float meshScale = 10; //unity default plane is 10*10 meters by default
            const float onePoint = 0.352f * 10e-3f; // one point == 1/72 inch or 0.35 mm
            float worldFontSize = (fontSize * onePoint) / meshScale;


            var verticalSize = vres * worldFontSize;
            var scale = new Vector3(0, 1, 0);
            scale.x = !float.IsNaN(textAspectRatio) ? textAspectRatio * verticalSize : 0;
            scale.z = verticalSize;

            childPlane.transform.localScale = scale;
        }
    }
}
