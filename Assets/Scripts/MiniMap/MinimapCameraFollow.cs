using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    private Transform player;
    private void Start()
    {
        player = GameManager.instance.player.transform;
    }
    void LateUpdate()
    {
        if (player == null) return;

        Vector3 newPos = player.position;
        newPos.y = transform.position.y; // ≈ÁÕ§§«“¡ Ÿß
        transform.position = newPos;
    }
}
