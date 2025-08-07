using UnityEngine;
using UnityEditor;

public class ColliderRemover : EditorWindow
{
    [MenuItem("Tools/Remove Colliders From Selected Object And Children")]
    private static void RemoveCollidersFromSelectedAndChildren()
    {
        GameObject selected = Selection.activeGameObject;

        if (selected == null)
        {
            Debug.LogError("No GameObject selected.");
            return;
        }

        // Получаем все коллайдеры на объекте и его детях
        Collider[] colliders = selected.GetComponentsInChildren<Collider>(true);

        if (colliders.Length == 0)
        {
            Debug.Log("No colliders found on selected GameObject or its children.");
            return;
        }

        Undo.RegisterCompleteObjectUndo(selected, "Remove Colliders");

        int removedCount = 0;
        foreach (var collider in colliders)
        {
            Undo.DestroyObjectImmediate(collider);
            removedCount++;
        }

        Debug.Log($"Removed {removedCount} collider(s) from '{selected.name}' and its children.");
    }
}