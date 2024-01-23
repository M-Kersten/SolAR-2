using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FaderView: MonoBehaviour
{
    public Image FadeImage;
    private FaderModel _faderModel;
    
    private void Awake()
    {
        _faderModel = FindObjectOfType<FaderModel>();
        if (_faderModel)
        {
            _faderModel.OnFadeStart += FadeIn;
            _faderModel.OnFullyFaded += FadeOut;
            _faderModel.OnFadeComplete += FadeComplete;
        }
    }

    private void OnDestroy()
    {
        if (_faderModel)
        {
            _faderModel.OnFadeStart -= FadeIn;
            _faderModel.OnFullyFaded -= FadeOut;
            _faderModel.OnFadeComplete -= FadeComplete;
        }
    }

    void FadeIn()
    {
        FadeImage.raycastTarget = true;
        FadeImage
            .DOFade(1, _faderModel.FadeDuration / 2)
            .OnComplete(_faderModel.FullFaded);
    }

    void FadeOut()
    {
        FadeImage
            .DOFade(0, _faderModel.FadeDuration / 2)
            .OnComplete(_faderModel.CompleteFade);
    }

    void FadeComplete()
    {
        FadeImage.raycastTarget = false;
    }
}
