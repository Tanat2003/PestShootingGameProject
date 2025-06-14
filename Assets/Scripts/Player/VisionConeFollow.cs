using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeFollow : MonoBehaviour
{
    private Transform player;
    public float distance = 3f;
    public float width = 1.5f;
    [SerializeField] private Color normalColor = new Color(1, 1, 1, 0.3f);
    [SerializeField] private Color alertColor = new Color(1, 0, 0, 0.5f);
    public LayerMask enemyLayer;
    private Material coneMat;

    void Start()
    {
        player = GameManager.instance.player.transform;
        coneMat = GetComponent<Renderer>().material;
        coneMat.SetColor("_OverlayColor", normalColor);
    }
    private void Update()
    {
        if (player == null)
            return;

        transform.position = player.position+player.forward*(distance/2f);
        transform.rotation = Quaternion.LookRotation(player.forward,Vector3.up);
        transform.localScale = new Vector3 (width,.01f,distance);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            coneMat.SetColor("_OverlayColor", alertColor);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            coneMat.SetColor("_OverlayColor", normalColor);
        }
    }
}
