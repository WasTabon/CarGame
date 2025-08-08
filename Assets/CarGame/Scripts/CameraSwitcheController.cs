using Cinemachine;
using UnityEngine;

public class CameraSwitcheController : MonoBehaviour
{
    public static CameraSwitcheController Instance;
    
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras; // 0- Back | 1 - Right | 2- Front | 3- Left
    [SerializeField] private CinemachineBrain brain;

    private int _currentIndex = 0;
    public int CurrentIndex => _currentIndex;
    private bool _isBlending => brain.IsBlending;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < virtualCameras.Length; i++)
            virtualCameras[i].gameObject.SetActive(i == _currentIndex);
    }

    public void NextCamera()
    {
        if (_isBlending) return;

        virtualCameras[_currentIndex].gameObject.SetActive(false);
        _currentIndex = (_currentIndex + 1) % virtualCameras.Length;
        virtualCameras[_currentIndex].gameObject.SetActive(true);
    }

    public void PreviousCamera()
    {
        if (_isBlending) return;

        virtualCameras[_currentIndex].gameObject.SetActive(false);
        _currentIndex = (_currentIndex - 1 + virtualCameras.Length) % virtualCameras.Length;
        virtualCameras[_currentIndex].gameObject.SetActive(true);
    }
}
