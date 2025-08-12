using System.Collections.Generic;
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
    [SerializeField] private TextMeshProUGUI _symptomText1;
    [SerializeField] private TextMeshProUGUI _symptomText2;
    [SerializeField] private TextMeshProUGUI _symptomText3;

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
    
    public void ShowProblem(string problemName)
    {
        var problem = problems.Find(p => p.problemName == problemName);
        if (problem == null)
        {
            Debug.LogWarning($"Проблема '{problemName}' не найдена");
            return;
        }

        _symptomText1.text = problem.symptoms[0];
        _symptomText2.text = problem.symptoms[1];
        _symptomText3.text = problem.symptoms[2];
    }
}
