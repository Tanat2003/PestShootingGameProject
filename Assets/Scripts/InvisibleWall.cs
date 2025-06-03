using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private ParticleSystem[] lines;
    private BoxCollider zoneCollider;

    private void Start()
    {
        zoneCollider = GetComponent<BoxCollider>();
        lines = GetComponentsInChildren<ParticleSystem>();
        ActivateWall(false);

    }

    private void ActivateWall(bool activate)
    {
        foreach (var line in lines)
        {
            if(activate)
            {
                line.Play();
            }
            else
            {
                line.Stop();
            }
        }
        zoneCollider.isTrigger = !activate;
    }

    IEnumerator WallActivateCooldown()
    {
        ActivateWall(true);
        yield return new WaitForSeconds(1);
        ActivateWall(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(WallActivateCooldown()); 
    }
}

