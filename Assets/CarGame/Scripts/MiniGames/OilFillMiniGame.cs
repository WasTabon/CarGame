using UnityEngine;
using UnityEngine.UI;

public class OilFillMiniGame : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fillBar; // Заполняющаяся шкала (Image с типом Filled)
    public Button fillButton; // Кнопка заливки

    [Header("Settings")]
    public float fillSpeed = 1f; // Скорость заполнения шкалы
    public float targetMin = 0.45f; // Нижняя граница правильного диапазона заполнения (0-1)
    public float targetMax = 0.55f; // Верхняя граница правильного диапазона заполнения (0-1)

    private bool isFilling = false;
    private float fillAmount = 0f;

    public delegate void MiniGameResult(bool success);
    public event MiniGameResult OnMiniGameEnded;

    void Start()
    {
        fillBar.fillAmount = 0f;
        fillButton.onClick.AddListener(OnButtonPressed);
        fillButton.onClick.AddListener(() => Debug.Log("Button clicked")); // Для отладки
    }

    public void Click()
    {
        Debug.Log("Pressed");
    }

    void Update()
    {
        if (isFilling)
        {
            fillAmount += fillSpeed * Time.deltaTime;
            fillBar.fillAmount = fillAmount;

            if (fillAmount >= 1f)
            {
                // Дошли до максимума — автоматически завершаем
                EndMiniGame(false);
            }
        }
    }

    public void OnButtonPressed()
    {
        if (!isFilling)
        {
            // Начинаем заливку при первом нажатии
            isFilling = true;
            fillAmount = 0f;
            fillBar.fillAmount = 0f;
        }
        else
        {
            // При повторном нажатии останавливаем заливку
            EndMiniGame(fillAmount >= targetMin && fillAmount <= targetMax);
        }
    }

    private void EndMiniGame(bool success)
    {
        isFilling = false;
        Debug.Log(success ? "Success! Oil filled correctly." : "Failed! Oil level incorrect.");
        OnMiniGameEnded?.Invoke(success);
        // Можно здесь запускать анимации успеха/неудачи или закрывать мини-игру
    }
}