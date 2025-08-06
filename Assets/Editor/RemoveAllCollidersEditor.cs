using UnityEngine;
using UnityEditor;

public class RemoveAllCollidersEditor : MonoBehaviour
{
    [MenuItem("Tools/Удалить все Collider'ы на сцене")]
    private static void RemoveAllColliders()
    {
        Collider[] allColliders = GameObject.FindObjectsOfType<Collider>(true);
        
        int count = allColliders.Length;

        foreach (var collider in allColliders)
        {
            Undo.DestroyObjectImmediate(collider);
        }

        Debug.Log($"Удалено {count} Collider компонент(ов) со сцены.");
    }
}
