using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeFollowCam : MonoBehaviour
{
    public Transform player;
    public float coneAngle = 60f;

    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (material != null && player != null)
        {
            // กำหนดตำแหน่งของผู้เล่นให้ Shader
            material.SetVector("_PlayerPosition", player.position);

            // ทิศทางที่ผู้เล่นหัน (ใน Topdown ใช้ Forward เป็น Vector3)
            Vector3 coneDir = player.forward;
            material.SetVector("_ConeDirection", coneDir.normalized);

            // กำหนดมุมของกรวย
            material.SetFloat("_ConeAngle", coneAngle);

            // หมุน Cone ให้หมุนตาม Player
            transform.position = player.position + Vector3.up * 0.1f;
            transform.rotation = Quaternion.LookRotation(coneDir, Vector3.up);
        }
    }
}
