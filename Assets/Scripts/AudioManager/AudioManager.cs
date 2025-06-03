using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private AudioSource plrSpeak;
    [SerializeField] private AudioSource ownerSpeak;
    [SerializeField] private bool playbgm;
    private int bgmindex;

    private void Start()
    {
        instance = this;
        PlayBGM(9);
    }
    private void Update()
    {
        if(playbgm == false && BGMIsPlaying())
        {
            StopAllBGM();
        }
        if(playbgm && bgm[bgmindex].isPlaying == false)
        {
            PlayBGM(8);
        }
    }
    public void PlayBGM(int index)
    {
        if (bgm[index] == null)
            return;
        StopAllBGM();

        bgmindex = index;
        bgm[index].Play();
    }
    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
        GameManager.instance.player.inbattle = false;
    }
    private bool BGMIsPlaying()
    {
        for(int i = 0;i < bgm.Length;i++)
        {
            if (bgm[i].isPlaying)
                return true;
        }
        return false;
    }
    [ContextMenu("PlayRandomBGM")]
    public void PlayRandomBGM()
    {
        StopAllBGM();
        bgmindex = Random.Range(0, bgm.Length);
        PlayBGM(bgmindex);
    }
    public void PlayPlrSpeakSound()
    {
        if(ownerSpeak.isPlaying || plrSpeak.isPlaying)
        {
            ownerSpeak.Stop();
            plrSpeak.Stop();

        }
        plrSpeak.Play();
    }
    public void PlayOwnerSpeakSound()
    {
        if (ownerSpeak.isPlaying || plrSpeak.isPlaying)
        {
            ownerSpeak.Stop();
            plrSpeak.Stop();

        }
        ownerSpeak.Play();
    }

    public void PlaySFX(AudioSource sfx, bool randomPitch = false,float minPitch = .85f,float maxPitch=1.1f)
    {
        if (sfx == null)
            return;
        float pitch = Random.Range(minPitch,maxPitch);

        sfx.pitch = pitch;
        sfx.Play();

    }
    public void PlaySFXWithDelayAndFade(AudioSource source,bool play,float targetVolume, float delay = 0,float fadeDuration=1)
    {
        StartCoroutine(SFXDelayAndFade(source,play,targetVolume, delay,fadeDuration));
    }
    private IEnumerator SFXDelayAndFade(AudioSource source,bool play,float targetVolume ,float delay = 0 ,float fadeDuration=1 )
    {
        yield return new WaitForSeconds(delay);

        float startVolume =play? 0 : source.volume;
        float endVolume = play ? targetVolume : 0;
        float elapsed = 0; //เวลาที่ผ่านไป
        if(play)
        {
            source.volume = 0;
            source.Play();
        }
        //Fadein/oute
        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume,endVolume,elapsed/fadeDuration);
            yield return null;
        }
        source.volume = endVolume;
        if(play == false)
        {
            source.Stop();
        }
    }
   
    
}
