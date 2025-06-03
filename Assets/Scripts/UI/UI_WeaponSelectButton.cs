using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_WeaponSelectButton : UI_Button
{
    private UI_WeaponSelection weaponSelectionUI;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private GameObject lockedIcon;

    private UI_WeaponSelectedWindow emptySlots;
    private void OnValidate()
    {
        gameObject.name = "Button - Select Weapin : " + weaponData.weaponType;
    }
    public override void Start()
    {
        base.Start();
        weaponSelectionUI = GetComponentInParent<UI_WeaponSelection>();
        weaponIcon.sprite = weaponData.weaponIcon;
    }
    private void OnEnable()
    {
        if(weaponData.unlockedWeapon == false)
        {
            lockedIcon.SetActive(true);
        }
        else
        {
            lockedIcon.SetActive(false);

        }
        

    }
    private void OnDisable()
    {
        if (weaponData.unlockedWeapon == false)
        {
            lockedIcon.SetActive(true);
        }
        else
        {
            lockedIcon.SetActive(false);

        }

    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if(weaponData.unlockedWeapon == false)
        {
            weaponIcon.color = Color.gray;
        }
        else
        {

            weaponIcon.color = Color.yellow;
        }

        emptySlots = weaponSelectionUI.FindEmptySlot();
        emptySlots?.UpdateSlotInfo(weaponData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        weaponIcon.color = Color.white;
        emptySlots?.UpdateSlotInfo(null);
        emptySlots = null;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (weaponData.unlockedWeapon == false)
            return;


        base.OnPointerDown(eventData);
        weaponIcon.color = Color.white;

        bool noMoreEmptySlot = weaponSelectionUI.FindEmptySlot() == null;
        bool noThisWeaponInSlot = weaponSelectionUI.FindSlotWithWeaponType(weaponData) == null;

        if(noMoreEmptySlot && noThisWeaponInSlot)
        {
            weaponSelectionUI.ShowWarningMessage("เลือกได้เพียง 3 กระบอก");
            return;
        }
        //ถ้าคลิกปุ่มเดิมก็จะหาค่าเจอ
        UI_WeaponSelectedWindow slotBusyWithThisWeapon = weaponSelectionUI.FindSlotWithWeaponType(weaponData);
        if (slotBusyWithThisWeapon != null)
        {
            slotBusyWithThisWeapon.SetWeaponSlot(null);
        }
        else
        {
            emptySlots = weaponSelectionUI.FindEmptySlot();
            emptySlots.SetWeaponSlot(weaponData);
        }



        emptySlots = null;
    }
}
