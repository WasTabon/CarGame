using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarsController : MonoBehaviour
{
    public static CarsController Instance;
    
    [SerializeField] private Car[] _cars;

    public Car currentCar;

    private Car _tempCar;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InvokeRepeating("ManageCars", 0f, 6f);
    }

    private void Update()
    {
        if (ScenesState.cleanWon)
        {
            ScenesState.cleanWon = false;
            UIController.Instance.HideCameraButtons();
            UIController.Instance.ShowFinishButton();
        }
    }

    public void OpenElement()
    {
        switch (CameraSwitcheController.Instance.CurrentIndex)
        {
            case 0:
                currentCar.OpenPart(PartType.Engine);
                break;
            case 1:
                currentCar.OpenPart(PartType.LeftDoor);
                break;
            case 2:
                currentCar.OpenPart(PartType.Trunk);
                break;
            case 3:
                currentCar.OpenPart(PartType.RightDoor);
                break;
        }
    }
    public void CloseElement()
    {
        currentCar.ClosePart();
    }

    public void ManageCars()
    {
        if (currentCar == null)
        {
            int randomCar = Random.Range(0, _cars.Length);
            
            Car car = _cars[randomCar];
            
            IssueType randomIssue = (IssueType)Random.Range(0, System.Enum.GetValues(typeof(IssueType)).Length);

            car.IssueType = randomIssue;
            
            car.gameObject.GetComponent<Animator>().SetTrigger("Move");
            _tempCar = car;
            
            Invoke("SetCurrentCar", 5f);
        }
    }

    public void Finish()
    {
        CameraSwitcheController.Instance.ToggleDefaultCamera();
        currentCar.gameObject.GetComponent<Animator>().SetTrigger("GoBack");
        Invoke("SetCarNull", 4f);
    }
    
    private void SetCurrentCar()
    {
        currentCar = _tempCar;
        UIController.Instance.ShowCameraButtons();
        CameraSwitcheController.Instance.ToggleDefaultCamera();
    }
    private void SetCarNull()
    {
        currentCar = null;
    }

    public void ScanElement()
    {
        int index = CameraSwitcheController.Instance.CurrentIndex;

        switch (index)
        {
            case 0:
                if (currentCar.IssueType == IssueType.EngineOverheat || 
                    currentCar.IssueType == IssueType.EngineBelt)
                {
                    UIController.Instance.ShowIssue();
                }
                else
                {
                    UIController.Instance.ShowNoIssue();
                }
                break;

            case 1:
            case 3: 
                if (currentCar.IssueType == IssueType.DoorWindow || 
                    currentCar.IssueType == IssueType.DoorLock)
                {
                    UIController.Instance.ShowIssue();
                }
                else
                {
                    UIController.Instance.ShowNoIssue();
                }
                break;

            case 2:
                if (currentCar.IssueType == IssueType.TrunkLock || 
                    currentCar.IssueType == IssueType.TrunkBroken)
                {
                    UIController.Instance.ShowIssue();
                }
                else
                {
                    UIController.Instance.ShowNoIssue();
                }
                break;
        }
    }
}
