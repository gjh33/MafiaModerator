using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DayNightBackground : MonoBehaviour
{
    public enum BlendMode { Additive, OneMinusAlpha }

    public BlendMode Blend = BlendMode.Additive;
    public Image DaytimeImageUnderlay;
    public Image NightImageOverlay;

    [Range(0, 1)]
    public float Ratio;

    private void Update()
    {
        if (DaytimeImageUnderlay == null || NightImageOverlay == null) return;
        Ratio = Mathf.Clamp01(Ratio);
        Color baseColor = DaytimeImageUnderlay.color;
        if (Blend == BlendMode.OneMinusAlpha)
            baseColor.a = 1 - Ratio;
        else
        {
            baseColor.a = 1;
        }
        DaytimeImageUnderlay.color = baseColor;
        baseColor = NightImageOverlay.color;
        baseColor.a = Ratio;
        NightImageOverlay.color = baseColor;
    }
}
