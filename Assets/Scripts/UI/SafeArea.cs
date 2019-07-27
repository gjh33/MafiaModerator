using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    public GameObject OptionalScreenCover;

    private RectTransform rt;

    private void OnEnable()
    {
        if (OptionalScreenCover != null) OptionalScreenCover.SetActive(true);
        rt = GetComponent<RectTransform>();
        StartCoroutine(PostCameraInitialize());
    }

    public void ResizeToSafeArea()
    {
        Rect r = Screen.safeArea;

        Vector2 anchorMin = Vector2.zero;
        Vector2 anchorMax = Vector2.zero + r.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
    }

    IEnumerator PostCameraInitialize()
    {
        yield return new WaitForEndOfFrame();
        ResizeToSafeArea();
        if (OptionalScreenCover != null) OptionalScreenCover.SetActive(false);
    }
}
