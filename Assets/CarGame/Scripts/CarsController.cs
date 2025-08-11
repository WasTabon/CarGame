using UnityEngine;

public class CarsController : MonoBehaviour
{
    public static CarsController Instance;
    
    [SerializeField] private GameObject[] _cars;

    public Car currentCar;

    private void Awake()
    {
        Instance = this;
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

    public void ScanElement()
    {
        int index = CameraSwitcheController.Instance.CurrentIndex;

        switch (index)
        {
            case 0:
                if (currentCar.IssueType == IssueType.EngineOverheat || 
                    currentCar.IssueType == IssueType.EngineBelt)
                {
                    Debug.Log("Issue Found");
                }
                break;

            case 1:
            case 3: 
                if (currentCar.IssueType == IssueType.DoorWindow || 
                    currentCar.IssueType == IssueType.DoorLock)
                {
                    Debug.Log("Issue Found");
                }
                break;

            case 2:
                if (currentCar.IssueType == IssueType.TrunkLock || 
                    currentCar.IssueType == IssueType.TrunkBroken)
                {
                    Debug.Log("Issue Found");
                }
                break;
        }
    }
}
