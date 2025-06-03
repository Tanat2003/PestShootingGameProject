using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WeaponSelection : MonoBehaviour
{
    [SerializeField] private GameObject nextUIToSwitchOn;
    public UI_WeaponSelectedWindow[] selectedWeapon;

    [Header("WarningInfo")]
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private float warnTextDisaperSpeed = .25f;
    private float currentWarningAlpha;
    private float targetWarningAlpha;
    private void Start()
    {
        selectedWeapon = GetComponentsInChildren<UI_WeaponSelectedWindow>();
    }
    private void Update()
    {
        if (currentWarningAlpha > targetWarningAlpha)
        {
            currentWarningAlpha -= Time.deltaTime * warnTextDisaperSpeed;
            warningText.color = new Color(1, 1, 1, currentWarningAlpha);
        }
    }

    public UI_WeaponSelectedWindow FindEmptySlot()
    {
        for (int i = 0; i < selectedWeapon.Length; i++)
        {
            if (selectedWeapon[i].IsSlotEmpyty())
            {
                return selectedWeapon[i];
            }
        }
        return null;
    }

    public UI_WeaponSelectedWindow FindSlotWithWeaponType(WeaponData weaponData)
    {
        for (int i = 0; i < selectedWeapon.Length; i++)
        {
            if (selectedWeapon[i].weaponData == weaponData)
            {
                return selectedWeapon[i];
            }
        }
        return null;
    }
    public void ConfirmWeaponSelection()
    {
        if (HasAtleastOneWeapon())
        {
            UI.instance.SwtichTo(nextUIToSwitchOn);
            UI.instance.StartLevelGeneration();

        }
        else
        {
            ShowWarningMessage("ต้องเลือกปืนอย่างน้อย 1 กระบอก");
        }
    }
    private bool HasAtleastOneWeapon() => SelectedWeapon().Count > 0;
    public void ShowWarningMessage(string message)
    {
        warningText.color = Color.white;
        warningText.text = message;
        currentWarningAlpha = warningText.color.a;
        targetWarningAlpha = 0;
    }
    public List<WeaponData> SelectedWeapon()
    {
        List<WeaponData> selectedData = new List<WeaponData>();
        foreach (UI_WeaponSelectedWindow weapon in selectedWeapon)
        {
            if (weapon.weaponData != null)
            {
                selectedData.Add(weapon.weaponData);
            }
        }
        return selectedData;
    }
}
