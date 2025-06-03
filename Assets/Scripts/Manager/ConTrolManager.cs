using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConTrolManager : MonoBehaviour
{
    public static ConTrolManager instance;
    public PlayerControll controls { get; private set; }
    private Player player;

    private void Awake()
    {
        instance = this;
        controls = new PlayerControll();
    }

    private void Start()
    {
        player = GameManager.instance.player;
        SwitchToCharacterControls();
    }

    public void SwitchToCharacterControls()
    {
        controls.Character.Enable();

        controls.UI.Disable();
        controls.Car.Disable();
        player.SetControlEnable(true);
        UI.instance.uiInGame.SwitchToCharacterUI();
    }
    public void SwtichToUIControls()
    {
        controls.UI.Enable();

        controls.Character.Disable();
        controls.Car.Disable();
        player.SetControlEnable(false);
    }
    public void SwitchToCarConTrols()
    {
        controls.Car.Enable();

        controls.UI.Disable();
        controls.Character.Disable();
        player.SetControlEnable(false);
        UI.instance.uiInGame.SwtichToCarUI();
    }

}
