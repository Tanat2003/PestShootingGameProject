using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Delivery Mission", menuName = ("Mission/Delivery Mission"))]
public class Mission_CarDelivery : Mission
{
    
    private bool carDelivered;
    public override void StartMission()
    {
        if (GameManager.instance.playerIsInTurtorial == true)
        {
            SetMissionDetail();
            return;
        }
        LevelGenerator.Instance.DisableUselessEnemy(enemyToEnable);
        FindObjectOfType<Mission_ObjectCarDeliveryZone>(true).gameObject.SetActive(true);

        

        SetMissionDetail();

        carDelivered = false;
        Mission_ObjectCarToDeliver.OnCarDelivery += CarDeliveryCompleted;
        Car_Controller[] cars = FindObjectsOfType<Car_Controller>(true);

        foreach (Car_Controller car in cars) //����ʤ�Ի����ŧ�ö�������������Ѻ��Ҩش������
        {
            car.AddComponent<Mission_ObjectCarToDeliver>();
            car.gameObject.SetActive(true);
        }

    }

    private static void SetMissionDetail(bool playerInTurtorial=false)
    {
        playerInTurtorial = GameManager.instance.playerIsInTurtorial;
        string missionText;
        string missionDetail;
        if(playerInTurtorial)
        {
             missionText = "�ԧ�ء���ҧ����ҧ˹������ !";
             missionDetail = "�����������¹������� HP ��� ��ШС�Ѻ������������";
        }
        else
        {
             missionText = "��ö�����ҹ�������ѧ�ش����";
            missionDetail = "�Ѻö��ѧ�ç�բ���";
        }



        UI.instance.uiInGame.UpdateMissionInfo(missionText, missionDetail);
    }

    public override bool MissionCompleted()
    {
        return carDelivered;
    }

    private void CarDeliveryCompleted()
    {
        
        carDelivered = true;
        Mission_ObjectCarToDeliver.OnCarDelivery -= CarDeliveryCompleted;
        UI.instance.uiInGame.UpdateMissionInfo("��áԨ�������");

        UI_MissionSelection.instance.missionCheck.missionCompleted[4] = true;

        GameManager.instance.GameCompleted();
    }

    public override void ResetMissionValue()
    {
        
    }
}
