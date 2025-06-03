using UnityEngine;

public class Car_Interaction : Interactable
{
    private Car_HealthController healthController;
    private Car_Controller car;
    private Transform player;
    private float defaultPlayerScale;

    [Header("ExitDetail")]
    [SerializeField] private float exitCheckRadius;
    [SerializeField] private LayerMask whatToIgnoreForExitCar;
    [SerializeField] private Transform[] exitPoints;
    private void Start()
    {
        healthController = GetComponent<Car_HealthController>();
        car = GetComponent<Car_Controller>();
        player = GameManager.instance.player.transform; 
        foreach(var point in exitPoints )
        {
            point.GetComponent<MeshRenderer>().enabled = false;
            point.GetComponent<SphereCollider>().enabled = false;
        }
        
    }
    public override void InterAction()
    {
        base.InterAction();
        if(healthController.CarBroken())
        {
            if(CheckIfPlayerHasorbToFixCar())
            {
                healthController.FixCar();
                weaponController.RemoveWeaponFromWeaponSlot(WeaponType.Orb);
            }
        }
        GetInToCar();
    }
    private bool CheckIfPlayerHasorbToFixCar()
    {
        Weapon orb = weaponController.WeaponInInventory(WeaponType.Orb);
        if (orb != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void GetInToCar()
    {
        ConTrolManager.instance.SwitchToCarConTrols();
        healthController.UpdateCarHealthUI();
        car.ActivateCar(true);

        defaultPlayerScale = player.localScale.x;

        player.localScale = new Vector3(.01f, .01f, .01f);
        player.transform.parent = transform;
        player.transform.localPosition = Vector3.up /2;

        CameraManager.instance.ChangeCameraTarget(transform,8,.5f);
    }
    public void GetOutCar()
    {
        if(car.carActive == false)
        {
            return;
        }
        car.ActivateCar(false);


        player.parent = null;
        player.position = GetExitPoint();
        player.transform.localScale 
            = new Vector3(defaultPlayerScale,defaultPlayerScale,defaultPlayerScale);
        ConTrolManager.instance.SwitchToCharacterControls();
        CameraManager.instance.ChangeCameraTarget(GameManager.instance.player.aim.GetAimCameraTarget()); 
    }
    private Vector3 GetExitPoint()
    {
        for(int i = 0; i < exitPoints.Length; i++)
        {
            if(IsExitClear(exitPoints[i].position))
            {
                return exitPoints[i].position;
            }
        }
        return exitPoints[0].position;
    }
    private bool IsExitClear(Vector3 point)
    {
        Collider[] colliders  
            = Physics.OverlapSphere(point,exitCheckRadius, ~whatToIgnoreForExitCar);
        return colliders.Length == 0;
    }
    private void OnDrawGizmos()
    {
        if(exitPoints.Length > 0)
        {
            foreach(var point in exitPoints)
            {
                Gizmos.DrawWireSphere(point.position,exitCheckRadius);
            }
        }
    }
}
