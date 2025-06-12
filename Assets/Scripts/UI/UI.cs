using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    public UI_InGame uiInGame { get; private set; }
    public UI_WeaponSelection weaponSelection { get; private set; }
    public UI_GameOver gameOverUI { get; private set; }
    public UI_Setting setting_UI { get; private set; }
    public UI_MissionSelection missionSelect_UI { get; private set; }
    public GameObject pauseUI;
    public GameObject victoryScreenUI;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private Texture2D customCursor;

    [Header("Fade Image")]
    [SerializeField] private Image fadeImage;

    
    

    private void Awake()
    {
        instance = this;
        uiInGame = GetComponentInChildren<UI_InGame>(true);
        weaponSelection = GetComponentInChildren<UI_WeaponSelection>(true);
        gameOverUI = GetComponentInChildren<UI_GameOver>(true);
        setting_UI = GetComponentInChildren<UI_Setting>(true);
        missionSelect_UI = GetComponentInChildren<UI_MissionSelection>(true);
    }
    private void Start()
    {
        AssignInputUI();
        StartCoroutine(ChangeImageAlpha(0, 1.5f, null));

        if(GameManager.instance.quickStart) //‰«ÈµÕπ‡∑ ‡°¡
        {
            StartLevelGeneration();
            StartGame();
        }
        
    }
    public void SwtichTo(GameObject uiToSwitchOn)
    {
        foreach (GameObject a in uiElements)
        {
            a.SetActive(false);
        }
        uiToSwitchOn.SetActive(true);
        if(uiToSwitchOn == setting_UI.gameObject)
        {
            setting_UI.LoadSetting();
        }
        if(uiToSwitchOn == missionSelect_UI.gameObject)
            missionSelect_UI.CheckAvailableLevel();
        if(uiToSwitchOn == uiInGame.gameObject)
        {
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
           // MinimapController.instance.GetEnemyPosition();
        }
        

    }
    public void StartGame() => StartCoroutine(StartGameWithFadeINOUT());
    public void StartLevelGeneration() => LevelGenerator.Instance.InitializeGeneration();
    //public void SetDefaultWeaponPlayer() => GameManager.Instance.SetDefaultWeapons();
    public void RestartGame()
    {
        SwtichTo(uiInGame.gameObject);
        ConTrolManager.instance.SwitchToCharacterControls();
        TimeManager.instance.ResumeTime();
        StartCoroutine(ChangeImageAlpha(1,1f,GameManager.instance.RestartScene));
        
        
    }
    public void PauseSwitch()
    {
        bool gamePause = pauseUI.activeSelf; //activeSelf §◊Õ UIπ—ÈπEnableÕ¬ŸË
        if (gamePause)
        {
            SwtichTo(uiInGame.gameObject);
            ConTrolManager.instance.SwitchToCharacterControls();
            TimeManager.instance.ResumeTime();
        }
        else
        {
            ConTrolManager.instance.SwtichToUIControls();
            SwtichTo(pauseUI);
            TimeManager.instance.PauseTime();
        }



    }
    public void ShowGameOverUI(string message = "¿“√°‘®≈È¡‡À≈«")
    {
        SwtichTo(gameOverUI.gameObject);
        gameOverUI.ShowGameOverMessage(message);
    }

    public void ShowVictoryScreenUI()
    {
        StartCoroutine(ChangeImageAlpha(1, 1.5f, SwitchToVictoryScreenUI));
    }
    private void SwitchToVictoryScreenUI()
    {
        AudioManager.instance.PlayBGM(12);
        SwtichTo(victoryScreenUI);
        victoryText.text = Mission_Manager.instance.currentMission.missionCompleteDescription;
        Color color = fadeImage.color;
        color.a = 0;
        fadeImage.color = color;


    }
    private void AssignInputUI()
    {
        PlayerControll controlss = GameManager.instance.player.controls;
        controlss.UI.UIPause.performed += ctx => PauseSwitch(); //Assign PauseSwitch„ÀÈUI ActionMap
    }
    private IEnumerator ChangeImageAlpha(float targetAlpha, float duration, System.Action onComplete)
    {

        float time = 0;
        Color currentColor = fadeImage.color;
        float startAlpha = currentColor.a;

        while (time < duration)
        {
            time += Time.deltaTime; //MathLerp√’‡∑√‘π§Ë“A∂÷ßB‚¥¬∑’Ë¡’ChangeRate C
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null; //§◊Õ°“√„ÀÈCorutine À¬ÿ¥ Executionµ√ßπ—Èπ·≈–√’‡∑√‘π„π‡ø√¡∂—¥‰ª ‡√“„™Èreturn null ‡æ◊ËÕ„ÀÈ‡ø¥ ¡Ÿ∑¢÷Èπ
        }
        fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

        //„πIenumeratorµÕπ ÿ¥∑È“¬parameter‡√“ “¡“√∂„™È MethodÕ◊Ëπ‰¥È ·≈–Subscribe§≈È“¬ÊµÕπ∑”ActionEvent
        onComplete?.Invoke(); //æÕinvoke°Á®–„™ÈMethodπ’È
    }
    private IEnumerator StartGameWithFadeINOUT()
    {
        GameDataManager.instance.ResetCurrentScore(Mission_Manager.instance.currentMission.missionName);
        StartCoroutine(ChangeImageAlpha(1, 1.5f, null)); //fadeIN
        yield return new WaitForSecondsRealtime(1);
        
        SwtichTo(uiInGame.gameObject);
        
        GameManager.instance.GameStart();
        StartCoroutine(ChangeImageAlpha(0, 1.5f, null)); //fadeout
        
        
    }
    public void QuitGame() => Application.Quit();

    public void ResetGameData()
    {
        GameDataManager.instance.ResetGameData();
    }
    public void EnableHardAimMode() => setting_UI.EnableHardMode();

    [ContextMenu("Assign Audio To Button")]
    public void AssignAudioListenerToButtons()
    {
        UI_Button[] buttons = FindObjectsOfType<UI_Button>(true);
        foreach (var button in buttons)
        {
            button.AssignAudioSource();
        }
    }


}
