using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    [SerializeField] private GameObject[] rice1;
    [SerializeField] private GameObject[] rice2;
    [Header("Quick Testing")]
    public bool quickStart;
    [SerializeField] private List<WeaponData> stratweaponfortest;

    [Header("For MoveAround Player")]
    [SerializeField] private GameObject levelStart;
    [SerializeField] private GameObject levelTurtorial;
    public bool playerIsInTurtorial;

    private void Awake()
    {
        instance = this;
        player = Object.FindObjectOfType<Player>();
    }

    public void GameStart()
    {
        if (quickStart == true)
        {
            player.weapon.SetDefaultWeapon(stratweaponfortest);
            MovePlayerToLevelStart();
            return;
        }
        SetDefaultWeapons();
        

        //เราเริ่มภารกิจตอนที่สร้างlevelPartเสร็จใน LevelGenerator 
    }
    private void SetDefaultWeapons()
    {
       
        List<WeaponData> newList = UI.instance.weaponSelection.SelectedWeapon();
        player.weapon.SetDefaultWeapon(newList);
    }
    public void RestartScene()
    {
        Mission_Manager.instance.currentMission.ResetMissionValue();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    public void GameOver()
    {
        TimeManager.instance.SlowMotion(2);
        AudioManager.instance.PlayBGM(11);
        UI.instance.ShowGameOverUI();
        CameraManager.instance.ChangeCameraDistance(4);
        
    }
    public void GameCompleted()
    {
        UI.instance.ShowVictoryScreenUI();
        ConTrolManager.instance.controls.Character.Disable();
        player.health.currentHealth += 99999;

    }
    public void EnableRice()
    {
        string mission = Mission_Manager.instance.currentMission.missionName;

        switch (mission)
        {
            case "นำส่งผลผลิต":
            case "การเก็บเกี่ยว":
                SetRiceState(false, true);
                break;

            case "การปลูกข้าว":
            case "การดูแลต้นกล้า":
                SetRiceState(true, false);
                break;
        }
    }

    private void SetRiceState(bool rice1State, bool rice2State)
    {
        foreach (var r in rice1) r.gameObject.SetActive(rice1State);
        foreach (var r in rice2) r.gameObject.SetActive(rice2State);
    }

    [ContextMenu("MovePlayerToLevelStart")]
    public void MovePlayerToLevelStart()
    {
        player.transform.parent = levelStart.transform;
        player.transform.position = levelStart.transform.position;
        player.transform.parent = null;
        playerIsInTurtorial = false;
    }

    public void MovePlayerToTuTorial()
    {
        player.transform.parent = levelTurtorial.transform;
        player.transform.position = levelTurtorial.transform.position;
        player.transform.parent = null;
        playerIsInTurtorial = true;
    }
}
