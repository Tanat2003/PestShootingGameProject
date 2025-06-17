using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ��� Cinemachine Virtual Cam ���� Main Camera �ͧ�س
    public Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
        
    }
}
