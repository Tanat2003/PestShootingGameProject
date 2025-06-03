
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_HealthController : MonoBehaviour, IDamagable
{
    public int maxHealth;
    public int currentHealth;

    private bool carBroken;
    private Car_Controller car_Controller;

    [Header("ExplosionInfo")]
    [Space]
    [SerializeField] private int explosionDamage = 500;
    [SerializeField] private float explosionForce = 9;
    [SerializeField] private float explosionUpwardsModifer = 2;
    [SerializeField] private float explosionDelay = 3;
    [SerializeField] private float explosionRadius = 3;
    [SerializeField] private Transform explosionPoint;

    [SerializeField] private ParticleSystem fireFX;
    [SerializeField] private ParticleSystem explosion;
    
    private void Start()
    {
        
        car_Controller = GetComponent<Car_Controller>();
        currentHealth = maxHealth;

        
    }
    private void Update()
    {
        if(fireFX.gameObject.activeSelf)
            fireFX.transform.rotation = Quaternion.identity;
    }
    private void ReduceHealth(int damage)
    {
        if (carBroken)
            return;
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            BreakCar();
        }
    }

    private void BreakCar()
    {
        carBroken = true;
        car_Controller.CrashTheCar();

        fireFX.gameObject.SetActive(true);
        StartCoroutine(ExplosionCo(explosionDelay));
        
    }

    public void TakeDamage(int damage)
    {
        ReduceHealth(damage);
        UpdateCarHealthUI();
    }
    public void UpdateCarHealthUI()
    {
        UI.instance.uiInGame.UpdateCarHealth(currentHealth,maxHealth);
    }

    private IEnumerator ExplosionCo(float delay)
    {

        
        yield return new WaitForSeconds(delay);
        explosion.gameObject.SetActive(true);


        
        car_Controller.rb.AddExplosionForce
            (explosionForce,explosionPoint.position,explosionRadius,explosionUpwardsModifer,ForceMode.Impulse);
        
        
        
        Explode();
        explosion.gameObject.SetActive(false);
        

    }

    private void Explode()
    {
        
       HashSet<GameObject> unieqEntities = new HashSet<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(explosionPoint.position,explosionRadius);
        //Obj‰Àπ¡’Idamagable∫È“ß
        foreach (Collider hit in colliders)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null)
            {
                GameObject rootEntities = hit.transform.root.gameObject;

                if (unieqEntities.Add(rootEntities) == false)
                    continue;
                damagable.TakeDamage(explosionDamage);

                

                //Obj∑’Ë¡’Idamagable‚¥π√–‡∫‘¥·≈È«°√–‡¥Áπ
                hit.GetComponentInChildren<Rigidbody>()
                    .AddExplosionForce
                    (explosionForce, explosionPoint.position, explosionRadius, explosionUpwardsModifer, ForceMode.VelocityChange);
            }

        }
         
    }

    public bool CarBroken() => carBroken;
    public void FixCar()
    {
        string fixCarTextInfo = "§ÿ≥‰¥È„™ÈÀÿËπ¬πµÏµ—«π’È´ËÕ¡√∂·≈È«";
        carBroken = false;
        car_Controller.FixTheCar();

        currentHealth = maxHealth;
        UpdateCarHealthUI();

        UI.instance.uiInGame.DisplayInfoWhenInteract(fixCarTextInfo);

        fireFX.gameObject.SetActive(false);

        
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(explosionPoint.position, explosionRadius);
    }
}
