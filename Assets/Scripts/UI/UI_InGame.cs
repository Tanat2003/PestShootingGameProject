using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DialogueWith
{
    Owner, Boss,
}
public class UI_InGame : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject carUI;

    [Header("Health")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image damageScreenEffect;

    [Space]
    [Header("WeaponSlot")]
    [SerializeField] private UI_WeaponSlot[] weaponSlotsUI;

    [Space]
    [Header("Missions")]
    [SerializeField] private GameObject missionInfoParent;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI missionDetail;
    [SerializeField] private TextMeshProUGUI bossCountDownText;


    [Space]
    [Header("Dialog")]
    [SerializeField] private TextMeshProUGUI dialogTextOwn;
    [SerializeField] private TextMeshProUGUI dialogTextPlrWithOwn;
    [SerializeField] private Image ownerIcon;
    [SerializeField] private Image plrIcon;
    [SerializeField] private Image dialogTextBox;
    public string[] dialogOwn;
    public string[] dialogPlrWithOwn;
    private DialogueWith playerHasDialogWith;


    [Header("BossDialog")]
    [SerializeField] private TextMeshProUGUI dialogTextBoss;
    [SerializeField] private TextMeshProUGUI dialogTextPlrWithBoss;
    [SerializeField] private UI_BossIconHolder bossIconHolder;
    private Image bossIcon;

    public string[] dialogBoss;
    public string[] dialogPlrWithBoss;

    [Space]
    [Header("EnemyInfo")]
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI enemyDetail;
    [SerializeField] private GameObject enemyInfoPanel;

    [Space]
    [Header("OrbInfo")]
    [SerializeField] private GameObject orbInfoPanel;
    [SerializeField] private TextMeshProUGUI orbInfoText;



    private int currentEnemyIndex;

    private Coroutine displayRoutine;


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


        bossIcon = bossIconHolder.GetBossIcon();
        displayRoutine = StartCoroutine(ShowDialogue(1f));
    }


    #region MissionInfo
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
    private IEnumerator ShowEnemyInfoForhMission()
    {

        enemyInfoPanel.SetActive(true);

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

            CameraManager.instance.ChangeCameraTarget(filteredList[currentEnemyIndex].gameObject.transform, 7, 0, true);
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

        GameManager.instance.player.GetComponent<Player_WeaponController>().ShowWeaponUpgradeFX();

        enemyInfoPanel.SetActive(false);
        CameraManager.instance.ChangeCameraTarget(GameManager.instance.player.gameObject.transform, 6);
        ConTrolManager.instance.SwitchToCharacterControls();
        Mission_Manager.instance.startMission = true;

        SetDialog
            (Mission_Manager.instance.currentMission.dialogBoss,
                Mission_Manager.instance.currentMission.dialogPlayerWithBoss, DialogueWith.Boss);
    }




    #endregion

    #region CarUIMethod
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
    public void UpdateCarHealth(float currentCarHealth, float maxCarHealth)
    {
        carHealthBar.fillAmount = currentCarHealth / maxCarHealth;
        carHealthText.text = "" + currentCarHealth + " / " + maxCarHealth;
    }
    public void UpdateSpeedText(string text) => carSpeedText.text = text;
    #endregion

    #region OwnerDialogue Method
    public void SetDialog(string[] dialogForOther, string[] dialogForPlr, DialogueWith dialogWith)
    {

        
        switch (dialogWith)
        {
            case DialogueWith.Owner:
                dialogOwn = dialogForOther;
                dialogPlrWithOwn = dialogForPlr;
                break;

            case DialogueWith.Boss:
                dialogBoss = dialogForOther;
                dialogPlrWithBoss = dialogForPlr;
                break;

               
        }
        playerHasDialogWith = dialogWith;




    }
    private IEnumerator ShowDialogue(float delay)
    {
        


        yield return new WaitForSeconds(delay);
       

        if (playerHasDialogWith == DialogueWith.Owner)
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
                for (int i = 0; i < Mathf.Min(dialogOwn.Length, dialogPlrWithOwn.Length); i++)
                {
                    // แสดงบทพูดของ Owner
                    AudioManager.instance.PlayOwnerSpeakSound();
                    SetDialogueUI(ownerIcon, plrIcon, dialogOwn[i], "");
                    yield return WaitForInputOrTime(displayTime);

                    // แสดงบทพูดของ Player
                    AudioManager.instance.PlayPlrSpeakSound();
                    SetDialogueUI(plrIcon, ownerIcon, "", dialogPlrWithOwn[i], .35f);
                    yield return WaitForInputOrTime(displayTime);


                }
            }
            finally
            {
                StopDialogue();
            }
        }

        if (playerHasDialogWith == DialogueWith.Boss)
        {
            
            

            dialogTextBox.gameObject.SetActive(true);
            bossIcon.gameObject.SetActive(true);
            plrIcon.gameObject.SetActive(true);

            AudioManager.instance.PlayBGM(0);

            TimeManager.instance.PauseTime();

            ConTrolManager.instance.SwtichToUIControls();
            dialogTextBox.gameObject.SetActive(true);
            bossIcon.gameObject.SetActive(true);
            plrIcon.gameObject.SetActive(true);
            try
            {
                for (int i = 0; i < Mathf.Min(dialogBoss.Length, dialogPlrWithBoss.Length); i++)
                {
                    
                    // แสดงบทพูดของ boss
                    AudioManager.instance.PlayOwnerSpeakSound();
                    SetDialogueUI(bossIcon, plrIcon, dialogBoss[i], "");
                    yield return WaitForInputOrTime(displayTime);

                    // แสดงบทพูดของ Player
                    AudioManager.instance.PlayPlrSpeakSound();
                    SetDialogueUI(plrIcon, bossIcon, "", dialogPlrWithBoss[i], .35f);
                    yield return WaitForInputOrTime(displayTime);


                }
            }
            finally
            {
                StopDialogue();
            }
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
    public void StartDialogueWithDelay(float delay = 0)
    {
        
        
        displayRoutine = StartCoroutine(ShowDialogue(delay));



    }

    private void SetDialogueUI(Image activeIcon, Image inactiveIcon, string otherText, string plrText, float aAlpha = .5f)
    {

        if (playerHasDialogWith == DialogueWith.Owner)
        {
            activeIcon.color = Color.white;
            inactiveIcon.color = new Color(1, 1, 1, aAlpha); // เปลี่ยนความโปร่งใส

            dialogTextOwn.text = otherText;
            dialogTextPlrWithOwn.text = plrText;
        }
        else if (playerHasDialogWith == DialogueWith.Boss)
        {
            activeIcon.color = Color.white;
            inactiveIcon.color = new Color(1, 1, 1, aAlpha); // เปลี่ยนความโปร่งใส



            dialogTextBoss.text = otherText;
            dialogTextPlrWithBoss.text = plrText;
        }

    }
    public void StopDialogue()
    {
        
        if (playerHasDialogWith == DialogueWith.Owner)
        {

            dialogTextPlrWithOwn.text = "";
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
        else if (playerHasDialogWith == DialogueWith.Boss)
        {

            dialogTextPlrWithBoss.text = "";
            dialogTextBoss.text = "";


            bossIcon.gameObject.SetActive(false);
            plrIcon.gameObject.SetActive(false);
            dialogTextBox.gameObject.SetActive(false);

            TimeManager.instance.ResumeTime();
            ConTrolManager.instance.SwitchToCharacterControls();
            CameraManager.instance.ChangeCameraTarget(GameManager.instance.player.transform);

            if (displayRoutine != null)
            {
                StopCoroutine(displayRoutine);
                displayRoutine = null;

            }

        }



    }
    #endregion

    #region DamageScreenMethod
    private IEnumerator DisplayDamageScreenCo(float timeToDisplay)
    {
        damageScreenEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeToDisplay);
        damageScreenEffect.gameObject.SetActive(false);

    }
    public void DisplayDamageScreen(float timeToDisplay)
    {
        displayRoutine = StartCoroutine(DisplayDamageScreenCo(timeToDisplay));
    }
    #endregion


    public void UpdatePlayerHealth(float currentHealth, float maxHealth)
    {

        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = "" + currentHealth + " / " + maxHealth;

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
    public void UpdateBossWaringInfo(string text)
    {
        this.bossCountDownText.text = text;
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

        if (canPauseGame)
            return true;
        else
            return false;
    }



}
