using System;
using DG.Tweening;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    
    public RectTransform _openButton;
    public RectTransform _closeButton;
    public RectTransform _nextButton;
    public RectTransform _previousButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _closeButton.localScale = Vector3.zero;
    }

    public void SetCloseButton()
    {
        _openButton.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete((() =>
            {
                _closeButton.DOScale(Vector3.one, 0.5f)
                    .SetEase(Ease.OutBack);
            }));
    }
    public void SetOpenButton()
    {
        _closeButton.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete((() =>
            {
                _openButton.DOScale(Vector3.one, 0.5f)
                    .SetEase(Ease.OutBack);
            }));
    }
}
