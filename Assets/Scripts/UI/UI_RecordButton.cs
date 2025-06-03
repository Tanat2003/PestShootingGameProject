using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_RecordButton : MonoBehaviour, IPointerEnterHandler
{
    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI currentScoreText;
    public GameObject infoPanel;
    

    private void Start()
    {
        infoPanel.SetActive(false);
    }


    public void ShowMissionData(string missionName)
    {
        GameDataManager gameDataManager = GameDataManager.instance;
        
        if (gameDataManager == null)
        {

            return;
        }

        GameData gameData = gameDataManager.GetGameData();
        if (gameData == null || !gameData.missions.ContainsKey(missionName))
        {
            Debug.LogWarning($"Mission '{missionName}' not found!");
            return;
        }

        // ดึงข้อมูลของเควสที่เลือก
        MissionData missionData = gameData.missions[missionName];

        // อัปเดต UI
        missionNameText.text = "ภารกิจ: " + missionName;
        bestScoreText.text = $"รอบที่ดีที่สุด สังหารไปทั้งหมด : {missionData.bestScore.enemiesKilled} ตัว\n" +
                             $"และได้เก็บไอเทม : \n" + GetItemList(missionData.bestScore.itemsCollected);
        currentScoreText.text = $"รอบล่าสุด สังหารไปทั้งหมด : {missionData.currentScore.enemiesKilled} ตัว\n" +
                                $"และได้เก็บไอเทม : \n" + GetItemList(missionData.currentScore.itemsCollected);

        infoPanel.SetActive(true);
    }
    private void OnDisable()
    {
        infoPanel.SetActive(false);
    }

    private string GetItemList(System.Collections.Generic.Dictionary<string, int> items)
    {
        if (items == null || items.Count == 0) return "";

        string itemList = " :\n";
        foreach (var item in items)
        {
            itemList += $"{item.Key} : {item.Value}\n";
        }
        return itemList;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
       
    }
}


