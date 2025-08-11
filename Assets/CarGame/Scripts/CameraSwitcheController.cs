using Cinemachine;
using UnityEngine;

public class CameraSwitcheController : MonoBehaviour
{
    public static CameraSwitcheController Instance;
    
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras; // 0- Back | 1 - Right | 2- Front | 3- Left
    [SerializeField] private CinemachineVirtualCamera defaultCamera;    // Добавляем дефолтную камеру
    [SerializeField] private CinemachineBrain brain;

    private int _currentIndex = 0;
    public int CurrentIndex => _currentIndex;
    private bool _isBlending => brain.IsBlending;

    private bool _isDefaultActive = false; // Флаг, на дефолтной ли камере сейчас
    public bool IsDefaultActive => _isDefaultActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _isDefaultActive = true; // по умолчанию дефолтная камера активна
        defaultCamera.gameObject.SetActive(true);
        SetVirtualCameraActive(false);
    }

    private void SetVirtualCameraActive(bool active)
    {
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            virtualCameras[i].gameObject.SetActive(active && i == _currentIndex);
        }
    }

    public void NextCamera()
    {
        if (_isBlending || _isDefaultActive) return; // Если дефолтная камера включена — листать нельзя

        CarsController.Instance.CloseElement();
        
        virtualCameras[_currentIndex].gameObject.SetActive(false);
        _currentIndex = (_currentIndex + 1) % virtualCameras.Length;
        virtualCameras[_currentIndex].gameObject.SetActive(true);
    }

    public void PreviousCamera()
    {
        if (_isBlending || _isDefaultActive) return; // Если дефолтная камера включена — листать нельзя

        CarsController.Instance.CloseElement();
        
        virtualCameras[_currentIndex].gameObject.SetActive(false);
        _currentIndex = (_currentIndex - 1 + virtualCameras.Length) % virtualCameras.Length;
        virtualCameras[_currentIndex].gameObject.SetActive(true);
    }

    public void ToggleDefaultCamera()
    {
        if (_isBlending) return;

        CarsController.Instance.CloseElement();

        if (_isDefaultActive)
        {
            // С дефолтной на виртуальные камеры
            defaultCamera.gameObject.SetActive(false);
            _isDefaultActive = false;
            SetVirtualCameraActive(true);
        }
        else
        {
            // С виртуальных на дефолтную
            SetVirtualCameraActive(false);
            defaultCamera.gameObject.SetActive(true);
            _isDefaultActive = true;
        }
    }
}
