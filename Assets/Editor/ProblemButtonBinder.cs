using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;

public class ProblemButtonBinder : EditorWindow
{
    public List<GameObject> uiObjects = new List<GameObject>();

    [MenuItem("Tools/Bind Problem Buttons")]
    public static void ShowWindow()
    {
        GetWindow<ProblemButtonBinder>("Problem Button Binder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Problem Button Binder", EditorStyles.boldLabel);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty listProp = so.FindProperty("uiObjects");
        EditorGUILayout.PropertyField(listProp, new GUIContent("UI Objects"), true);
        so.ApplyModifiedProperties();

        GUILayout.Space(10);

        if (GUILayout.Button("Bind"))
        {
            Bind();
        }
    }

    private void Bind()
    {
        foreach (var obj in uiObjects)
        {
            if (obj == null) continue;

            var tmp = obj.GetComponentInChildren<TextMeshProUGUI>(true);
            if (tmp == null)
            {
                Debug.LogWarning($"В {obj.name} нет TMPRO");
                continue;
            }

            var pb = obj.GetComponent<ProblemButton>();
            if (pb == null)
            {
                pb = obj.AddComponent<ProblemButton>();
            }

            pb.problemName = tmp.text;

            EditorUtility.SetDirty(pb);
            Debug.Log($"Назначил {tmp.text} для {obj.name}");
        }
    }
}