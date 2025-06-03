using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponSelectedWindow : MonoBehaviour
{
    public WeaponData weaponData;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponDetail;
  


    private void Start()
    {
        UpdateSlotInfo(null);
      
        
    }

    public void SetWeaponSlot(WeaponData newWeaponData)
    {
        weaponData = newWeaponData;
        UpdateSlotInfo(newWeaponData);
    }
    public void UpdateSlotInfo(WeaponData weapon_Data)
    {
        if (weapon_Data == null)
        {
            weaponIcon.color = Color.clear;
            weaponDetail.text = "เลือกอาวุธได้เลย";
            return;
        }
        weaponIcon.color = Color.white;
        weaponIcon.sprite = weapon_Data.weaponIcon;
        weaponDetail.text = weapon_Data.weaponInfo;
    }
    public bool IsSlotEmpyty() => weaponData == null;
}
