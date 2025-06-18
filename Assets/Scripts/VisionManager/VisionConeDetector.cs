using UnityEngine;

public class VisionConeDetector : MonoBehaviour
{

    
    private void OnTriggerEnter(Collider other)
    {
        VisionFade fade = other.GetComponent<VisionFade>()?? GetComponentInChildren<VisionFade>()??GetComponentInParent<VisionFade>();
        if (fade != null)
        {
            Debug.Log("Enter" + other.gameObject.name);
            fade.SetDark(false);
            
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        VisionFade fade = other.GetComponent<VisionFade>() ?? GetComponentInChildren<VisionFade>();
        if (fade != null)
        {
            fade.SetDark(true);
            

        }
    }
}
