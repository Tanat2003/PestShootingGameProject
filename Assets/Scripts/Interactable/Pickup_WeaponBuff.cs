using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;


public enum BuffType {Attack,BulletInMagazine}
public class Pickup_WeaponBuff : Interactable
{
    [SerializeField] private WeaponData[] weaponData;
    [SerializeField] private BackupWeaponModels[] models;
    [SerializeField] private ParticleSystem attackBuff;
    [SerializeField] private ParticleSystem bulletInMagazineBuff;
    [Header("Weapon")]
    [SerializeField] private Weapon weapon;

    [SerializeField] private BuffType buffType;
    private string buffName; //‰«È„™È‚™«ÏµÕπ¥Ÿ ∂‘µ‘

    private int index = 0;
    private void OnEnable()
    {
        RandomBuff();
        buffName = buffType ==
            BuffType.Attack ? "‡ √‘¡¥“‡¡®" + weaponData[index].weaponName : "‡æ‘Ë¡·¡°°“´’π" + weaponData[index].weaponName;
    }
    //private void Start()
    //{

    //    weapon.weaponType = weaponData[index].weaponType;

    //    SetupGameObject();
    //    if (buffType == BuffType.Attack)
    //        attackBuff.Play();
    //    if (buffType == BuffType.BulletInMagazine)
    //        bulletInMagazineBuff.Play();
    //    buffName = buffType ==
    //        BuffType.Attack ? "‡ √‘¡¥“‡¡®" + weaponData[index].weaponName : "‡æ‘Ë¡·¡°°“´’π" + weaponData[index].weaponName;

    //} // fortesting

    private void RandomBuff()
    {
        if(Random.Range(0,2) ==0)
        {
            buffType = BuffType.Attack;
        }else
        {
            buffType = BuffType.BulletInMagazine;
        }
        if (buffType == BuffType.Attack)
            attackBuff.Play();
        if (buffType == BuffType.BulletInMagazine)
            bulletInMagazineBuff.Play();
        index = Random.Range(0, 5);
        weapon.weaponType = weaponData[index].weaponType;
        SetupGameObject();




    }

    [ContextMenu("Update Item Model")]
    public void SetupGameObject()
    {
        if(buffType == BuffType.Attack)
        {
            gameObject.name = "UpgradeAttack " + weaponData[index].weaponType.ToString();

        }
        if(buffType == BuffType.BulletInMagazine)
        {
            gameObject.name = "UpgradeMagazine" + weaponData[index].weaponType.ToString();
        }
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
        base.InterAction();
        
        if(buffType == BuffType.Attack)
        {
            weaponController.UpgradeAttackWeapon(weapon);

        }
        if(buffType==BuffType.BulletInMagazine)
        {
            weaponController.UpgradeMagazineCapacity(weapon);
        }
        GameDataManager.instance.ItemCollected(buffName, Mission_Manager.instance.currentMission.missionName);
        Object_Pool.instance.ReturnObject(gameObject);


    }
    

}
