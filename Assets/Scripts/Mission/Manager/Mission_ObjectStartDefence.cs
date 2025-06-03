using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_ObjectStartDefence : MonoBehaviour
{
    private GameObject player;

    public static event Action startDefence;

    
    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject != player)
        {
            return;
        }
        
        
        startDefence?.Invoke();

    }
}
