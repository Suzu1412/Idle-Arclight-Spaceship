using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(FiniteStateMachine))]
public class FiniteStateMachineEditor : Editor
{
    private Dictionary<StateSO, float> stateUtilities = new Dictionary<StateSO, float>();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw default Unity Inspector

        FiniteStateMachine fsm = (FiniteStateMachine)target;

        if (fsm.CurrentContext == null || fsm.StateList == null)
        {
            EditorGUILayout.HelpBox("FSM is missing a State Context or State List!", MessageType.Warning);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("State Utility Debug", EditorStyles.boldLabel);

        // Evaluate utility values
        stateUtilities.Clear();
        foreach (var state in fsm.StateList.GetStates())
        {
            if (state == null) continue;
            float utility = state.EvaluateUtility(fsm.CurrentContext);
            stateUtilities[state] = utility;
        }

        // Sort states by utility (highest first)
        foreach (var kvp in stateUtilities)
        {
            GUI.color = kvp.Key == fsm.CurrentState ? Color.green : Color.white;
            EditorGUILayout.LabelField($"{kvp.Key.name}: {kvp.Value:F3}");
        }
        GUI.color = Color.white;

        Repaint(); // Force editor refresh to update values in real-time
    }
}
