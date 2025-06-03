using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    private float targetTimeScale = 1f;
    private float timeAdjustRate; //�������繴�����㹡��������ҡ�Ѻ���Թ

    [SerializeField] private float resumeRate = 3;//timeadjustRate�͹������������
    [SerializeField] private float pauseRate = 7; //timeadjustRate�͹��ش����
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        
        if(Mathf.Abs(Time.timeScale -targetTimeScale)>.05f)
        {   
            float adjustRate = Time.unscaledDeltaTime * timeAdjustRate;
            //UnscaleDeltatime����¡��ԡ�����TimeScale�����������TimeAdjustRate���᷹
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, adjustRate);
        }else
        {
            Time.timeScale = targetTimeScale;
        }
    }
    public void PauseTime()
    {
        timeAdjustRate = pauseRate;
        targetTimeScale = 0;
    }
    public void ResumeTime()
    {
        timeAdjustRate = resumeRate;
        targetTimeScale = 1;
    }
    public void SlowMotion(float second) =>StartCoroutine(SlowTime(second));
    private IEnumerator SlowTime(float second)
    {
        targetTimeScale = .5f;
        Time.timeScale = targetTimeScale;
        yield return new WaitForSecondsRealtime(second); //WaitRealTime�ѹ�����ԴTimeScale
        ResumeTime();
    }
}
