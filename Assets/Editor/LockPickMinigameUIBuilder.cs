#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class LockPickMinigameUIBuilder
{
    [MenuItem("GameObject/Create LockPick Minigame UI", false, 10)]
    public static void CreateUI(MenuCommand menuCommand)
    {
        // Создаём корневой Canvas, если его нет
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
        }

        // Родитель для мини-игры — растянутый на весь экран
        GameObject root = new GameObject("LockPickMinigame", typeof(RectTransform), typeof(LockPickMinigame));
        root.transform.SetParent(canvas.transform, false);
        RectTransform rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = Vector2.zero;
        rootRect.anchorMax = Vector2.one;
        rootRect.offsetMin = Vector2.zero;
        rootRect.offsetMax = Vector2.zero;

        // Фон
        GameObject bg = CreateUIElement("Background", root.transform, Vector2.zero);
        RectTransform bgRect = bg.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        // Target Zone
        GameObject targetZone = CreateUIElement("TargetZone", root.transform, new Vector2(40, 20));
        Image zoneImage = targetZone.AddComponent<Image>();
        zoneImage.color = Color.green;

        // Pin
        GameObject pin = CreateUIElement("Pin", root.transform, new Vector2(20, 40));
        Image pinImage = pin.AddComponent<Image>();
        pinImage.color = Color.white;

        // Result Text
        GameObject textObj = CreateUIElement("ResultText", root.transform, new Vector2(500, 50));
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = 32;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.text = "Drag to pick the lock";
        textObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);

        // Click Button
        GameObject clickBtnObj = CreateButton("ClickButton", root.transform, "Start", new Vector2(300, 80));
        clickBtnObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -400);

        // Finish Button
        GameObject finishBtnObj = CreateButton("FinishButton", root.transform, "Finish", new Vector2(300, 80));
        finishBtnObj.SetActive(false);
        finishBtnObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -500);

        // Ставим ссылки в LockPickMinigame
        LockPickMinigame script = root.GetComponent<LockPickMinigame>();
        script.pin = pin.GetComponent<RectTransform>();
        script.targetZone = targetZone.GetComponent<RectTransform>();
        script.resultText = tmp;
        script.clickButton = clickBtnObj;
        script.finishButton = finishBtnObj;

        Selection.activeGameObject = root;
    }

    private static GameObject CreateUIElement(string name, Transform parent, Vector2 size)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.localScale = Vector3.one;
        rect.localPosition = Vector3.zero;
        return go;
    }

    private static GameObject CreateButton(string name, Transform parent, string text, Vector2 size)
    {
        GameObject btnGO = CreateUIElement(name, parent, size);
        Image img = btnGO.AddComponent<Image>();
        img.color = new Color(0.8f, 0.8f, 0.8f);

        Button btn = btnGO.AddComponent<Button>();

        GameObject textGO = CreateUIElement("Text", btnGO.transform, size);
        TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = 28;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.text = text;

        return btnGO;
    }
}
#endif
