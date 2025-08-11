using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum IssueType
{
    EngineOverheat,
    EngineBelt,
    DoorWindow,
    DoorLock,
    TrunkLock,
    TrunkBroken
}

[Serializable]
public class IssueScenePair
{
    public IssueType issueType;
    [TextArea]
    public string description;

#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif

    [HideInInspector]
    public string sceneName;
}

[CreateAssetMenu(fileName = "NewCarIssue", menuName = "Game Data/Issues", order = 1)]
public class IssuesData : ScriptableObject
{
    public List<IssueScenePair> issueScenes = new List<IssueScenePair>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var pair in issueScenes)
        {
            if (pair.sceneAsset != null)
            {
                string path = AssetDatabase.GetAssetPath(pair.sceneAsset);
                pair.sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            }
        }
    }
#endif
}
