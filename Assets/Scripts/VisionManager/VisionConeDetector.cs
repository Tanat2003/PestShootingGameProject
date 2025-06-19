using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeDetector : MonoBehaviour
{
    [Header("WeUseLayerAndVisionFadeToDetect")]
    [SerializeField] private LayerMask targetLayers;

    [SerializeField] private float fadeOutDelay = 0.2f;

    // ตรวจจับว่าศัตรูอยู่ในโซนหรือไม่ และเวลาเหลืออยู่กี่วิ
    private Dictionary<GameObject, float> visibleTargets = new Dictionary<GameObject, float>();
    private void Start()
    {
        StartCoroutine(CheckFadeOutLoop());
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsValidTarget(other)) return;

        GameObject root = other.transform.root.gameObject;
        if (!visibleTargets.ContainsKey(root))
        {
            if (root.TryGetComponent(out VisionFade fade))
                fade.SetDark(false); 
        }

        visibleTargets[root] = fadeOutDelay; // ต่ออายุการมองเห็น
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!IsValidTarget(other))
    //        return;
    //    VisionFade fade = other.transform.root.GetComponentInChildren<VisionFade>() ?? other.GetComponent<VisionFade>();
    //    if (fade != null && !enemiesInZone.Contains(fade.gameObject))
    //    {
    //        enemiesInZone.Add(fade.gameObject);
    //        fade.SetDark(false);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!IsValidTarget(other))
    //        return;
    //    VisionFade fade = other.transform.root.GetComponentInChildren<VisionFade>() ?? other.GetComponent<VisionFade>();
    //    if (fade != null && enemiesInZone.Contains(fade.gameObject))
    //    {
    //        enemiesInZone.Remove(fade.gameObject);
    //        fade.SetDark(true);
    //    }
    //}

    private bool IsValidTarget(Collider col) => ((1 << col.gameObject.layer) & targetLayers) != 0;

    private IEnumerator CheckFadeOutLoop()
    {
        var wait = new WaitForSeconds(0.1f);
        while (true)
        {
            yield return wait;

            var toFadeOut = new List<GameObject>();
            var keys = new List<GameObject>(visibleTargets.Keys);

            foreach (var obj in keys)
            {
                visibleTargets[obj] -= 0.1f;
                if (visibleTargets[obj] <= 0f)
                {
                    toFadeOut.Add(obj);
                }
            }

            foreach (var obj in toFadeOut)
            {
                if (obj != null && obj.TryGetComponent(out VisionFade fade))
                {
                    fade.SetDark(true); 
                }

                visibleTargets.Remove(obj);
            }
        }
    }

}
