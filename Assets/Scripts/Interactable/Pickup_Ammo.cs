using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct AmmoData //struct ��Datastruc����纵���÷�������纤�Ң�ҧ��ա����(�纵���÷���Ң�ҧ�����¹�ŧ��)
{
    public WeaponType WeaponType;//�������׹
    [Range(10, 100)]
    public int minAmount;
    [Range(10, 100)]
    public int maxAmount;
}

public enum AmmoBoxType { smallBox,bigBox}
public class Pickup_Ammo : Interactable
{
    
    [SerializeField] private AmmoBoxType boxType;
    
    [SerializeField] private List<AmmoData> smallBoxAmmo;
    [SerializeField] public List<AmmoData> bigBoxAmmo;
    [SerializeField] private GameObject[] boxModel;
    private string boxName; // ������ʴ��������ʶԵ�
    private void Start()
    {
        boxName = boxType == AmmoBoxType.smallBox ? "����ع���ͧ���" : "����ع���ͧ�˭�";
        SetupBoxModel();

    }

    private void OnEnable()
    {
        RandomBoxModel();
        boxName = boxType == AmmoBoxType.smallBox ? "����ع���ͧ���" : "����ع���ͧ�˭�";
    }
    private void RandomBoxModel()
    {
        boxType = 
            (AmmoBoxType)
            Random.Range(0, System.Enum.GetValues(typeof(AmmoBoxType)).Length);
        SetupBoxModel();




    }
    private void SetupBoxModel()
    {

        for (int i = 0; i < boxModel.Length; i++)
        {
            if (i == ((int)boxType))
            {
                boxModel[i].SetActive(true);
                UpdateMeshAndMaterial(boxModel[i].GetComponent<MeshRenderer>());
            }
        }
    }

    public override void InterAction()
    {
       List<AmmoData> currentAmmoList = smallBoxAmmo;
        if(boxType == AmmoBoxType.bigBox)
        {
            currentAmmoList = bigBoxAmmo;

                
        }
        foreach(AmmoData ammo in currentAmmoList)
        {
            Weapon weapon =weaponController.WeaponInInventory(ammo.WeaponType);
            AddBulletsToWeapon(weapon,GetBulletAmount(ammo));
            weaponController.UpdateWeaponUI();
        }
        GameDataManager.instance.ItemCollected(boxName, Mission_Manager.instance.currentMission.missionName);
        Object_Pool.instance.ReturnObject(gameObject);

    }

    private void AddBulletsToWeapon(Weapon weapon, int amountBullet)
    {
        if (weapon == null)
        { return; }
        weapon.totalReserveAmmo += amountBullet;


    }
   
    private int GetBulletAmount(AmmoData ammo)
    {
        //mathf.min������ҷ����·���ش�ҡ�ͧ���
        //mathf.max��������ҡ����ش�ҡ�ͧ���
        //�ѹ�˹������������Ѻ�Ţmax�Ѻmin�inspector
        float min = Mathf.Min(ammo.minAmount,ammo.maxAmount);
        float max = Mathf.Max(ammo.minAmount,ammo.maxAmount);

        float randomAmmoAmount = Random.Range(min,max);
        return Mathf.RoundToInt(randomAmmoAmount); //��ŧ��ҷȹ�������繵���Ţ  
    }
    private void OnDisable()
    {
        foreach (GameObject model in boxModel)
        {
            model.SetActive(false);
        }
    }

}
