using UnityEngine;
using UnityEngine.UI;

public class ProblemButton : MonoBehaviour
{
    public string problemName;
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                if (DiagnosticController.Instance != null)
                {
                    DiagnosticController.Instance.HandleProblemClick(problemName);
                }
            });
        }
    }
}