using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpareWheelMinigame : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform wheel;                   // Колесо, которое нужно поставить
    public RectTransform[] bolts;                 // Болты, которые надо поставить
    public RectTransform wheelTarget;             // Целевая позиция для колеса
    public RectTransform[] boltsTargets;          // Целевые позиции для болтов

    public TextMeshProUGUI resultText;
    public GameObject finishButton;

    private bool[] boltsPlaced;                    // Флаги, какие болты уже на месте
    private Vector2[] boltsStartPositions;         // Запоминаем начальные позиции болтов
    private bool wheelPlaced = false;
    private Vector2 wheelStartPosition;

    private RectTransform draggedObject = null;
    private Vector2 dragOffset;

    private bool isPlaying = false;

    void Start()
    {
        boltsPlaced = new bool[bolts.Length];
        boltsStartPositions = new Vector2[bolts.Length];
        for (int i = 0; i < bolts.Length; i++)
        {
            boltsStartPositions[i] = bolts[i].anchoredPosition;
        }
        wheelStartPosition = wheel.anchoredPosition;

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
        HandleMouseInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && draggedObject != null)
        {
            DragTo(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && draggedObject != null)
        {
            TryDrop();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        Vector2 touchPos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            TryStartDrag(touchPos);
        }
        else if (touch.phase == TouchPhase.Moved && draggedObject != null)
        {
            DragTo(touchPos);
        }
        else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && draggedObject != null)
        {
            TryDrop();
        }
    }

    private void TryStartDrag(Vector2 pointerPos)
    {
        // Проверяем, есть ли под курсором draggable объект (болт или колесо)
        for (int i = 0; i < bolts.Length; i++)
        {
            if (!boltsPlaced[i] && RectTransformUtility.RectangleContainsScreenPoint(bolts[i], pointerPos))
            {
                StartDrag(bolts[i], pointerPos);
                return;
            }
        }
        if (!wheelPlaced && RectTransformUtility.RectangleContainsScreenPoint(wheel, pointerPos))
        {
            StartDrag(wheel, pointerPos);
        }
    }

    private void StartDrag(RectTransform obj, Vector2 pointerPos)
    {
        draggedObject = obj;
        RectTransform parentRect = obj.parent as RectTransform;

        Vector2 localPointerPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, pointerPos, null, out localPointerPos);
        dragOffset = (Vector2)obj.anchoredPosition - localPointerPos;
    }

    private void DragTo(Vector2 pointerPos)
    {
        RectTransform parentRect = draggedObject.parent as RectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, pointerPos, null, out localPoint);
        draggedObject.anchoredPosition = localPoint + dragOffset;
    }

    private void TryDrop()
    {
        if (draggedObject == null) return;

        if (draggedObject == wheel)
        {
            float dist = Vector2.Distance(wheel.anchoredPosition, wheelTarget.anchoredPosition);
            if (dist < 50f)
            {
                // Успешно поставили колесо
                wheel.anchoredPosition = wheelTarget.anchoredPosition;
                wheelPlaced = true;
                draggedObject = null;
                CheckWin();
                return;
            }
            else
            {
                // Вернуть на старт
                wheel.anchoredPosition = wheelStartPosition;
                draggedObject = null;
                return;
            }
        }
        else
        {
            // Болт — ищем ближайшую цель в радиусе
            int boltIndex = -1;
            for (int i = 0; i < bolts.Length; i++)
            {
                if (bolts[i] == draggedObject)
                {
                    boltIndex = i;
                    break;
                }
            }

            if (boltIndex == -1)
            {
                draggedObject = null;
                return;
            }

            float snapRadius = 50f; // Радиус магнитности
            int nearestTargetIndex = -1;
            float nearestDist = float.MaxValue;

            for (int i = 0; i < boltsTargets.Length; i++)
            {
                float dist = Vector2.Distance(bolts[boltIndex].anchoredPosition, boltsTargets[i].anchoredPosition);
                if (dist < snapRadius && dist < nearestDist)
                {
                    // Проверяем, что цель не занята другим болтом
                    bool targetOccupied = false;
                    for (int j = 0; j < boltsPlaced.Length; j++)
                    {
                        if (boltsPlaced[j] && j != boltIndex)
                        {
                            // Если другой болт стоит на этой же позиции
                            if (bolts[j].anchoredPosition == boltsTargets[i].anchoredPosition)
                            {
                                targetOccupied = true;
                                break;
                            }
                        }
                    }
                    if (!targetOccupied)
                    {
                        nearestTargetIndex = i;
                        nearestDist = dist;
                    }
                }
            }

            if (nearestTargetIndex != -1)
            {
                // Магнитим болт на найденную цель
                bolts[boltIndex].anchoredPosition = boltsTargets[nearestTargetIndex].anchoredPosition;
                boltsPlaced[boltIndex] = true;
                draggedObject = null;
                CheckWin();
                return;
            }
            else
            {
                // Не попал ни на одну цель — вернуть болт на старт
                bolts[boltIndex].anchoredPosition = boltsStartPositions[boltIndex];
                boltsPlaced[boltIndex] = false;
                draggedObject = null;
                return;
            }
        }
    }

    private void CheckWin()
    {
        if (wheelPlaced)
        {
            foreach (bool placed in boltsPlaced)
            {
                if (!placed)
                    return;
            }

            isPlaying = false;
            ScenesState.trunkWon = true;
            resultText.text = "Spare wheel installed!";
            resultText.color = Color.green;
            finishButton.SetActive(true);
        }
    }

    public void StartGame()
    {
        isPlaying = true;
        finishButton.SetActive(false);
        wheelPlaced = false;
        for (int i = 0; i < boltsPlaced.Length; i++)
            boltsPlaced[i] = false;

        // Возвращаем все объекты на стартовые позиции
        wheel.anchoredPosition = wheelStartPosition;
        for (int i = 0; i < bolts.Length; i++)
        {
            bolts[i].anchoredPosition = boltsStartPositions[i];
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
