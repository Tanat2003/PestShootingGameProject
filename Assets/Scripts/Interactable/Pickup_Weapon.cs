using UnityEngine;

public class Pickup_Weapon : Interactable
{

    [SerializeField] private WeaponData[] weaponData;
    [SerializeField] private BackupWeaponModels[] models;
    [SerializeField] private Weapon[] weapon;
    private int index;

    private bool oldWeapon;

    private void OnEnable()
    {
        RandomWeaponSetUp();
    }

    private void RandomWeaponSetUp()
    {
        
        if (oldWeapon == false)
        {
            index = Random.Range(0, weaponData.Length-1);
            weapon[index] = new Weapon(weaponData[index]);
            weapon[index].weaponType = weaponData[index].weaponType;
            SetupGameObject();
        }

    }
    public void SetupPickupWeapon(Weapon weapon, Transform transform)
    {
        oldWeapon = true;
        this.weapon[index] = weapon;
        weaponData[index] = weapon.weaponData;
        this.transform.position = transform.position + new Vector3(0, 0.75f, 0);
        SetupGameObject();
    }

    [ContextMenu("Update Item Model")]
    public void SetupGameObject()
    {
        gameObject.name = "Pickup_Weapon - " + weaponData[index].weaponType.ToString();
        SetupWeaponPickupModel();
    }
    private void SetupWeaponPickupModel()
    {
        foreach (BackupWeaponModels model in models)
        {
            model.gameObject.SetActive(false);
            if (model.weapontype == weaponData[index].weaponType)
            {
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
            }
        }
    }
    public override void InterAction()
    {
        weaponController.PickupWeapon(weapon[index]);
        GameDataManager.instance.ItemCollected("»×¹ " + weaponData[index].weaponName, Mission_Manager.instance.currentMission.missionName);
        Object_Pool.instance.ReturnObject(gameObject);

    }



}
