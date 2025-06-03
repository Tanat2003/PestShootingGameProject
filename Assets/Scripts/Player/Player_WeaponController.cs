using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_WeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20f; //ความเร็วกระสุนพื้นฐานที่ใช้ในสูตรคำนวน
    private Player player;



    [SerializeField] private List<WeaponData> defaultWeaponData;
    [SerializeField] private Weapon currentWeapon;
    private bool weaponIsReady;
    private bool isShooting;

    [Space]
    [Header("Bullet Detail")]
    [SerializeField] private float bulletImpactForce = 100;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    [Space]

    [Header("ForDebugging")]
    [SerializeField] private Transform weaponHolder;
    public List<WeaponModel> allWeaponModels;
    public List<WeaponData> allWeaponDatas;
    [Space]

    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;
    [SerializeField] private GameObject weaponPickupPrefab;




    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvent();

        currentWeapon.ammoInMagazine = currentWeapon.totalReserveAmmo;

    }
    private void Update()
    {
        if (isShooting)
        {
            Shoot();
        }

    }


    private IEnumerator BurstFire()
    {
        SetWeaponReady(false);
        for (int i = 1; i <= currentWeapon.bulletPerShot; i++)
        {
            FireSingleBullet();
            yield return new WaitForSeconds(currentWeapon.burst_FireDelay);
            if (i >= currentWeapon.bulletPerShot)
            {
                SetWeaponReady(true);
            }
        }
    }
    private void Shoot()
    {


        if (WeaponReady() == false)
        {
            return;
        }
        if (currentWeapon.CanShoot() == false)
        {
            return;
        }
        player.weaponVisuals.PlayerFireAnimation();
        //เช็คถ้าไม่ใช่ปืนประเภทที่ยิงค้างได้ให้กดทีละครั้งถึงยิงกระสุน
        if (currentWeapon.shootType == ShootType.Single)
        {
            isShooting = false; //ให้isShootingเป็นfalse และมันจะไม่เป็นtrueจนกว่าจะคลิกเม้าซ้ายอีกครั้ง    
        }
        if (currentWeapon.BurstActivated() == true) //ถ้าปืนนี้เปิดใช้งานburstโหมดให้ยิงอัตโนมัติต่อวิ
        {
            StartCoroutine(BurstFire());
            return;
        }

        FireSingleBullet();
        TriggerEnemyDodge();




    }//การยิงปืน

    private void FireSingleBullet()
    {
        currentWeapon.ammoInMagazine--;
        UpdateWeaponUI();
        player.weaponVisuals.CurrentWeaponModel().fireSFX.Play();
        // เอากระสุนมาจากBulletPoolที่เข้าคิวไว้
        GameObject newBullet = Object_Pool.instance.GetObject(bulletPrefab, GunPoint());


        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);


        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetup(currentWeapon.gunDistance, currentWeapon.bulletDamage, bulletImpactForce);

        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        //กำหนดmassให้กระสุนเพื่อให้กระสุนพุ่งเร็วแค่ไหนก็ได้ในขณะที่ยังชนกับวัตถุด้วยมวลเท่าเดิมกับที่เราเทส

        rbNewBullet.velocity = bulletDirection * bulletSpeed;
    }

    private void Reload()
    {
        SetWeaponReady(false);
        player.weaponVisuals.PlayReloadAnimation();
        player.weaponVisuals.CurrentWeaponModel().reloadSFX.Play();
        //อัปเดตuiในplayerAnimationEvent
    }

    #region Slots Manager(Equip,Pickup,Drop,ReadyWeapon)
    private void EquipWeapon(int i)
    {
        if (i >= weaponSlots.Count) //ถ้าพยายามกดเปลี่ยนปืนที่ไม่มีให้รีเทิรน
        {
            return;
        }
        SetWeaponReady(false);
        currentWeapon = weaponSlots[i];

        player.weaponVisuals.PlayWeaponEquipAnimation();

        CameraManager.instance.ChangeCameraDistance(currentWeapon.cameraDistance);

        UpdateWeaponUI();
    }

    public void PickupWeapon(Weapon newWeapon)
    {

        if (WeaponInInventory(newWeapon.weaponType) != null)
        {
            WeaponInInventory(newWeapon.weaponType).totalReserveAmmo += newWeapon.ammoInMagazine;
            UpdateWeaponUI();
            return;
        }
        //ถ้าสล็อตเต็มและปืนที่จะเก็บไม่ใช่ปืนที่ถือให้ทำการเปลี่ยนปืน(เก็บเป็นถือ)
        if (weaponSlots.Count >= maxSlots && newWeapon.weaponType != currentWeapon.weaponType)
        {

            int weaponIndex = weaponSlots.IndexOf(currentWeapon);

            player.weaponVisuals.SwitchOffWeaponModels();

            weaponSlots[weaponIndex] = newWeapon;
            DropWeaponOnTheGround();

            EquipWeapon(weaponIndex);

            UpdateWeaponUI();
            return;
        }
        //ถ้าไม่มีอาวุธนั้นในกระเป๋าและไม่ได้ถืออยู่ให้เพิ่มอาวุธใหม่ลงกระเป๋า
        weaponSlots.Add(newWeapon);
        player.weaponVisuals.SwitchOnBackupWeaponModel();
        UpdateWeaponUI();

        //ตอนหน้าเลือกอาวุธจะได้ปลดล็อกการเลือกอาวุธนี้
        if (newWeapon.weaponData.unlockedWeapon == false)
        {
            if (GameManager.instance.playerIsInTurtorial)
                return;

            newWeapon.weaponData.unlockedWeapon = true;

        }



    }

    private void DropWeapon()
    {
        if (HasOnlyOneWeapon()) //ถ้ามีอาวุธ1ชิ้นทิ้งอาวุธไม่ได้
        {
            return;
        }
        DropWeaponOnTheGround();
        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0); //ให้ใส่อาวุธในอาเรย์อีกช่องที่เหลืออยู่

    }

    private void DropWeaponOnTheGround()
    {
        GameObject dropWeapon = Object_Pool.instance.GetObject(weaponPickupPrefab, transform);
        dropWeapon.GetComponent<Pickup_Weapon>()?.SetupPickupWeapon(currentWeapon, transform);
    }
    public void RemoveWeaponFromWeaponSlot(WeaponType typeOfWeaponToRemove)
    {
        Weapon weaponToRemove = WeaponInInventory(typeOfWeaponToRemove);
        if (weaponToRemove != null)
        {
            weaponSlots.Remove(weaponToRemove);
            UpdateWeaponUI();
        }

    }

    //SetWeaponReadyไว้เช็คว่าอาวุธเราพร้อมยังถ้าพร้อมให้เลเซอร์ขึ้นบนปืน,สามารถรีโหลดกระสุนได้
    //ไม่ให้สามารถยิงหรือรีโหลดกระสุนได้ตอนที่ยังหยิบปืนขึ้นมาไม่เสร็จ
    public void SetWeaponReady(bool ready)
    {

        weaponIsReady = ready; //เซ็ทสถานะweaponIsready
        if (ready)
            player.sfx.weaponReady.Play();
    }

    public bool WeaponReady() => weaponIsReady;
    #endregion
    #region UpgradeWeaponMethod
    public void UpgradeAttackWeapon(Weapon weaponToUpgrade)
    {
        foreach (var weapon in weaponSlots)
        {
            if (weapon.weaponType == weaponToUpgrade.weaponType)
            {
                foreach (var weaponmodel in player.weaponVisuals.weaponModels)
                {
                    if (weaponmodel.weaponType == weapon.weaponType)
                    {
                        int index = weapon.currentUpgradeWeapon;

                        weaponmodel.SetUpgradeFX(index, 0);
                        weapon.UpgradeAttackWeapon();
                    }
                }

            }
        }
    }

    public void UpgradeMagazineCapacity(Weapon weaponToUpgrade)
    {
        foreach (var weapon in weaponSlots)
        {
            if (weapon.weaponType == weaponToUpgrade.weaponType)
            {
                foreach (var weaponmodel in player.weaponVisuals.weaponModels)
                {
                    if (weaponmodel.weaponType == weapon.weaponType)
                    {
                        int index = weapon.currentUpgradeWeapon;

                        weaponmodel.SetUpgradeFX(index, 1);
                        weapon.UpgradeMagazineCapacity();
                        UpdateWeaponUI();
                    }
                }

            }
        }
    }
    public void ResetUpgrade()
    {
        foreach (var weaponmodel in allWeaponModels)
        {

            weaponmodel.ResetUpgradeFX();
        }
        foreach (var weapon in allWeaponDatas)
        {
            weapon.ResetUpgradeWeapon();

        }
    }
    public void ResetUnlocked()
    {
        foreach (var weapon in allWeaponDatas)
        {
            if (weapon.defaultWeapon == false)
            {
                weapon.unlockedWeapon = false;
            }

        }
    }
    public void ShowWeaponUpgradeFX()
    {
        foreach (var weaponmodel in player.weaponVisuals.weaponModels)
        {
            weaponmodel.PlayUpgradeFX();
        }

    }
    #endregion
    public void SetDefaultWeapon(List<WeaponData> newWeaponData)
    {
        defaultWeaponData = new List<WeaponData>(newWeaponData);
        weaponSlots.Clear();

        foreach (WeaponData weaponData in defaultWeaponData)
        {
            PickupWeapon(new Weapon(weaponData));
        }
        EquipWeapon(0);
    }

    public void UpdateWeaponUI()
    {

        UI.instance.uiInGame.UpdateWeaponUI(weaponSlots, currentWeapon);
    }
    public Vector3 BulletDirection() //ทิศทางของกระสุน    
    {
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - GunPoint().position).normalized;

        if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
        {
            direction.y = 0;

        }

        //weaponHolder.LookAt(aim);
        //gunPoint.LookAt(aim); หาที่วางใหม่

        return direction;
    }

    //ส่งตำแหน่งgunpointของปืนปัจจุบันที่ถือไป
    public Transform GunPoint() => player.weaponVisuals.CurrentWeaponModel().gunPoint;
    public Weapon CurrentWeapon() => currentWeapon;
    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1; //เช็คว่าตอนนี้เหลือปืนอันเดียวในกระเป๋ารึป่าว
    public Weapon WeaponInInventory(WeaponType weaponType) //เช็คว่ามีอาวุธนี้ในกระเป๋ามั้ย
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon.weaponType == weaponType)
            {
                return weapon;
            }
        }
        return null;
    }
    private void TriggerEnemyDodge()
    { //ให้ศัตรูหลบการโจมตีถ้าตอนที่ยิงเป้ามีศัตรูอยู่ข้างหน้า
        Vector3 rayOrigin = GunPoint().position;
        Vector3 rayDirection = BulletDirection();
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, Mathf.Infinity))
        {
            Enemy_Melee enemy_Melee = hit.collider.gameObject.GetComponentInParent<Enemy_Melee>();
            if (enemy_Melee != null)
            {
                enemy_Melee.ActiveDodgeRoll();
            }
        }

    }


    #region InputEvent
    private void AssignInputEvent()
    {
        PlayerControll controls = player.controls;
        player.controls.Character.Fire.performed += context => isShooting = true;
        player.controls.Character.Fire.canceled += context => isShooting = false;
        player.controls.Character.ToggleMode.performed += context => currentWeapon.ToggleBurst();


        controls.Character.EquipSlot1.performed += context => EquipWeapon(0); //กดปุ่ม1ให้ใช้สล็อตอาวุธแรก
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.EquipSlot3.performed += context => EquipWeapon(2);


        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };

    }

    #endregion

}
