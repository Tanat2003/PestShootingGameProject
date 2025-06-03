using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducePlayerHealth : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Player_Health>().ReduceHealth(999);
        }
    }
}
