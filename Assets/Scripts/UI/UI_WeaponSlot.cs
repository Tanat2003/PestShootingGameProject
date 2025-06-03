using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponSlot : MonoBehaviour
{
    public Image weaponIcon;
    public TextMeshProUGUI ammoText;
    
    
    private void Awake()
    {
        weaponIcon = GetComponentInChildren<Image>();
        ammoText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateWeaponSlot(Weapon myweapon, bool activeWeapon)
    {
        //myweapon∑’Ë Ëß¡“µÈÕß¡’weaponData¥È«¬
        if( myweapon == null)
        {
            weaponIcon.color = Color.clear;
            ammoText.text = "";
            
            return;

        }
        

        Color newColor = activeWeapon? Color.white :new Color(1f,1f,1f,.39f);
        
        weaponIcon.color = newColor;
        
        weaponIcon.sprite = myweapon.weaponData?.weaponIcon;

        
        ammoText.text = myweapon.ammoInMagazine + "/" + myweapon.totalReserveAmmo;
        

    }

}
