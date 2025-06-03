using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Setting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float sliderMultiplier = 25f;
    [Header("SFX Setting")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxSliderText;
    [SerializeField] private string sfxParameter;


    [Header("BGM Setting")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmSliderText;
    
    [SerializeField] private string bgmParameter;

    [Header("Toggle")]
    [SerializeField] private Toggle hardMode;
    


    public void SFXSliderValue(float value)
    {
        sfxSliderText.text = Mathf.RoundToInt(value * 100) + " %";
        float newValue = Mathf.Log10(value) * sliderMultiplier; // „πAudio Mixervalue¡—π‡ªÁπ -80 ∂÷ß20 ‡√“‡≈¬‰¥È Ÿµ√π’È¡“
        audioMixer.SetFloat(sfxParameter, newValue);
        
    }
    public void BGMSliderValue(float value)
    {
        bgmSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * sliderMultiplier;
        audioMixer.SetFloat(bgmParameter, newValue);
    }
    public void EnableHardMode()
    {
        bool lockMode = Player_Aim.instance.isLockingToTarget;
        Player_Aim.instance.isLockingToTarget = !lockMode;

        bool freeAim = Player_Aim.instance.isAimingPrecisly;
        Player_Aim.instance.isAimingPrecisly = !freeAim;
    }

    
    public void LoadSetting()
    {
        sfxSlider.value =PlayerPrefs.GetFloat(sfxParameter,.7f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, .7f);

        int freeAimInt = PlayerPrefs.GetInt("FreeAim", 0);
        bool newfreeAim;
        if(freeAimInt ==1)
        {
            newfreeAim =true;

        }
        else
        {
            newfreeAim =false;
        }
        
        hardMode.isOn = newfreeAim;

        

    }
    private void OnDisable()
    {
        //SetData On UI Disable
        bool freeAim = Player_Aim.instance.isAimingPrecisly;
        int hardModeInt =  freeAim ? 0 : 1;

        

        
        PlayerPrefs.SetInt("FreeAim",hardModeInt);
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter , bgmSlider.value);
    }
}
