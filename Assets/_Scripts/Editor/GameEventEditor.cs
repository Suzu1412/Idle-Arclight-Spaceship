using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseGameEvent<>), true)]
public class GameEventEditor : Editor
{
    private object eventValue; // Stores input value for the event

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw default fields

        var gameEvent = target as IGameEvent;
        if (gameEvent == null) return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Controls", EditorStyles.boldLabel);

        if (gameEvent.HasParameterType)
        {
            eventValue = DrawFieldForType(gameEvent.ParameterType, eventValue);
        }

        if (GUILayout.Button("Raise Event"))
        {
            gameEvent.RaiseFromEditor(eventValue, this);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Listeners", EditorStyles.boldLabel);

        foreach (var sender in gameEvent.SenderCounts.Keys)
        {
            EditorGUILayout.BeginHorizontal();

            // Show sender name and count
            EditorGUILayout.LabelField($"{sender.name} ({gameEvent.SenderCounts[sender]} times)");

            // Display a button for each GameObject
            if (GUILayout.Button(sender.name, EditorStyles.miniButton))
            {
                EditorGUIUtility.PingObject(sender); // Highlight it
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Event History", EditorStyles.boldLabel);

        foreach (var log in gameEvent.EventHistory)
        {
            EditorGUILayout.LabelField(log);
        }
    }

    private object DrawFieldForType(Type type, object value)
    {
        if (type == typeof(int))
        {
            return EditorGUILayout.IntField("Value", value != null ? (int)value : 0);
        }
        if (type == typeof(float))
        {
            return EditorGUILayout.FloatField("Value", value != null ? (float)value : 0f);
        }
        if (type == typeof(double))
        {
            return EditorGUILayout.DoubleField("Value", value != null ? (double)value : 0);
        }
        if (type == typeof(string))
        {
            return EditorGUILayout.TextField("Value", value as string);
        }
        if (type == typeof(bool))
        {
            return EditorGUILayout.Toggle("Value", value != null ? (bool)value : false);
        }
        if (type == typeof(Vector2))
        {
            return EditorGUILayout.Vector2Field("Value", value != null ? (Vector2)value : Vector2.zero);
        }
        if (type == typeof(Vector3))
        {
            return EditorGUILayout.Vector3Field("Value", value != null ? (Vector3)value : Vector3.zero);
        }
        return value; // Unsupported type
    }
}
