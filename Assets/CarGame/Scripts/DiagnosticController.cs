using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Problem
{
    public string problemName;
    public string[] symptoms = new string[3];
}

public class DiagnosticController : MonoBehaviour
{
    public static DiagnosticController Instance;
    
    [SerializeField] private TextMeshProUGUI _symptomText1;
    [SerializeField] private TextMeshProUGUI _symptomText2;
    [SerializeField] private TextMeshProUGUI _symptomText3;

    [SerializeField] private Transform symptomParent;
    private TextMeshProUGUI[] symptomTexts;
    
    [SerializeField] private List<Problem> problems = new List<Problem>();

    [ContextMenu("Заполнить список проблем")]
    private void FillProblems()
    {
        problems.Clear();
        
        problems.Add(new Problem { problemName = "Radiator clogged", symptoms = new[] { "Engine temperature rising", "Fluid leakage", "Reduced performance" } });
        problems.Add(new Problem { problemName = "Worn timing belt", symptoms = new[] { "Strange noise", "Reduced performance", "Warning light on dashboard" } });
        problems.Add(new Problem { problemName = "Faulty thermostat", symptoms = new[] { "Engine temperature rising", "Warning light on dashboard", "Strange noise" } });
        problems.Add(new Problem { problemName = "Cracked cylinder head", symptoms = new[] { "Engine temperature rising", "Fluid leakage", "Warning light on dashboard" } });
        problems.Add(new Problem { problemName = "Dirty air filter", symptoms = new[] { "Reduced performance", "Strange noise", "Visible physical damage" } });
        problems.Add(new Problem { problemName = "Fuel injector failure", symptoms = new[] { "Reduced performance", "Warning light on dashboard", "Electrical glitch" } });
        problems.Add(new Problem { problemName = "Spark plug damage", symptoms = new[] { "Strange noise", "Reduced performance", "Electrical glitch" } });
        problems.Add(new Problem { problemName = "Turbocharger failure", symptoms = new[] { "Reduced performance", "Strange noise", "Engine temperature rising" } });

        problems.Add(new Problem { problemName = "Power window motor burned out", symptoms = new[] { "Moves slowly", "Electrical glitch", "Mechanism stuck/jammed" } });
        problems.Add(new Problem { problemName = "Window track jammed", symptoms = new[] { "Mechanism stuck/jammed", "Difficulty opening/closing", "Visible physical damage" } });
        problems.Add(new Problem { problemName = "Broken door lock actuator", symptoms = new[] { "Difficulty opening/closing", "Electrical glitch", "Mechanism stuck/jammed" } });
        problems.Add(new Problem { problemName = "Rusted hinge", symptoms = new[] { "Difficulty opening/closing", "Strange noise", "Visible physical damage" } });
        problems.Add(new Problem { problemName = "Wiring short in door controls", symptoms = new[] { "Electrical glitch", "Warning light on dashboard", "Mechanism stuck/jammed" } });
        problems.Add(new Problem { problemName = "Handle mechanism broken", symptoms = new[] { "Difficulty opening/closing", "Mechanism stuck/jammed", "Visible physical damage" } });
        
        problems.Add(new Problem { problemName = "Trunk latch stuck", symptoms = new[] { "Mechanism stuck/jammed", "Difficulty opening/closing", "Electrical glitch" } });
        problems.Add(new Problem { problemName = "Broken hydraulic lift support", symptoms = new[] { "Moves slowly", "Visible physical damage", "Difficulty opening/closing" } });
        problems.Add(new Problem { problemName = "Damaged trunk lock cylinder", symptoms = new[] { "Difficulty opening/closing", "Electrical glitch", "Mechanism stuck/jammed" } });
        problems.Add(new Problem { problemName = "Rusted trunk hinge", symptoms = new[] { "Difficulty opening/closing", "Strange noise", "Visible physical damage" } });
        problems.Add(new Problem { problemName = "Faulty trunk release button", symptoms = new[] { "Electrical glitch", "Warning light on dashboard", "Mechanism stuck/jammed" } });
        problems.Add(new Problem { problemName = "Stuck spare tire compartment", symptoms = new[] { "Mechanism stuck/jammed", "Difficulty opening/closing", "Visible physical damage" } });
    }

    private void Awake()
    {
        Instance = this;
        
        symptomTexts = symptomParent.GetComponentsInChildren<TextMeshProUGUI>(true)
            .Where(t => t.CompareTag("Symptom"))
            .ToArray();
    }

    public void ShowProblem()
{
    if (CarsController.Instance?.currentCar == null)
    {
        Debug.LogWarning("Текущая машина не найдена");
        return;
    }

    IssueType issueType = CarsController.Instance.currentCar.IssueType;

    // Фильтрация проблем по типу
    List<Problem> filteredProblems = new List<Problem>();

    switch (issueType)
    {
        case IssueType.EngineOverheat:
        case IssueType.EngineBelt:
            filteredProblems.AddRange(problems.FindAll(p =>
                p.problemName == "Radiator clogged" ||
                p.problemName == "Worn timing belt" ||
                p.problemName == "Faulty thermostat" ||
                p.problemName == "Cracked cylinder head" ||
                p.problemName == "Dirty air filter" ||
                p.problemName == "Fuel injector failure" ||
                p.problemName == "Spark plug damage" ||
                p.problemName == "Turbocharger failure"));
            break;

        case IssueType.DoorWindow:
        case IssueType.DoorLock:
            filteredProblems.AddRange(problems.FindAll(p =>
                p.problemName == "Power window motor burned out" ||
                p.problemName == "Window track jammed" ||
                p.problemName == "Broken door lock actuator" ||
                p.problemName == "Rusted hinge" ||
                p.problemName == "Wiring short in door controls" ||
                p.problemName == "Handle mechanism broken"));
            break;

        case IssueType.TrunkLock:
        case IssueType.TrunkBroken:
            filteredProblems.AddRange(problems.FindAll(p =>
                p.problemName == "Trunk latch stuck" ||
                p.problemName == "Broken hydraulic lift support" ||
                p.problemName == "Damaged trunk lock cylinder" ||
                p.problemName == "Rusted trunk hinge" ||
                p.problemName == "Faulty trunk release button" ||
                p.problemName == "Stuck spare tire compartment"));
            break;
    }

    if (filteredProblems.Count == 0)
    {
        Debug.LogWarning($"Нет проблем для типа {issueType}");
        return;
    }

    // Случайная проблема
    Problem selectedProblem = filteredProblems[UnityEngine.Random.Range(0, filteredProblems.Count)];

    // Устанавливаем симптомы
    _symptomText1.text = selectedProblem.symptoms[0];
    _symptomText2.text = selectedProblem.symptoms[1];
    _symptomText3.text = selectedProblem.symptoms[2];

    // Список симптомов для сравнения
    string[] selectedSymptoms = selectedProblem.symptoms;
    
    Debug.Log($"Найдено текстов (включая отключенные): {symptomTexts.Length}");

    foreach (var tmp in symptomTexts)
    {
        if (tmp.CompareTag("Symptom"))
        {
            Debug.Log("Текст найден и имеет тег Symptom");
            tmp.color = Array.Exists(selectedSymptoms, s => s == tmp.text)
                ? Color.green
                : Color.red;
        }
    }

    Debug.Log($"Выбрана проблема: {selectedProblem.problemName}");
}

}