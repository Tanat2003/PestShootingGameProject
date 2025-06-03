using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp_Orb : Interactable
{
    [SerializeField] private Weapon orb;
    [SerializeField] private WeaponData orbData;

    private void OnEnable()
    {
        orb = new Weapon(orbData);
    }



    public override void InterAction()
    {
        weaponController.PickupWeapon(orb);
        UI.instance.uiInGame.DisplayInfoWhenInteract(orb.weaponData.weaponInfo);
       Object_Pool.instance.ReturnObject(gameObject);

    }
}
