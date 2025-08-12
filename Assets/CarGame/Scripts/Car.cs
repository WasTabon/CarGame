using System;
using DG.Tweening;
using UnityEngine;

public enum CarType
{
    Yellow,
    Pink,
    Black,
    Purple,
    Red
}
public enum PartType
{
    LeftDoor,
    RightDoor,
    Engine,
    Trunk,
}

public class Car : MonoBehaviour
{
    public event Action<PartType> OnPartOpen;
    public event Action OnPartClose;
    
    public CarType carType;

    [SerializeField] private float _openSpeed;
    
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private Transform _engine;
    [SerializeField] private Transform _trunk;

    public Transform openedPart;

    public IssueType IssueType;

    private void Start()
    {
        OnPartOpen += UIController.Instance.SetCloseButton;
        OnPartClose += UIController.Instance.SetOpenButton;
    }

    public void OpenPart(PartType partType)
    {
        Transform part = partType switch
        {
            PartType.LeftDoor => _leftDoor,
            PartType.RightDoor => _rightDoor,
            PartType.Engine => _engine,
            PartType.Trunk => _trunk,
            _ => null
        };

        Vector3 targetRotation = part.localEulerAngles;

        if (partType == PartType.LeftDoor)
        {
            if (carType == CarType.Yellow)
                targetRotation.x = 90f;
            else
                targetRotation.y = 85f;
        }
        else if (partType == PartType.RightDoor)
        {
            if (carType == CarType.Yellow)
                targetRotation.x = -90f;
            else
                targetRotation.y = -85f;
        }
        else if (partType == PartType.Engine)
        {
            targetRotation.x = -60f;
        }
        else if (partType == PartType.Trunk)
        {
            targetRotation.x = (carType == CarType.Black) ? 23f : 60f;
        }

        part.DOLocalRotate(targetRotation, _openSpeed).SetEase(Ease.OutBack);
        openedPart = part;
        OnPartOpen?.Invoke(partType);
    }

    public void ClosePart()
    {
        if (openedPart == null)
            return;
        
        Vector3 targetRotation = openedPart.localEulerAngles;
        targetRotation.x = 0f;
        if (carType != CarType.Yellow)
            targetRotation.y = 0f;
        openedPart.DOLocalRotate(targetRotation, _openSpeed).SetEase(Ease.InBack);
        OnPartClose?.Invoke();
    }

}
