using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ClampToEdge : MonoBehaviour
{
    [Header("Clamp To:")]
    public bool Top;
    public bool Bottom;
    public bool Left;
    public bool Right;

    private RectTransform rt;

    private void OnEnable()
    {
        rt = GetComponent<RectTransform>();
        StartCoroutine(PostCameraInitialize());
    }

    public void Clamp()
    {
        Vector2 dSize = Vector2.zero;
        Vector2 dPos = Vector2.zero;
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        if (Top)
        {
            float curTop = corners[1].y;
            float targetTop = Screen.height;
            float dheight = targetTop - curTop;
            dSize.y += dheight;
            dPos.y += dheight / 2;
        }
        if (Bottom)
        {
            float curBot = corners[0].y;
            dSize.y += curBot;
            dPos.y -= curBot / 2;
        }
        if (Left)
        {
            float curLeft = corners[0].x;
            dSize.x += curLeft;
            dPos.x -= curLeft / 2;
        }
        if (Right)
        {
            float curRight = corners[2].x;
            float targetRight = Screen.width;
            float dwidth = targetRight - curRight;
            dSize.x += dwidth;
            dPos.x += dwidth / 2;
        }

        rt.sizeDelta += (Vector2)rt.InverseTransformVector(dSize);
        rt.anchoredPosition += (Vector2)rt.InverseTransformVector(dPos);
    }

    IEnumerator PostCameraInitialize()
    {
        yield return new WaitForEndOfFrame();
        Clamp();
    }
}
