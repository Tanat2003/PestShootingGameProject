using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject carUI;
    [Header("Health")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image damageScreenEffect;


    [Header("WeaponSlot")]
    [SerializeField] private UI_WeaponSlot[] weaponSlotsUI;

    [Header("Missions")]
    [SerializeField] private GameObject missionInfoParent;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI missionDetail;
    [SerializeField] private TextMeshProUGUI bossCountDownText;


    [Space]
    [Header("Dialog")]
    [SerializeField] private TextMeshProUGUI dialogTextOwn;
    [SerializeField] private TextMeshProUGUI dialogTextPlr;
    [SerializeField] private Image ownerIcon;
    [SerializeField] private Image plrIcon;
    [SerializeField] private Image dialogTextBox;
    public string[] dialogOwn;
    public string[] dialogPlr;

    [Header("EnemyInfo")]
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI enemyDetail;
    [SerializeField] private GameObject enemyInfoPanel;

    [Header("OrbInfo")]
    [SerializeField] private GameObject orbInfoPanel;
    [SerializeField] private TextMeshProUGUI orbInfoText;

    

    private int currentEnemyIndex;
    private bool isDisplaying = true;
    private Coroutine displayRoutine;
   
    public bool candoCoroutine;


    private float displayTime = 5f;

    private int currentLineIndex = 0; // ติดตามว่ากำลังอยู่ที่ประโยคไหน


    [Header("CarInfo")]
    [SerializeField] private Image carHealthBar;
    [SerializeField] private TextMeshProUGUI carSpeedText;
    [SerializeField] private TextMeshProUGUI carHealthText;
    private bool missioninfo = true;
    
    private void Awake()
    {
        if (GameManager.instance.quickStart == true)
        {
            ownerIcon.enabled = false;
        }
        weaponSlotsUI = GetComponentsInChildren<UI_WeaponSlot>();


    }
    private void OnEnable()
    {
       
        if (candoCoroutine == false)
            return;
        displayRoutine = StartCoroutine(StartDialogueWithDelay());
    }
    
    public void TurnOnOffMissionInfo()
    {
        missioninfo = !missioninfo;
        missionInfoParent.SetActive(missioninfo);


    }

    public void UpdateMissionInfo(string missionText, string missionDetails = "")
    {
        this.missionText.text = missionText;
        this.missionDetail.text = missionDetails;

    }
    public void UpdateBossWaringInfo(string text)
    {
        this.bossCountDownText.text = text;
    }
    public void UpdateWeaponUI(List<Weapon> weaponSlots, Weapon currentWeapon)
    {
        for (int i = 0; i < weaponSlotsUI.Length; i++) //อัพเดมอาวุธที่ใช้
        {
            if (i < weaponSlots.Count)
            {
                bool isActiveWeapon;
                if (weaponSlots[i] == currentWeapon)
                {
                    isActiveWeapon = true;
                }
                else isActiveWeapon = false;

                weaponSlotsUI[i].UpdateWeaponSlot(weaponSlots[i], isActiveWeapon);
            }
            else
            {
                weaponSlotsUI[i].UpdateWeaponSlot(null, false); //อัพเดทอาวุธที่ไม่ใช้
            }
        }

    }

    public void UpdatePlayerHealth(float currentHealth, float maxHealth)
    {

        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = ""+currentHealth+ " / " + maxHealth;

    }
    public void UpdateCarHealth(float currentCarHealth, float maxCarHealth)
    {
        carHealthBar.fillAmount = currentCarHealth / maxCarHealth;
        carHealthText.text = "" + currentCarHealth +" / " + maxCarHealth;
    }


    public void SwitchToCharacterUI()
    {
        characterUI.SetActive(true);
        carUI.SetActive(false);
    }
    public void SwtichToCarUI()
    {
        carUI.SetActive(true);
        characterUI.SetActive(false);
    }
   
    public void UpdateSpeedText(string text) => carSpeedText.text = text;
    #region Dialogue Method
    public void SetDialog(string[] dialogforown, string[] dialogforplr)
    {

        dialogPlr = dialogforplr;
        dialogOwn = dialogforown;

    }
    private IEnumerator ShowDialogue()
    {
        
        AudioManager.instance.PlayBGM(0);
        yield return new WaitForSeconds(2);
        TimeManager.instance.PauseTime();
        ConTrolManager.instance.SwtichToUIControls();
        dialogTextBox.gameObject.SetActive(true);
        ownerIcon.gameObject.SetActive(true);
        plrIcon.gameObject.SetActive(true);
        try
        {
            for (int i = 0; i < Mathf.Min(dialogOwn.Length, dialogPlr.Length); i++)
            {
                // แสดงบทพูดของ Owner
                AudioManager.instance.PlayOwnerSpeakSound();
                SetDialogueUI(ownerIcon, plrIcon, dialogOwn[i], "");
                yield return WaitForInputOrTime(displayTime);

                // แสดงบทพูดของ Player
                AudioManager.instance.PlayPlrSpeakSound();
                SetDialogueUI(plrIcon, ownerIcon, "", dialogPlr[i], .35f);
                yield return WaitForInputOrTime(displayTime);

                
            }
        }
        finally
        {
            StopDialogue();
        }

    }
    private IEnumerator WaitForInputOrTime(float delay)
    {
        float elapsed = 0;
        while (elapsed < delay)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                elapsed += 3;
            }
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }
    public IEnumerator StartDialogueWithDelay(float delay = 0)
    {
        
        yield return new WaitForSeconds(delay);
        displayRoutine = StartCoroutine(ShowDialogue());
    }
    private void SetDialogueUI(Image activeIcon, Image inactiveIcon, string ownText, string plrText,float aAlpha =.5f)
    {
        
        activeIcon.color = Color.white;
        inactiveIcon.color = new Color(1, 1, 1, aAlpha); // เปลี่ยนความโปร่งใส



        dialogTextOwn.text = ownText;
        dialogTextPlr.text = plrText;
    }
    public void StopDialogue()
    {

        isDisplaying = false;
        dialogTextPlr.text = "";
        dialogTextOwn.text = "";


        ownerIcon.gameObject.SetActive(false);
        plrIcon.gameObject.SetActive(false);
        dialogTextBox.gameObject.SetActive(false);

        TimeManager.instance.ResumeTime();
        ConTrolManager.instance.SwitchToCharacterControls();

        if (displayRoutine != null)
        {
            StopCoroutine(displayRoutine);
            displayRoutine = null;
        }
        
        StartCoroutine(StartEnemyShowInfo());


    }
    #endregion
    private IEnumerator ShowEnemyInfoForhMission()
    {
        
        enemyInfoPanel.SetActive(true);
        isDisplaying = true;
        currentEnemyIndex = 0;
        ConTrolManager.instance.SwtichToUIControls();
        List<Enemy> allenemys = LevelGenerator.Instance.GetEnemyList();
        var nameOfEnemyToShowInfo = Mission_Manager.instance.currentMission.enemyToEnable;
        List<Enemy> filteredList = new List<Enemy>();
        HashSet<string> addedEnemyNames = new HashSet<string>(); // ใช้เก็บชื่อที่ถูกเพิ่มแล้ว

        foreach (Enemy enemy in allenemys)
        {
            if (nameOfEnemyToShowInfo.Contains(enemy.enemyName) && !addedEnemyNames.Contains(enemy.enemyName.ToString()))
            {
                filteredList.Add(enemy);
                addedEnemyNames.Add(enemy.enemyName.ToString()); // บันทึกชื่อไว้ว่าเพิ่มแล้ว
            }
        }

        foreach (var enemy in filteredList)
        {

            CameraManager.instance.ChangeCameraTarget(filteredList[currentEnemyIndex].gameObject.transform,7,0,true);
            enemyName.text = filteredList[currentEnemyIndex].enemyName.ToString();
            enemyDetail.text = filteredList[currentEnemyIndex].enemyDetail;
           


            yield return WaitForInputOrTime(displayTime);

            currentEnemyIndex++;


        }



        StopEnemyDisplay();


    }

   
    private IEnumerator StartEnemyShowInfo(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        displayRoutine = StartCoroutine(ShowEnemyInfoForhMission());
    }
    private void StopEnemyDisplay()
    {

        if (displayRoutine != null)
        {
            StopCoroutine(displayRoutine);
            displayRoutine = null;
        }
        candoCoroutine = false;
        GameManager.instance.player.GetComponent<Player_WeaponController>().ShowWeaponUpgradeFX();
        isDisplaying = false;
        enemyInfoPanel.SetActive(false);
        CameraManager.instance.ChangeCameraTarget(GameManager.instance.player.gameObject.transform,6);
        ConTrolManager.instance.SwitchToCharacterControls();
        Mission_Manager.instance.startMission = true;
    }
    
    

    public void DisplayInfoWhenInteract(string infoToShow)
    {
        displayRoutine = StartCoroutine(DisplayInfoWhenInteractCo(infoToShow));
    }
    private IEnumerator DisplayInfoWhenInteractCo(string infoToShow)
    {
        orbInfoPanel.SetActive(true);
        orbInfoText.text = infoToShow;
        yield return new WaitForSeconds(4);
        orbInfoPanel.SetActive(false);
    }
    public bool canPauseGame()
    {
        bool canPauseGame = this.gameObject.activeSelf;

        if(canPauseGame)
            return true;
        else 
            return false;
    }

    public void DisplayDamageScreen(float timeToDisplay)
    {
        displayRoutine = StartCoroutine(DisplayDamageScreenCo(timeToDisplay));
    }
    private IEnumerator DisplayDamageScreenCo(float timeToDisplay)
    {
        damageScreenEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeToDisplay);
        damageScreenEffect.gameObject.SetActive(false);

    }



}
