using System.ComponentModel;
using UnityEngine;


public enum EquipType { SideEquipAnimation, BackEquipAnimation }; //»ÃÐ¡ÒÈ¤èÒ¤§·Õèà»ç¹enum SideGrab=0 BackGrab=1
public enum HoldType { CommonHold = 1, LowHold, HighHold }//à»ÅÕèÂ¹index¢Í§enumà¾ÃÒÐàÃÒ¨ÐãËé¡ÓË¹´LayerIndex¢Í§Í¹ÔàÁªÑè¹
public class WeaponModel : MonoBehaviour
{
    
    public WeaponType weaponType;
    public EquipType equipAnimationType;
    public HoldType holdType;

    public Transform gunPoint; //µÓáË¹è§¡ÃÐÊØ¹
    public Transform holdPoint; //lefthandIK
    [Header("SFX")]
    public AudioSource fireSFX;
    public AudioSource reloadSFX;
    [Header("WeaponBuffFX")]
    public ParticleSystem[] attackBuffFX;
    public ParticleSystem[] magazineBuffFX;

    public void PlayUpgradeFX()
    {
        foreach (var particle in attackBuffFX)
        {
            if (PlayerPrefs.GetInt(particle.name, 0) == 1)
            {
                particle.gameObject.SetActive(true);
                particle.Play();

            }
        }
        foreach (var particle in magazineBuffFX)
        {
            if (PlayerPrefs.GetInt(particle.name, 0) == 1)
            {
                particle.gameObject.SetActive(true);
                particle.Play();

            }
        }
    }
    public void SetUpgradeFX(int index, int particleArray)
    {
        ParticleSystem[] particles = null;
        if (particleArray == 0)
        {
            particles = attackBuffFX;
        }
        else if (particleArray == 1)
        {
            particles = magazineBuffFX;
        }


        if (index >= particles.Length)
            return;
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].gameObject.SetActive(false);
            PlayerPrefs.SetInt(particles[i].name, 0);
        }
        if (particles[index] != null)
        {
            particles[index].gameObject.SetActive(true);
            particles[index].Play();
            PlayerPrefs.SetInt(particles[index].name, 1);



        }


    }

    public void ResetUpgradeFX()
    {
        foreach (var particle in attackBuffFX)
        {
            if (particle != null)
            {
                PlayerPrefs.SetInt(particle.name, 0);
                particle.gameObject.SetActive(false);

            }

        }
        foreach (var particle in magazineBuffFX)
        {
            if (particle != null)
            {

                PlayerPrefs.SetInt(particle.name, 0);
                particle.gameObject.SetActive(false);
            }

        }
    }







}
