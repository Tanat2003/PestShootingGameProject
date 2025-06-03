using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ComicPanel : MonoBehaviour
{
    [SerializeField] private Image loadingImgae;
    [SerializeField] private GameObject buttonToEnable;

    private int imageIndex;
    private Image myImage;
    private bool comicShowOver;

    private void Start()
    {
        myImage = GetComponent<Image>();
        ShowLoadingImage();
    }
    private void ShowLoadingImage()
    {
       
        StartCoroutine(ChangeImageAlpha(3));
    }
    private IEnumerator ChangeImageAlpha(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            
            time += 1;
            
            loadingImgae.color = new Color(1, 1, 1, 0);

            yield return new WaitForSeconds(1);

            loadingImgae.color = new Color(1, 1, 1, 1);

            yield return new WaitForSeconds(1);
        }
        
        FinishImageShow();
    }

    private void FinishImageShow()
    {
        StopAllCoroutines();
        comicShowOver = true;
        buttonToEnable.SetActive(true);
        myImage.raycastTarget = false;
    }
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    if (imageIndex >= loadingImgae.Length)
    //    {
    //        return;
    //    }
    //    ShowNextImageOnClick();
    //}

    //private void ShowNextImageOnClick()
    //{
        
    //    loadingImgae[imageIndex].color = Color.white;
    //    imageIndex++;
    //    if(imageIndex < loadingImgae.Length)
    //    {
    //        FinishImageShow();
    //    }
    //    if (comicShowOver)
    //    {
    //        return;
    //    }
    //    ShowNextImage();
    //}
}
