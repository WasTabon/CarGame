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
}
