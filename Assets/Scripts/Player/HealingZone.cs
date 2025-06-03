using System.Collections;
using UnityEngine;

public class HealingZone : MonoBehaviour
{
    private GameObject player;
    private float lastTimeHeal;
    private float healCooldown = 3;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void OnTriggerStay(Collider other)
    {
        Heal(other);

    }

    private void Heal(Collider other)
    {
        if (other.gameObject != player)
        {
            return;
        }
        if (Time.time - lastTimeHeal < healCooldown)
        {
            return;
        }
        lastTimeHeal = Time.time;
        player.GetComponent<Player_Health>().IncreaseHealth(10);
    }




}
