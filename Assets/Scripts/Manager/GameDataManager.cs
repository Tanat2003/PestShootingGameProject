using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Playables;

[System.Serializable]
public class ScoreData
{
    public int enemiesKilled = 0;
    public Dictionary<string, int> itemsCollected = new Dictionary<string, int>();
}

[System.Serializable]
public class MissionData
{
    public ScoreData bestScore = new ScoreData();
    public ScoreData currentScore = new ScoreData();
}

[System.Serializable]
public class GameData
{
    public Dictionary<string, MissionData> missions = new Dictionary<string, MissionData>();
   
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    private string filePath;
    private GameData gameData;
    private Player_WeaponController weaponController;

    void Awake()
    {
        instance = this;
        filePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        LoadGameData();
    }
    private void Start()
    {
        weaponController = GameManager.instance.player.GetComponent<Player_WeaponController>();
    }
    public GameData GetGameData()
    {
        return gameData;
    }
    private void LoadGameData()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                
                gameData = JsonConvert.DeserializeObject<GameData>(json);

                if (gameData == null)
                {
                    Debug.LogWarning("JSON data is empty or corrupted. Creating new GameData.");
                    gameData = new GameData();
                }
            }
            else
            {
                Debug.Log("JSON File Didn't Exist, Creating New One");
                gameData = new GameData();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading game data: " + e.Message);
        }
    }

    private void SaveGameData()
    {
        try
        {
            
            string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving game data: " + e.Message);
        }
    }

    public void EnemyKilled(string currentMission)
    {
        if (!gameData.missions.ContainsKey(currentMission))
        {
            gameData.missions[currentMission] = new MissionData();
        }

        gameData.missions[currentMission].currentScore.enemiesKilled++;

        if (gameData.missions[currentMission].currentScore.enemiesKilled > gameData.missions[currentMission].bestScore.enemiesKilled)
        {
            gameData.missions[currentMission].bestScore.itemsCollected = gameData.missions[currentMission].currentScore.itemsCollected;
            gameData.missions[currentMission].bestScore.enemiesKilled = gameData.missions[currentMission].currentScore.enemiesKilled;
        }

        SaveGameData();
    }

    public void ItemCollected(string itemName, string currentMission)
    {
        if (!gameData.missions.ContainsKey(currentMission))
        {
            gameData.missions[currentMission] = new MissionData();
        }

        if (!gameData.missions[currentMission].currentScore.itemsCollected.ContainsKey(itemName))
        {
            gameData.missions[currentMission].currentScore.itemsCollected[itemName] = 0;
        }

        gameData.missions[currentMission].currentScore.itemsCollected[itemName]++;
        SaveGameData();
    }

    public void ResetCurrentScore(string currentMission)
    {
        if (!gameData.missions.ContainsKey(currentMission))
        {
            gameData.missions[currentMission] = new MissionData();
        }

        gameData.missions[currentMission].currentScore = new ScoreData();
        SaveGameData();
    }

    public void ResetGameData()
    {
        //Reset All PlayerPrefs มีตรงตั้งค่ายิงยาก ระดับเสียง และการเปิดปิดเอฟเฟคอัพเกรดปืน
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        //ปิดEffect Upgradeปืน และ Resetค่าต่างๆที่อัพเกรดไปให้เป็น Default รีเซ็ทปืนที่เคยเก็บด้วย
        weaponController.ResetUpgrade();
        weaponController.ResetUnlocked();

        //ลบไฟล์Jsonที่เก็บข้อมูลแต่การกำจัดศัตรูและการเก็บไอเทมในแต่ละด่าน
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            
        }
        gameData = new GameData(); // สร้างข้อมูลใหม่
        LoadGameData();
        SaveGameData();

    }
}
