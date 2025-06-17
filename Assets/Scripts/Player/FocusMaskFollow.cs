using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusMaskFollow : MonoBehaviour
{
    public Transform target;       // ผู้เล่น
    public Camera blurCam;        // BlurCam
    public RectTransform maskRect;// RawImage RectTransform
    public Vector2 maskSize = new Vector2(400, 300); // ขนาดกรวย

    void Start()
    {
        if (maskRect != null)
        {
            maskRect.sizeDelta = maskSize;
        }
    }

    void LateUpdate()
    {
        if (target == null || blurCam == null || maskRect == null)
            return;

        Vector3 screenPos = blurCam.WorldToScreenPoint(target.position);
        maskRect.anchoredPosition = screenPos - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
    }
}
