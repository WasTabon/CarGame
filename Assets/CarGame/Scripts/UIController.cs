using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public IssuesData issuesData;
    
    public GameObject _noIssuesPanel;
    public GameObject _issuePanel;

    public RectTransform finishButton;

    public TextMeshProUGUI descriptionText;
    
    public RectTransform _openButton;
    public RectTransform _closeButton;
    public RectTransform _nextButton;
    public RectTransform _previousButton;
    
    public RectTransform _engineScanButton;
    public RectTransform _leftDoorScanButton;
    public RectTransform _rightDoorScanButton;
    public RectTransform _trunkScanButton;
    
    public Camera mainSceneCamera;
    public Canvas mainSceneCanvas;
    
    private RectTransform _lastOpenedPartButton;

    private string _fixScene;

    private void Awake()
    {
        Instance = this;
        
        mainSceneCamera = Camera.main;
    }

    private void Start()
    {
        _closeButton.localScale = Vector3.zero;
        
        _engineScanButton.localScale = Vector3.zero;
        _leftDoorScanButton.localScale = Vector3.zero;
        _rightDoorScanButton.localScale = Vector3.zero;
        _trunkScanButton.localScale = Vector3.zero;

        finishButton.localScale = Vector3.zero;
        
        HideCameraButtons();
    }

    public void ShowFinishButton()
    {
        finishButton.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.InOutBack);
    }

    public void HideFinishButton()
    {
        finishButton.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InOutBack);
    }
    
    public void SetCloseButton(PartType partType)
    {
        _lastOpenedPartButton = GetPartButton(partType);
        
        _openButton.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete((() =>
            {
                _closeButton.DOScale(Vector3.one, 0.5f)
                    .SetEase(Ease.OutBack);
            }));
        
        if (_lastOpenedPartButton != null)
        {
            _lastOpenedPartButton.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBack);
        }
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
        
        if (_lastOpenedPartButton != null)
        {
            _lastOpenedPartButton.DOScale(Vector3.zero, 0.5f)
                .SetEase(Ease.InBack);
            _lastOpenedPartButton = null;
        }
    }

    public void CloseOpenButton()
    {
        _openButton.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.OutBack);
    }

    private RectTransform GetPartButton(PartType partType)
    {
        return partType switch
        {
            PartType.Engine => _engineScanButton,
            PartType.LeftDoor => _leftDoorScanButton,
            PartType.RightDoor => _rightDoorScanButton,
            PartType.Trunk => _trunkScanButton,
            _ => null
        };
    }
    
    public void ShowIssue()
    {
        var issue = issuesData.issueScenes
            .Find(x => x.issueType == CarsController.Instance.currentCar.IssueType);

        if (issue != null)
        {
            descriptionText.text = issue.description;
            _fixScene = issue.sceneName;
            _issuePanel.gameObject.SetActive(true);
        }
        else
        {
            descriptionText.text = "Описание не найдено";
        }
    }

    public void HideCameraButtons()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_nextButton.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InOutBack));
        sequence.Join(_previousButton.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InOutBack));
        sequence.Join(_openButton.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InOutBack));
        sequence.Join(_closeButton.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InOutBack));
    }
    public void ShowCameraButtons()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_nextButton.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.InOutBack));
        sequence.Join(_previousButton.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.InOutBack));
        sequence.Join(_openButton.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.InOutBack));
        sequence.Join(_closeButton.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InOutBack));
    }

    public void ShowNoIssue()
    {
        _noIssuesPanel.gameObject.SetActive(true);
    }
    
    public void LoadFixScene()
    {
        StartCoroutine(LoadAndActivateScene(_fixScene));
    }

    private IEnumerator LoadAndActivateScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Ждем пока сцена загрузится полностью
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);

        if (loadedScene.IsValid() && loadedScene.isLoaded)
        {
            mainSceneCanvas.gameObject.SetActive(false);
            mainSceneCamera.gameObject.SetActive(false);
            SceneManager.SetActiveScene(loadedScene);
        }
        else
        {
            Debug.LogError($"Scene {sceneName} failed to load or is invalid");
        }
    }

// Метод для обратного включения UI и камеры главной сцены после выгрузки фикс-сцены
    public void RestoreMainSceneUI()
    {
        if (mainSceneCamera != null) mainSceneCamera.enabled = true;
        if (mainSceneCanvas != null) mainSceneCanvas.gameObject.SetActive(true);
    }
    public void UnloadFixScene()
    {
        StartCoroutine(UnloadFixSceneCoroutine());
    }

    private IEnumerator UnloadFixSceneCoroutine()
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(_fixScene);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    
        // Восстанавливаем UI и камеру главной сцены
        RestoreMainSceneUI();
    }
}
