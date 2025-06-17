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
    public Transform player; // ��ҧ�ԧ�֧ GameObject �ͧ������
    public float wallCheckDistance = 5f; // ���з����ͧ���礡�ᾧ
    public LayerMask wallLayer; // ��˹� Layer �ͧ��ᾧ�����ҵ�ͧ��èЫ�͹

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
        originalCullingMask = mainCamera.cullingMask; // �纤�� Culling Mask ����ͧ���ͧ���
        wallLayerMask = wallLayer.value;
    }

    private void Update()
    {
        UpdateCameraDistance();

        //// ��Ǩ�ͺ����ա�ᾧ���������ҧ���ͧ�Ѻ�������������

        //bool wallOccluding = Physics.Raycast
        //    (virtualCamera.transform.position, 
        //    (player.position - virtualCamera.transform.position).
        //    normalized, out RaycastHit hit, wallCheckDistance, wallLayer);

        //if (wallOccluding)
        //{
        //    // ����ա�ᾧ�ѧ ����繡�ᾧ� Layer �����ҵ�ͧ���
        //    // ������ᾧ� Layer ������١�Ҵ (�¡��ź Layer ����͡�ҡ Culling Mask)
        //    mainCamera.cullingMask = originalCullingMask & ~wallLayerMask;
        //    //mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Enviroment"));
            
        //}
        //else
        //{
        //    // �������ա�ᾧ�ѧ ���͡�ᾧ���ѧ���������� Layer ����˹�
        //    // �׹��� Culling Mask �ͧ���ͧ��Ѻ�
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
        //�礤��absolute�ͧ���С��ͧ-currentDistance ����ҡ����0.1��������Ѻ������ҧ���ͧ
        //���ж�һ�Ѻ����VirtualCamera��Ҩ���������ʹ�����Ҩ��������ԧ��
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

        float height = bounds.size.y;  // �����٧
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
