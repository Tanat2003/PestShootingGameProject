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
            // ��˹����˹觢ͧ��������� Shader
            material.SetVector("_PlayerPosition", player.position);

            // ��ȷҧ���������ѹ (� Topdown �� Forward �� Vector3)
            Vector3 coneDir = player.forward;
            material.SetVector("_ConeDirection", coneDir.normalized);

            // ��˹�����ͧ����
            material.SetFloat("_ConeAngle", coneAngle);

            // ��ع Cone �����ع��� Player
            transform.position = player.position + Vector3.up * 0.1f;
            transform.rotation = Quaternion.LookRotation(coneDir, Vector3.up);
        }
    }
}
