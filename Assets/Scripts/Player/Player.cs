using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControll controls { get; private set; }

    public Player_Aim aim { get; private set; } //get private set คืออ่านได้อย่างเดียวเท่านั้น
    public Player_Movement movement { get; private set; }
    public Player_WeaponController weapon { get; private set; }
    public Player_WeaponVisual weaponVisuals { get; private set; }
    public Player_Interaction interaction { get; private set; }
    public Player_Health health { get; private set; }
    public Ragdoll ragdoll { get; private set; }
    public Animator animator { get; private set; }
    public Player_SFX sfx { get; private set; }
    public bool controlEnable { get; private set; }
    public bool inbattle;

    private void Awake() //เข้าถึงสคริปต์ต่างๆ
    {

        sfx = GetComponent<Player_SFX>();
        animator = GetComponentInChildren<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        health = GetComponent<Player_Health>();
        aim = GetComponent<Player_Aim>();
        movement = GetComponent<Player_Movement>();
        weapon = GetComponent<Player_WeaponController>();
        weaponVisuals = GetComponent<Player_WeaponVisual>();
        interaction = GetComponent<Player_Interaction>();
        controls = ConTrolManager.instance.controls;
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Character.UIMissionInfo.performed += ctx
            => UI.instance.uiInGame.TurnOnOffMissionInfo();
        controls.Character.UIPause.performed += ctx =>
        {
            if(UI.instance.uiInGame.canPauseGame())
            {
                UI.instance.PauseSwitch();
            }
            
        }; 
        controls.Character.Minimap.performed += ctx => MinimapController.instance.ShowAndHideMinimap();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    public void SetControlEnable(bool enable)
    {

        controlEnable = enable;
        ragdoll.ColliderActive(enable); //ถ้าเราไม่ปิดColliderของผู้เล่นจะบัค
        aim.EnableAimLase(enable);
    }


}
