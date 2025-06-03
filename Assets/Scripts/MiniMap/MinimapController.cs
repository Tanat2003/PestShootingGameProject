using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [Header("References")]
    private Transform player;
    private Player_Movement playerMovement;
    [SerializeField] private RectTransform minimapIconParent;
    [SerializeField] private Image playerIcon;
    [SerializeField] private GameObject enemyIconPrefab;
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private RawImage minimapRawImage;


    private bool activeminimap;



    public static MinimapController instance;
    private Dictionary<Transform, GameObject> enemyIcons = new();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = GameManager.instance.player.transform;
        playerMovement = player.GetComponent<Player_Movement>();
        GetEnemyPosition();
    }

    public void GetEnemyPosition() //เรียกใช้ตอนสร้างLevelPartเสร็จ
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemyIcons.ContainsKey(enemy.transform))
                continue;

            GameObject icon = Instantiate(enemyIconPrefab, minimapIconParent);
            enemyIcons.Add(enemy.transform, icon);
        }
        activeminimap = true;
    }


    private void Update()
    {
        if (!activeminimap)
            return;

        // อัพเดทตำแหน่งไอค่อนผู้เล่น
        Vector2 playerPos = WorldToMinimapPosition(player.position);
        playerIcon.rectTransform.anchoredPosition = playerPos;

        //List<Transform> toRemove = new();

        // อัพเดทตำแหน่งไอค่อนศัตรู
        foreach (var pair in enemyIcons)
        {
            if (pair.Key == null) continue;

            Transform enemy = pair.Key;
            GameObject icon = pair.Value;

            if (!enemy.gameObject.activeInHierarchy)
            {
                // Enemy ถูกปิด (SetActive false)

                icon.SetActive(false);
                continue;
            }

            Vector3 viewportPos = minimapCamera.WorldToViewportPoint(enemy.position);

            // เช็คว่าอยู่ในขอบ viewport หรือไม่ (0 - 1)
            bool isVisible = viewportPos.z > 0 &&
                             viewportPos.x >= 0 && viewportPos.x <= 1 &&
                             viewportPos.y >= 0 && viewportPos.y <= 1;

            icon.SetActive(isVisible);

            if (isVisible)
            {
                Vector2 iconPos = ViewportToUIPosition(viewportPos);
                icon.GetComponent<RectTransform>().anchoredPosition = iconPos;
            }
        }
        //foreach (var enemy in toRemove)
        //{
        //    enemyIcons.Remove(enemy);
        //}

    }

    private Vector2 WorldToMinimapPosition(Vector3 worldPos)
    {
        Vector3 viewportPos = minimapCamera.WorldToViewportPoint(worldPos);

        // Viewport (0..1) → RectTransform anchoredPosition (centered at 0,0)
        RectTransform rt = minimapRawImage.rectTransform;
        float x = (viewportPos.x - 0.5f) * rt.rect.width;
        float y = (viewportPos.y - 0.5f) * rt.rect.height;

        return new Vector2(x, y);
    }
    private Vector2 ViewportToUIPosition(Vector3 viewportPos)
    {
        RectTransform rt = minimapRawImage.rectTransform;
        float x = (viewportPos.x - 0.5f) * rt.rect.width;
        float y = (viewportPos.y - 0.5f) * rt.rect.height;
        return new Vector2(x, y);
    }

    public void ShowAndHideMinimap()
    {
        activeminimap = !activeminimap;

        this.gameObject.SetActive(activeminimap);
    }
    private void OnDisable()
    {
        activeminimap = false;
    }
}
