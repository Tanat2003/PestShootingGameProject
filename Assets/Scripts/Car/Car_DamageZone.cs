using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_DamageZone : MonoBehaviour
{
   private Car_Controller car_controller;
    [SerializeField] private int carDamage;
    [SerializeField] private float impactForce = 150;
    [SerializeField] private float upwardMultyiplier = 3;
    [SerializeField] private float minSpeedToDamage = 1.5f;    


    private void Awake()
    {
        car_controller = GetComponentInParent<Car_Controller>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (car_controller.rb.velocity.magnitude < minSpeedToDamage)
            return;
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable == null)
        {
            return;
        }
        
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb != null)
        {
            ApplyForce(rb);
        }
        if (other.GetComponent<EnemyBoss>())
        {
            damagable.TakeDamage(carDamage /2);
            return;
        }
        damagable.TakeDamage(carDamage);
    }
    private void ApplyForce(Rigidbody rb)
    {
        

        rb.isKinematic = false;
        rb.AddExplosionForce(impactForce,transform.position,3,upwardMultyiplier,ForceMode.Impulse);
    }
}
