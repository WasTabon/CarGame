using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public GameObject _noIssuesPanel;
    public GameObject _issuePanel;

    public TextMeshProUGUI descriptionText;
    
    public RectTransform _openButton;
    public RectTransform _closeButton;
    public RectTransform _nextButton;
    public RectTransform _previousButton;
    
    public RectTransform _engineScanButton;
    public RectTransform _leftDoorScanButton;
    public RectTransform _rightDoorScanButton;
    public RectTransform _trunkScanButton;
    
    private RectTransform _lastOpenedPartButton;

    private string _fixScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _closeButton.localScale = Vector3.zero;
        
        _engineScanButton.localScale = Vector3.zero;
        _leftDoorScanButton.localScale = Vector3.zero;
        _rightDoorScanButton.localScale = Vector3.zero;
        _trunkScanButton.localScale = Vector3.zero;
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
    
    public void ShowIssue(IssuesData issuesData)
    {
        var issue = issuesData.issueScenes
            .Find(x => x.issueType == CarsController.Instance.currentCar.IssueType);

        if (issue != null)
        {
            descriptionText.text = issue.description;
            _fixScene = issue.sceneName;
        }
        else
        {
            descriptionText.text = "Описание не найдено";
        }
    }
    
    public void LoadFixScene()
    {
        SceneManager.LoadScene(_fixScene, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_fixScene));
    }
}
