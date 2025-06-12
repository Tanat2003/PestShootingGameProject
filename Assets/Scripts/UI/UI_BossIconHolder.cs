using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossIconHolder : MonoBehaviour
{
    [SerializeField] private Image[] allBossIcon;

   
    public Image GetBossIcon()
    {
        for (int i = 0; i < allBossIcon.Length; i++)
        {
            if (allBossIcon[i].GetComponent<UI_BossIcon>().bossName ==
               Mission_Manager.instance.currentMission.bossToSpawn.GetComponent<Enemy>().enemyName)
            {
                
                return allBossIcon[i];
            }
        }
        return allBossIcon[0];
    }
}
