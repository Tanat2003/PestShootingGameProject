using UnityEngine;

public class MissionEndTrigger : MonoBehaviour
{
    private GameObject player;
    private Mission currentMission;
    private void Start()
    {
        player = GameObject.Find("Player");
        currentMission = Mission_Manager.instance.currentMission;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player)
        {
            return;
        }
        if (Mission_Manager.instance.currentMission.MissionCompleted())
        {
            
            


            GameManager.instance.GameCompleted();
        }
            

    }




}
