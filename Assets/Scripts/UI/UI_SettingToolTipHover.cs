using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SettingToolTipHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler
{
    [SerializeField] private GameObject tooltip;

    [Header("Audio")]
    [SerializeField] private AudioSource pointerEnterSFX;
    [SerializeField] private AudioSource pointerDownSFX;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pointerDownSFX != null)
            pointerDownSFX.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pointerEnterSFX != null)
                pointerEnterSFX.Play();
        if(tooltip != null)
            tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(tooltip != null)       
            tooltip.SetActive(false);
    }
}
