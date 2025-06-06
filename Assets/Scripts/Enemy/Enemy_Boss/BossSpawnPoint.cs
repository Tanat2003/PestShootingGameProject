using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BossSpawnPoint : MonoBehaviour
{
    [Header("Boss Spawn Point")]
    public Transform[] spawnPoints;

    [Header("SapawnPoint")]
    [SerializeField] private float spawnCheckRadius;
    [SerializeField] private LayerMask whatToIgnoreForSpawn;
    


    public Transform GetClearSpawnPoint()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (IsSpawnPointClear(spawnPoints[i].position))
            {
                return spawnPoints[i].transform;
            }
        }
        return null;
    }
    private bool IsSpawnPointClear(Vector3 point)
    {
        Collider[] colliders
            = Physics.OverlapSphere(point, spawnCheckRadius, ~whatToIgnoreForSpawn);
        return colliders.Length == 0;
    }

    private void OnDrawGizmos()
    {
        if(spawnPoints.Length > 0)
        {
            foreach (var point in spawnPoints)
            {
                Gizmos.DrawWireSphere(point.position, spawnCheckRadius);
            }
        }
    }



}
