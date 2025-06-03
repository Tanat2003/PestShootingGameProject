using UnityEngine;

public enum AxelType { Front, Back }
[RequireComponent(typeof(WheelCollider))]
public class Car_Wheel : MonoBehaviour
{
    public AxelType axelType;
    public WheelCollider collider { get; private set; }
    public GameObject model { get; private set; }
    private float defaultSideStiffnes;

    private void Awake()
    {
        collider = GetComponent<WheelCollider>();
        model = GetComponentInChildren<MeshRenderer>().gameObject;
        
      
    }
    public void ReStoreDefaultStiffnes()
    {
        WheelFrictionCurve sidewayFriction = collider.sidewaysFriction;

        sidewayFriction.stiffness = defaultSideStiffnes;
        collider.sidewaysFriction = sidewayFriction;
    }

    public void SetDefaultStiffnes(float newValue)
    {

        defaultSideStiffnes = newValue;
        ReStoreDefaultStiffnes();
    }
}
