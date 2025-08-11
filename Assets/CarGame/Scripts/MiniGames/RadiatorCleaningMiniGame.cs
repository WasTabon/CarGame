using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadiatorCleaningMiniGame : MonoBehaviour
{
    [Header("Settings")]
    public int totalSpots = 10;             // Кол-во пятен грязи

    [Header("References")]
    public GameObject dirtSpotPrefab;       // Префаб пятна грязи (круглый спрайт с коллайдером)
    public RectTransform playArea;          // Область, где появятся пятна (UI или мир)
    public TextMeshProUGUI statusText;

    private List<GameObject> dirtSpots = new List<GameObject>();
    private bool isPlaying = false;

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        ClearOldSpots();

        isPlaying = true;
        statusText.text = "";

        // Создаем пятна в случайных местах
        for (int i = 0; i < totalSpots; i++)
        {
            SpawnDirtSpot();
        }
    }

    void ClearOldSpots()
    {
        foreach (var spot in dirtSpots)
            Destroy(spot);
        dirtSpots.Clear();
    }

    void SpawnDirtSpot()
    {
        // Получаем safeArea в пикселях экрана
        Rect safeArea = Screen.safeArea;

        Vector2 localSafePosMin;
        Vector2 localSafePosMax;

        Vector2 screenPosMin = new Vector2(safeArea.xMin, safeArea.yMin);
        Vector2 screenPosMax = new Vector2(safeArea.xMax, safeArea.yMax);

        Camera uiCamera = null;
        if (playArea.GetComponentInParent<Canvas>().renderMode == RenderMode.ScreenSpaceCamera)
            uiCamera = playArea.GetComponentInParent<Canvas>().worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(playArea, screenPosMin, uiCamera, out localSafePosMin);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(playArea, screenPosMax, uiCamera, out localSafePosMax);

        float x = Random.Range(localSafePosMin.x, localSafePosMax.x);
        float y = Random.Range(localSafePosMin.y, localSafePosMax.y);
        Vector2 randomPos = new Vector2(x, y);

        GameObject spot = Instantiate(dirtSpotPrefab, playArea);
        spot.GetComponent<RectTransform>().anchoredPosition = randomPos;
        dirtSpots.Add(spot);
    }

    void Update()
    {
        if (!isPlaying)
            return;

        // Проверяем, если все пятна убраны — показываем сообщение и заканчиваем игру
        if (dirtSpots.Count == 0)
        {
            GameOver(true);
        }

        // Проверка свайпа
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(playArea, touch.position, null, out localPoint);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
            {
                CheckSwipeHit(localPoint);
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(playArea, Input.mousePosition, null, out localPoint);
            CheckSwipeHit(localPoint);
        }
#endif
    }

    void CheckSwipeHit(Vector2 localPos)
    {
        for (int i = dirtSpots.Count - 1; i >= 0; i--)
        {
            var spot = dirtSpots[i];
            RectTransform rt = spot.GetComponent<RectTransform>();

            float radius = 40f;

            if (Vector2.Distance(rt.anchoredPosition, localPos) < radius)
            {
                Destroy(spot);
                dirtSpots.RemoveAt(i);
                break;
            }
        }
    }

    void GameOver(bool success)
    {
        isPlaying = false;
        statusText.text = success ? "Cleaned!" : "";
    }
}
