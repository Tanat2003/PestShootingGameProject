using UnityEngine;

public class SpinDamageArea : MonoBehaviour
{
    private EnemyBoss enemy;
    private float damageCoolDown;
    private float lastTimeDamage;
    private int spinDamage;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyBoss>();
        damageCoolDown = enemy.spindDamageCooldown;
        spinDamage = enemy.spinDamage;

    }

    private void OnTriggerStay(Collider other)
    {
        if (enemy.spinActive == false)
            return;
        if (Time.time - lastTimeDamage < damageCoolDown)
            return;

        IDamagable damagable = other.GetComponent<IDamagable>();
        
        if (damagable != null)
        {
            damagable.TakeDamage(spinDamage);
            lastTimeDamage = Time.time; //Update ‡«≈“≈Ë“ ÿ¥∑’Ë∑”¥“‡¡®‰ª
            damageCoolDown = enemy.spindDamageCooldown; //Õ—æ‡¥∑cooldowndamage∑ÿ°§√—Èß∑’Ë‚¥πºŸÈ‡≈Ëπ

        }
    }
}
