using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Sound : MonoBehaviour
{
    private Car_Controller car;
    [SerializeField] private float engineVolume = .75f;
    [SerializeField] private AudioSource engineStart;
    [SerializeField] private AudioSource engineOff;
    [SerializeField] private AudioSource engineWork;

    private float minSpeed = 0;
    private float maxSpeed = 10;

    public float minPitch = .75f;
    public float maxPitch = 1.5f;
    public bool allowCarSound;

    private void Start()
    {
        car = GetComponent<Car_Controller>();
        Invoke(nameof(AllowCarSound),1);
    }
    private void Update()
    {
        UpdateEngineSound();
    }
    public void ActiveCarSFX(bool activate)
    {
        if(activate)
        {
            engineStart.Play();
            AudioManager.instance.PlaySFXWithDelayAndFade(engineWork, true, .75f, 1);
        }else
        {

            AudioManager.instance.PlaySFXWithDelayAndFade(engineWork, false, .75f,.25f,0);
            engineOff.Play();
        }
    }

    private void AllowCarSound() => allowCarSound = true;
    private void UpdateEngineSound()
    {
        float currentSpeed = car.speed;
        float pitch = Mathf.Lerp(minPitch,maxPitch, currentSpeed/maxSpeed);
        engineWork.pitch = pitch;
    }

}
