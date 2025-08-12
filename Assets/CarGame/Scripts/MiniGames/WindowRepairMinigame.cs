using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowRepairMinigame : MonoBehaviour
{
    public GameObject finishButton;
    public GameObject clickButton;
    
    [SerializeField] private Slider progressBar; // Ползунок, показывающий движение стекла
    [SerializeField] private RectTransform targetZone; // Зелёная зона
    [SerializeField] private Button actionButton; // Кнопка для нажатия
    [SerializeField] private float moveSpeed = 1f; // Скорость движения
    [SerializeField] private float successTolerance = 0.05f; // Допуск для попадания

    private bool movingUp = true;
    private bool isPlaying = false;

    void Start()
    {
        progressBar.value = 0;
        actionButton.onClick.AddListener(OnAction);

        StartGame();
    }

    void Update()
    {
        if (!isPlaying) return;

        // Двигаем ползунок вверх-вниз
        if (movingUp)
        {
            progressBar.value += moveSpeed * Time.deltaTime;
            if (progressBar.value >= 1f)
            {
                progressBar.value = 1f;
                movingUp = false;
            }
        }
        else
        {
            progressBar.value -= moveSpeed * Time.deltaTime;
            if (progressBar.value <= 0f)
            {
                progressBar.value = 0f;
                movingUp = true;
            }
        }
    }

    public void StartGame()
    {
        isPlaying = true;
        progressBar.value = 0f;
        movingUp = true;
    }

    private void OnAction()
    {
        if (!isPlaying) return;

        // Центр зоны успеха
        float zoneCenter = targetZone.anchoredPosition.y / progressBar.GetComponent<RectTransform>().sizeDelta.y + 0.5f;
        float diff = Mathf.Abs(progressBar.value - zoneCenter);

        if (diff <= successTolerance)
        {
            Debug.Log("SUCCESS! Window fixed!");
            EndGame(true);
        }
        else
        {
            Debug.Log("Missed! Try again.");
            // Не останавливаем игру, просто продолжаем
        }
    }

    private void EndGame(bool success)
    {
        if (success)
        {
            isPlaying = false;
            ScenesState.windowWon = true;
            
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
        
        SceneManager.SetActiveScene(mainScene);
        
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
