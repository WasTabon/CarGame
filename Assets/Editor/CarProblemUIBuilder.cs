#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;

public class CarProblemUIBuilder : EditorWindow
{
    private Transform parentContainer;
    private GameObject problemCardPrefab;

    [System.Serializable]
    public class CarProblem
    {
        public string problemName;
        public string[] symptoms; // ровно 3 симптома
    }

    // Твои данные — можно потом вынести в отдельный ScriptableObject
    private CarProblem[] problems = new CarProblem[]
    {
        new CarProblem { problemName = "Radiator clogged", symptoms = new[] { "Engine temperature rising", "Fluid leakage", "Reduced performance" } },
        new CarProblem { problemName = "Worn timing belt", symptoms = new[] { "Strange noise", "Reduced performance", "Warning light on dashboard" } },
        new CarProblem { problemName = "Faulty thermostat", symptoms = new[] { "Engine temperature rising", "Warning light on dashboard", "Strange noise" } },
        new CarProblem { problemName = "Cracked cylinder head", symptoms = new[] { "Engine temperature rising", "Fluid leakage", "Warning light on dashboard" } },
        new CarProblem { problemName = "Dirty air filter", symptoms = new[] { "Reduced performance", "Strange noise", "Visible physical damage" } },
        new CarProblem { problemName = "Fuel injector failure", symptoms = new[] { "Reduced performance", "Warning light on dashboard", "Electrical glitch" } },
        new CarProblem { problemName = "Spark plug damage", symptoms = new[] { "Strange noise", "Reduced performance", "Electrical glitch" } },
        new CarProblem { problemName = "Turbocharger failure", symptoms = new[] { "Reduced performance", "Strange noise", "Engine temperature rising" } },
        new CarProblem { problemName = "Power window motor burned out", symptoms = new[] { "Moves slowly", "Electrical glitch", "Mechanism stuck/jammed" } },
        new CarProblem { problemName = "Window track jammed", symptoms = new[] { "Mechanism stuck/jammed", "Difficulty opening/closing", "Visible physical damage" } },
        new CarProblem { problemName = "Broken door lock actuator", symptoms = new[] { "Difficulty opening/closing", "Electrical glitch", "Mechanism stuck/jammed" } },
        new CarProblem { problemName = "Rusted hinge", symptoms = new[] { "Difficulty opening/closing", "Strange noise", "Visible physical damage" } },
        new CarProblem { problemName = "Wiring short in door controls", symptoms = new[] { "Electrical glitch", "Warning light on dashboard", "Mechanism stuck/jammed" } },
        new CarProblem { problemName = "Handle mechanism broken", symptoms = new[] { "Difficulty opening/closing", "Mechanism stuck/jammed", "Visible physical damage" } },
        new CarProblem { problemName = "Trunk latch stuck", symptoms = new[] { "Mechanism stuck/jammed", "Difficulty opening/closing", "Electrical glitch" } },
        new CarProblem { problemName = "Broken hydraulic lift support", symptoms = new[] { "Moves slowly", "Visible physical damage", "Difficulty opening/closing" } },
        new CarProblem { problemName = "Damaged trunk lock cylinder", symptoms = new[] { "Difficulty opening/closing", "Electrical glitch", "Mechanism stuck/jammed" } },
        new CarProblem { problemName = "Rusted trunk hinge", symptoms = new[] { "Difficulty opening/closing", "Strange noise", "Visible physical damage" } },
        new CarProblem { problemName = "Faulty trunk release button", symptoms = new[] { "Electrical glitch", "Warning light on dashboard", "Mechanism stuck/jammed" } },
        new CarProblem { problemName = "Stuck spare tire compartment", symptoms = new[] { "Mechanism stuck/jammed", "Difficulty opening/closing", "Visible physical damage" } }
    };

    [MenuItem("Tools/Build Car Problems UI")]
    public static void ShowWindow()
    {
        GetWindow<CarProblemUIBuilder>("Car Problems UI Builder");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Настройки генерации", EditorStyles.boldLabel);

        parentContainer = (Transform)EditorGUILayout.ObjectField("Родительский объект", parentContainer, typeof(Transform), true);
        problemCardPrefab = (GameObject)EditorGUILayout.ObjectField("Префаб карточки", problemCardPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Создать UI"))
        {
            if (parentContainer == null || problemCardPrefab == null)
            {
                EditorUtility.DisplayDialog("Ошибка", "Укажи родителя и префаб", "OK");
                return;
            }

            CreateUI();
        }
    }

    private void CreateUI()
    {
        foreach (Transform child in parentContainer)
        {
            DestroyImmediate(child.gameObject);
        }

        foreach (var problem in problems)
        {
            GameObject card = (GameObject)PrefabUtility.InstantiatePrefab(problemCardPrefab, parentContainer);

            var texts = card.GetComponentsInChildren<TextMeshProUGUI>(true);

            foreach (var t in texts)
            {
                if (t.name == "ProblemText")
                    t.text = problem.problemName;
                else if (t.name.Contains("SymptomName") && problem.symptoms.Length > 0)
                {
                    string indexStr = t.name.Replace("SymptomName (", "").Replace(")", "");
                    if (int.TryParse(indexStr, out int index) && index > 0 && index <= problem.symptoms.Length)
                        t.text = problem.symptoms[index - 1];
                }
            }
        }

        EditorUtility.DisplayDialog("Готово", "Карточки поломок созданы!", "OK");
    }
}
#endif
