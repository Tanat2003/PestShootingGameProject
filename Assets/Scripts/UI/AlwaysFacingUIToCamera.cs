using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFacingUIToCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        Camera.onPreRender += UpdateBillboard; // ���¡��੾�С�͹�ù����
    }

    void OnDestroy()
    {
        Camera.onPreRender -= UpdateBillboard; 
    }

    void UpdateBillboard(Camera cam)
    {
        if (cam == mainCamera)
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                             cam.transform.rotation * Vector3.up);
        }
    }
}
