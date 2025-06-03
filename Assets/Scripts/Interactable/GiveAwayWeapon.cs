using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAwayWeapon : Interactable
{
    [SerializeField] private GameObject weapon;
    private Vector3 dropPosition;
    private void Start()
    {
        dropPosition = transform.position+ new Vector3(-2,0,0);
    }
    public override void InterAction()
    {
        base.InterAction();

        Object_Pool.instance.GetObject(weapon,transform,false,true);
    }

}
