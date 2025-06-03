using Cinemachine;
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
    private void Update()
    {
        UpdateCameraDistance();

    }

    private void UpdateCameraDistance()
    {
        if (canChangeCameraDistance==false)
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


    //ฟังชั่นนี้ใช้เซ็ทระยะห่างของกล้อง
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








}
