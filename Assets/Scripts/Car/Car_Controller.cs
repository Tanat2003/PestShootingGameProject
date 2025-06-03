using System;
using System.IO;
using UnityEngine;

public enum DriveType { FrontWheelDrive, RearWheelDrive, AllWheelDrive }
[RequireComponent(typeof(Rigidbody))]

public class Car_Controller : MonoBehaviour
{
    public Car_Sound carsound {  get; private set; }
    public bool carActive { get; private set; }
    private PlayerControll controls;
    public  Rigidbody rb;
    public float speed;
    private float moveInput;
    private float steerInput;

    public float turnSentivity;
    [Header("Car Engine Setting")]
    public float currentSpeed;
    [Range(2, 10)]
    public float maxSpeed;
    [Range(2, 10)]
    public float increaseSpeedRate;
    [Range(1000, 5000)]
    public float defaultMotorForce = 1000f; //เอาไว้ใช้คำนวนความเกาะถนนของรถ
    private float motorFroce;
    [SerializeField] private DriveType driveType;
    [Header("Car Mass Setting")]
    [Range(350, 1000)]
    [SerializeField] private float carMass;
    [Range(20, 50)]
    [SerializeField] private float wheelsMass = 30;
    [Range(.5f, 2f)]
    [SerializeField] private float backWheelTraction = 1; //แรงฉุดของล้อ
    [Range(.5f, 2f)]
    [SerializeField] private float frontWheelTraction = 1;

    [SerializeField] private Transform centerOfMass;

    [Header("Brake Setting")]
    [Range(1, 10)]
    [SerializeField] private float backBrakeSentivity = 8;
    [Range(1, 10)]
    [SerializeField] private float frontBrakeSentivity = 5;
    public float brakePower = 5000;
    private bool isBrakeing;


    [Header("Drift Setting")]
    private float driftTimer;
    [Range(0, 1)]
    [SerializeField] private float driftDuration = 1f;
    [SerializeField] private float frontDriftFactor = .5f;
    [SerializeField] private float backDriftFactor = .5f;
    private bool isDrifting;


    private Car_Wheel[] wheels;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<Car_Wheel>();
        carsound = GetComponent<Car_Sound>();
        controls = ConTrolManager.instance.controls;
        //ConTrolManager.instance.SwitchToCarConTrols();

        ActivateCar(false);
        SetUpDefaultValue();
        AssignInputEvents();
    }
    private void SetUpDefaultValue()
    {
        motorFroce = defaultMotorForce;
        rb.centerOfMass = centerOfMass.localPosition;
        rb.mass = carMass;
        foreach (var wheel in wheels)
        {
            wheel.collider.mass = wheelsMass;
            if (wheel.axelType == AxelType.Front)
            {
                wheel.SetDefaultStiffnes(frontWheelTraction);
            }
            if (wheel.axelType == AxelType.Back)
            {
                wheel.SetDefaultStiffnes(backDriftFactor);
            }
        }
    }
    private void FixedUpdate() //คล้ายๆupdatae แต่จะเรียกใช้50เฟรมต่อ1วิ ซึ่งถี่กว่าอัพเดทเหมาะกับใช้อัพเดทฟิสิก
    {
        if (carActive == false)
            return;
        ApplyAnimationToWheels();
        ApplyDrive();
        ApplySteering();
        ApplyBrake();
        ApplySpeedLimit();
        if (isDrifting)
        {
            ApplyDrift();
        }
        else
        {
            StopDrift();
        }
    }
    private void Update()
    {
        if (carActive == false)
            return;
        UI.instance.uiInGame.UpdateSpeedText(Mathf.RoundToInt( speed * 10) + "กม./ชม.");
        speed = rb.velocity.magnitude;
        driftTimer -= Time.deltaTime;
        if (driftTimer < 0)
        {
            isDrifting = false;
        }
    }

    private void ApplySteering() //ดริฟ
    {

        foreach (var wheel in wheels)
        {
            if (wheel.axelType == AxelType.Front)
            {
                float targetSteerAngle = steerInput * turnSentivity;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, targetSteerAngle, .5f);
            }
        }
    }

    private void ApplyDrive()
    {
        currentSpeed = moveInput * increaseSpeedRate * Time.deltaTime;

        float motorTorqueValue = motorFroce * currentSpeed;
        foreach (var wheel in wheels)
        {
            if (driveType == DriveType.FrontWheelDrive)
            {
                if (wheel.axelType == AxelType.Front)
                    wheel.collider.motorTorque = motorTorqueValue;//motorTorqueValue ความเกาะถนน
            }
            else if (driveType == DriveType.RearWheelDrive)
            {
                if (wheel.axelType == AxelType.Back)
                    wheel.collider.motorTorque = motorTorqueValue;
            }
            else
            {
                wheel.collider.motorTorque = motorTorqueValue;
            }
        }
    }
    private void ApplySpeedLimit()
    {
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }
    private void ApplyBrake()
    {
        foreach (var wheel in wheels)
        {
            bool frontBrakes = wheel.axelType == AxelType.Front;
            float brakeSensetivity = frontBrakes ? frontBrakeSentivity : backBrakeSentivity;

            float newBrakeTorque = brakePower * brakeSensetivity * Time.deltaTime;
            float currentBrakeTorque = isBrakeing ? newBrakeTorque : 0;

            wheel.collider.brakeTorque = currentBrakeTorque;
        }
    }
    private void ApplyDrift()
    {
        foreach (var wheel in wheels)
        {
            bool frontWheel = wheel.axelType == AxelType.Front;
            float driftFactor = frontWheel ? frontDriftFactor : backDriftFactor;
            WheelFrictionCurve sidewayFriction = wheel.collider.sidewaysFriction;

            sidewayFriction.stiffness *= (1 - driftFactor);
            wheel.collider.sidewaysFriction = sidewayFriction;
        }
    }
    private void StopDrift()
    {
        foreach (var wheel in wheels)
        {
            wheel.ReStoreDefaultStiffnes();
        }
    }
    private void ApplyAnimationToWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rotation;
            Vector3 position;

            wheel.collider.GetWorldPose(out position, out rotation);

            if (wheel.model != null)
            {
                wheel.model.transform.position = position;
                wheel.model.transform.rotation = rotation;
            }
        }
    }
    public void ActivateCar(bool activate)
    {
        carActive = activate;
        if(carsound != null)
        {
            carsound.ActiveCarSFX(activate);
        }
        
    }
    public void  CrashTheCar()
    {
        rb.drag = 1;
        motorFroce = 0;
        isDrifting = true;
        frontDriftFactor = .9f;
        

    }
    public void FixTheCar()
    {
        rb.drag = 0;
        isDrifting = false;
        motorFroce = defaultMotorForce;
        frontDriftFactor = .5f;
        
    }

    private void AssignInputEvents()
    {
        controls.Car.Movement.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();

            moveInput = input.y;
            steerInput = input.x;
        };
        controls.Car.Movement.canceled += ctx =>
        {
            moveInput = 0;
            steerInput = 0;
        };
        controls.Car.Brake.performed += ctx =>
        {

            isBrakeing = true;
            isDrifting = true;
            driftTimer = driftDuration;
        };

        controls.Car.Brake.canceled += ctx => isBrakeing = false;
        

        controls.Car.CarExit.performed += ctx => GetComponent<Car_Interaction>().GetOutCar();

    }
    [ContextMenu("Focus Camera and Enable Car For Testing")]
    public void TestCar()
    {
        ActivateCar(true);
        CameraManager.instance.ChangeCameraTarget(transform, 11);
    }
}
