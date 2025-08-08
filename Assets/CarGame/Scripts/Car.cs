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
    public event Action OnPartOpen;
    public event Action OnPartClose;
    
    public CarType carType;

    [SerializeField] private float _openSpeed;
    
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private Transform _engine;
    [SerializeField] private Transform _trunk;

    public Transform openedPart;

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

        float targetX = partType switch
        {
            PartType.LeftDoor => 90f,
            PartType.RightDoor => -90f,
            PartType.Engine => -60f,
            PartType.Trunk => 60f,
            _ => part.localEulerAngles.x
        };

        Vector3 targetRotation = part.localEulerAngles;
        targetRotation.x = targetX;

        part.DOLocalRotate(targetRotation, _openSpeed).SetEase(Ease.OutBack);
        openedPart = part;
        OnPartOpen?.Invoke();
    }

}
