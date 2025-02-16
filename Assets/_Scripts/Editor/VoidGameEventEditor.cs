using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoidGameEvent), true)]
public class VoidGameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw default fields

        var gameEvent = target as VoidGameEvent;
        if (gameEvent == null) return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Raise Event"))
        {
            gameEvent.RaiseEvent(this);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Listeners", EditorStyles.boldLabel);

        foreach (var listener in gameEvent.SenderCounts)
        {
            EditorGUILayout.LabelField($"{listener.Key} - Triggered: {listener.Value} times");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Event History", EditorStyles.boldLabel);

        foreach (var log in gameEvent.EventHistory)
        {
            EditorGUILayout.LabelField(log);
        }

        foreach (var sender in gameEvent.SenderCounts.Keys)
        {
            if (sender == null) continue;

            EditorGUILayout.BeginHorizontal();

            // Show sender name and count
            EditorGUILayout.LabelField($"{sender.name} ({gameEvent.SenderCounts[sender]} times)");

            // Add "Select" button
            //if (GUILayout.Button("Select", GUILayout.Width(60)))
            //{
            //    Selection.activeObject = sender; // Select the object in Unity
            //    EditorGUIUtility.PingObject(sender); // Highlight it
            //}

            EditorGUILayout.EndHorizontal();
        }
    }
}
