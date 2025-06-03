using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimationEvents : MonoBehaviour
{
    private Player_WeaponVisual visualController;
    private Player_WeaponController weaponController;
    private void Start()
    {
        weaponController = GetComponentInParent<Player_WeaponController>();
        visualController = GetComponentInParent<Player_WeaponVisual>();
        
    }

    public void ReloadIsOver() 
    {

        visualController.ReturnLeftHandIKWeight();
        visualController.CurrentWeaponModel().reloadSFX.Stop();
        weaponController.CurrentWeapon().RefillBullets();

        weaponController.SetWeaponReady(true);
        weaponController.UpdateWeaponUI();
    }

    public void ReturnRig()
    {
        visualController.ReturnRigWeightToOne();
        visualController.ReturnLeftHandIKWeight();

    }

    public void WeaponEquipingIsOver()
    {
        
        weaponController.SetWeaponReady(true);
    }
    public void SwitchOnWeaponModel() => visualController.SwitchOnCurrentWeaponModel();
    
}
