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
    
    [Header("Wheels Settings")]
    [SerializeField] private Transform[] _wheels; 
    [SerializeField] private float _wheelSpinSpeed = 360f; // градусов в секунду
    [SerializeField] private float _movementThreshold = 0.01f; // порог чувствительности

    [Header("Car Parts")]
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private Transform _engine;
    [SerializeField] private Transform _trunk;

    public Transform openedPart;
    public IssueType IssueType;

    private Vector3 _lastPosition;
    private float _direction = 1f; // 1 = вперед, -1 = назад

    private void Start()
    {
        OnPartOpen += UIController.Instance.SetCloseButton;
        OnPartClose += UIController.Instance.SetOpenButton;

        _lastPosition = transform.position;
    }

    private void Update()
    {
        Vector3 delta = transform.position - _lastPosition;
        float distance = delta.magnitude;

        if (distance > _movementThreshold)
        {
            // Определяем направление (вперёд или назад)
            _direction = Vector3.Dot(delta.normalized, transform.forward) >= 0 ? 1f : -1f;

            RotateWheels(_direction * -1);
        }

        _lastPosition = transform.position;
    }

    private void RotateWheels(float direction)
    {
        float rotationAmount = _wheelSpinSpeed * direction * Time.deltaTime;

        foreach (var wheel in _wheels)
        {
            wheel.Rotate(rotationAmount, 0f, 0f, Space.Self);
        }
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
