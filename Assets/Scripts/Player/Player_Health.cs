public class Player_Health : HealthController
{
    private Player player;


    public bool isDead { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }
    private void Start()
    {
        UI.instance.uiInGame.UpdatePlayerHealth(currentHealth,maxHealth);
    }
    public override void ReduceHealth(int damage)
    {
        base.ReduceHealth(damage);
        UI.instance.uiInGame.DisplayDamageScreen(.5f);

        if (ShouldDie())
        {
            Die();
        }

        UI.instance.uiInGame.UpdatePlayerHealth(currentHealth, maxHealth);
        PlayBGMReadyTofight();
    }
    public override void IncreaseHealth(int heal)
    {
        base.IncreaseHealth(heal);
        UI.instance.uiInGame.UpdatePlayerHealth(currentHealth, maxHealth);

    }
    private void PlayBGMReadyTofight()
    {
        if (player.inbattle == true)
        {
            return;
        }
        AudioManager.instance.PlayBGM(10);
        player.inbattle = true;
    }

   
    
    
    
    private void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        player.animator.enabled = false;
        player.ragdoll.RagdollActive(true);

        GameManager.instance.GameOver();
    }

}
