using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(VoidGameEventBinding))]
public class VoidGameEventBindingEditor : Editor
{
    private string searchFilter = "";
    private bool showOnlyActive = false;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Display Default Inspector Fields
        DrawDefaultInspector();

        // Get the target binding (BaseGameEventBinding<T>)
        var binding = target as IGameEventBindingEditorHelper;
        if (binding == null) return;

        // UI Elements for Filtering
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Subscribers", EditorStyles.boldLabel);

        searchFilter = EditorGUILayout.TextField("Search", searchFilter);
        showOnlyActive = EditorGUILayout.Toggle("Show Only Active", showOnlyActive);

        EditorGUILayout.Space();

        // Fetch the subscribers
        List<GameObject> subscribers = binding.GetSubscribedGameObjects();

        if (subscribers.Count == 0)
        {
            EditorGUILayout.HelpBox("No subscribers found.", MessageType.Info);
            return;
        }

        foreach (var go in subscribers)
        {
            if (go == null) continue;
            if (showOnlyActive && !go.activeInHierarchy) continue;
            if (!string.IsNullOrEmpty(searchFilter) && !go.name.ToLower().Contains(searchFilter.ToLower())) continue;

            // Display a button for each GameObject
            if (GUILayout.Button(go.name, EditorStyles.miniButton))
            {
                Selection.activeGameObject = go;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}