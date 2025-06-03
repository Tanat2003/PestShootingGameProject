using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Health : Interactable
{

    public override void InterAction()
    {
        base.InterAction();

        GameManager.instance.player.GetComponent<Player_Health>().IncreaseHealth(Random.Range(50, 91)); ;
        Object_Pool.instance.ReturnObject(gameObject);
        GameDataManager.instance.ItemCollected("HealthBox", Mission_Manager.instance.currentMission.missionName);

    }
}
