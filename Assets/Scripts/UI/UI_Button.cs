using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("MouseHover Setting")]
    public float scaleSpeed = 5.2f;
    public float scaleRate = 1.2f;

    private Vector3 defaultScale;
    private Vector3 targetScale;

    private Image buttonImage;
    private TextMeshProUGUI buttonText;

    [Header("Audio")]
    [SerializeField] private AudioSource pointerEnterSFX;
    [SerializeField] private AudioSource pointerDownSFX;
    public virtual void Start()
    {
        defaultScale = transform.localScale;
        targetScale = defaultScale;
        buttonImage = GetComponent<Button>().image;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public virtual void Update()
    {
        if (Mathf.Abs(transform.localScale.x - targetScale.x) > .01f)
        {
            float scaleValue =
                Mathf.Lerp(transform.localScale.x, targetScale.x, Time.deltaTime * scaleSpeed);
            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (pointerEnterSFX != null)
            pointerEnterSFX.Play();
        targetScale = defaultScale * scaleRate;
        if (buttonImage != null)
            buttonImage.color = Color.yellow;

        if (buttonText != null)
            buttonText.color = Color.yellow;

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        targetScale = defaultScale;
        ReturnDefaultLook();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (pointerDownSFX != null)
            pointerDownSFX.Play();
        ReturnDefaultLook();
    }
    private void ReturnDefaultLook()
    {

        targetScale = defaultScale;

        if (buttonImage != null)
            buttonImage.color = Color.white;

        if (buttonText != null)
            buttonText.color = Color.white;
    }
    public void AssignAudioSource()
    {
        pointerEnterSFX = GameObject.Find("UI_PointerEnter").GetComponent<AudioSource>();
        pointerDownSFX = GameObject.Find("UI_PointerDown").GetComponent<AudioSource>();
    }
}
