using UnityEngine;

public class VisionConeDetector : MonoBehaviour
{

    
    private void OnTriggerEnter(Collider other)
    {
        VisionFade fade = other.GetComponent<VisionFade>();
        if (fade != null)
        {
            fade.SetDark(false);
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        VisionFade fade = other.GetComponent<VisionFade>();
        if (fade != null)
        {
            fade.SetDark(true);
            

        }
    }
}
