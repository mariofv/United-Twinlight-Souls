using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialExtension
{
    public static void SetAlpha(this Material material, float alpha)
    {
        Color materialColor = material.color;
        materialColor.a = alpha;
        material.color = materialColor;
    }
}
