using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionMaskFollow : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    private RectTransform maskRT;
    private Camera mainCam;

    void Start()
    {
        maskRT = GetComponent<RectTransform>();
        mainCam = playerCamera.GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 screenPos = mainCam.WorldToScreenPoint(playerCamera.position);
        maskRT.position = screenPos;
        maskRT.rotation = Quaternion.identity;
    }

}
