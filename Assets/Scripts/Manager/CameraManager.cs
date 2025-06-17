using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer transposer;
    private float targetCameraDistance;
    [Header("CameraDistance")]
    [SerializeField] private bool canChangeCameraDistance;
    [SerializeField] private float distanceChangeRate = .5f;

    [Header("VisiblelayerCameraSetting")]
    public Transform player; // อ้างอิงถึง GameObject ของผู้เล่น
    public float wallCheckDistance = 5f; // ระยะที่กล้องจะเช็คกำแพง
    public LayerMask wallLayer; // กำหนด Layer ของกำแพงที่เราต้องการจะซ่อน

    private Camera mainCamera;
    private int originalCullingMask;
    private int wallLayerMask;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("You Have more than 1 Camera Manager");
            Destroy(gameObject);
        }

        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        



    }
    private void Start()
    {
        mainCamera = Camera.main;
        originalCullingMask = mainCamera.cullingMask; // เก็บค่า Culling Mask เดิมของกล้องไว้
        wallLayerMask = wallLayer.value;
    }

    private void Update()
    {
        UpdateCameraDistance();

        //// ตรวจสอบว่ามีกำแพงอยู่ระหว่างกล้องกับผู้เล่นหรือไม่

        //bool wallOccluding = Physics.Raycast
        //    (virtualCamera.transform.position, 
        //    (player.position - virtualCamera.transform.position).
        //    normalized, out RaycastHit hit, wallCheckDistance, wallLayer);

        //if (wallOccluding)
        //{
        //    // ถ้ามีกำแพงบัง และเป็นกำแพงใน Layer ที่เราต้องการ
        //    // ทำให้กำแพงใน Layer นี้ไม่ถูกวาด (โดยการลบ Layer นั้นออกจาก Culling Mask)
        //    mainCamera.cullingMask = originalCullingMask & ~wallLayerMask;
        //    //mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Enviroment"));
            
        //}
        //else
        //{
        //    // ถ้าไม่มีกำแพงบัง หรือกำแพงที่บังไม่ได้อยู่ใน Layer ที่กำหนด
        //    // คืนค่า Culling Mask ของกล้องกลับไป
        //    mainCamera.cullingMask = originalCullingMask;
            
           
            
        //}
    }

   







    private void UpdateCameraDistance()
    {
        if (canChangeCameraDistance == false)
        {
            return;
        }
        float currentDistance = transposer.m_CameraDistance;
        //เช็คค่าabsoluteของระยะกล้อง-currentDistance ถ้ามากกว่า0.1ให้ค่อยๆปรับระยะห่างกล้อง
        //เพราะถ้าปรับเลยในVirtualCameraค่าจะวิ่งอยู่ตลอดเวลาอาจทำให้เกมปิงได้
        if (Mathf.Abs(targetCameraDistance - currentDistance) > 0.1f)
        {
            transposer.m_CameraDistance =
                Mathf.Lerp(currentDistance, targetCameraDistance, distanceChangeRate * Time.deltaTime);

        }
    }



    public void ChangeCameraDistance(float distance) => targetCameraDistance = distance;
    public void ChangeCameraTarget(Transform target, float manualDistance = 6f, float newLookAheadTime = 0f, bool autoAdjustCamera = false)
    {
        //transposer.m_LookaheadTime = newLookAheadTime;
        virtualCamera.Follow = target;



        if (autoAdjustCamera == true)
        {
            AdjustCameraByTargetSize(target);

        }
        else
        {
            ChangeCameraDistance(manualDistance);
        }


    }
    private void AdjustCameraByTargetSize(Transform target)
    {
        CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        var rend = target.GetComponentInChildren<Renderer>();
        if (rend == null)
            return;

        Bounds bounds = rend.bounds;

        float height = bounds.size.y;  // ความสูง
        float biggest = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

        float distanceMultiplier = .4f;
        float heightMultiplier = 1.2f;




        framingTransposer.m_CameraDistance = biggest * distanceMultiplier;

        Vector3 trackedOffset = framingTransposer.m_TrackedObjectOffset;
        trackedOffset.y = height * heightMultiplier;
        framingTransposer.m_TrackedObjectOffset = trackedOffset;
    }

    public Transform GetCameraLookAt() => virtualCamera.LookAt;







}
