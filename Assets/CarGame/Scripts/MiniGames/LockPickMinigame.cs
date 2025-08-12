using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LockPickMinigame : MonoBehaviour
{
    [Header("UI")]
    public GameObject finishButton;
    public GameObject clickButton;

    [SerializeField] public RectTransform pin;           // Штифт
    [SerializeField] public float moveSpeed = 200f;      // Скорость движения
    [SerializeField] public float targetZoneHeight = 20f;
    [SerializeField] public RectTransform targetZone;    // Зона успеха
    [SerializeField] public TextMeshProUGUI resultText;

    private bool isDragging = false;
    private Vector2 lastPointerPos;
    private bool isPlaying = false;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!isPlaying) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#else
        HandleMouseInput(); // на всякий случай fallback
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastPointerPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            CheckSuccess();
        }

        if (isDragging)
        {
            Vector2 currentPos = Input.mousePosition;
            float deltaY = currentPos.y - lastPointerPos.y;
            MovePin(deltaY);
            lastPointerPos = currentPos;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastPointerPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaY = touch.position.y - lastPointerPos.y;
                MovePin(deltaY);
                lastPointerPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
                CheckSuccess();
            }
        }
    }

    private void MovePin(float deltaY)
    {
        pin.anchoredPosition += new Vector2(0, deltaY * (moveSpeed * Time.deltaTime));
        pin.anchoredPosition = new Vector2(pin.anchoredPosition.x, Mathf.Clamp(pin.anchoredPosition.y, -100f, 100f));
    }

    public void StartGame()
    {
        isPlaying = true;
        finishButton.SetActive(false);
        clickButton.SetActive(true);

        // Случайная позиция зоны успеха
        float randomY = Random.Range(-80f, 80f);
        targetZone.anchoredPosition = new Vector2(targetZone.anchoredPosition.x, randomY);
        targetZone.sizeDelta = new Vector2(targetZone.sizeDelta.x, targetZoneHeight);
    }

    private void CheckSuccess()
    {
        float pinY = pin.anchoredPosition.y;
        float zoneY = targetZone.anchoredPosition.y;
        float halfZone = targetZoneHeight / 2f;

        if (pinY >= zoneY - halfZone && pinY <= zoneY + halfZone)
        {
            EndGame(true);
        }
        else
        {
            resultText.text = "Missed! Try again.";
            resultText.color = Color.red;
        }
    }

    private void EndGame(bool success)
    {
        if (success)
        {
            isPlaying = false; 
            resultText.text = "Unlocked!";
            resultText.color = Color.green;

            clickButton.SetActive(false);
            finishButton.SetActive(true);
        }
    }

    public void LoadMainScene()
    {
        Scene mainScene = SceneManager.GetSceneByName("Main");
        if (!mainScene.isLoaded)
        {
            Debug.LogError("Main scene not loaded!");
            return;
        }

        Scene currentScene = SceneManager.GetActiveScene();
        ScenesState.lockpickWon = true;
        SceneManager.SetActiveScene(mainScene);
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
