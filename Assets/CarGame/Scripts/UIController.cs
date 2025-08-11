using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    
    public RectTransform _openButton;
    public RectTransform _closeButton;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCloseButton()
    {
        
    }
    public void SetOpenButton()
    {
        
    }
}
