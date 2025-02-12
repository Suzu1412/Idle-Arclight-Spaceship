using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
public class FSMUtilityGraphWindow : EditorWindow
{
    private FiniteStateMachine _fsm;
    private Dictionary<StateSO, List<float>> utilityHistory = new Dictionary<StateSO, List<float>>();
    private const int maxHistory = 100; // Number of frames to track

    [MenuItem("Tools/FSM Utility Graph")]
    public static void ShowWindow()
    {
        GetWindow<FSMUtilityGraphWindow>("FSM Utility Graph");
    }

    private void OnGUI()
    {
        GUILayout.Label("FSM Utility Graph", EditorStyles.boldLabel);
        _fsm = EditorGUILayout.ObjectField("Finite State Machine", _fsm, typeof(FiniteStateMachine), true) as FiniteStateMachine;

        if (_fsm == null || _fsm.CurrentContext == null || _fsm.StateList == null)
        {
            EditorGUILayout.HelpBox("Assign a valid FSM with a State Context and State List.", MessageType.Warning);
            return;
        }

        UpdateUtilityHistory();
        DrawGraph();
        Repaint(); // Refresh the window continuously
    }

    private void UpdateUtilityHistory()
    {
        foreach (var state in _fsm.StateList.GetStates())
        {
            if (state == null) continue;
            if (!utilityHistory.ContainsKey(state))
                utilityHistory[state] = new List<float>();

            float utility = state.EvaluateUtility(_fsm.CurrentContext);
            List<float> history = utilityHistory[state];

            history.Add(utility);
            if (history.Count > maxHistory)
                history.RemoveAt(0);
        }
    }

    private void DrawGraph()
    {
        Rect graphRect = GUILayoutUtility.GetRect(600, 300);
        EditorGUI.DrawRect(graphRect, Color.black);

        if (utilityHistory.Count == 0) return;

        float maxUtility = utilityHistory.Values.SelectMany(list => list).Max();
        float minUtility = utilityHistory.Values.SelectMany(list => list).Min();

        foreach (var entry in utilityHistory)
        {
            Handles.color = Random.ColorHSV();
            List<float> history = entry.Value;
            for (int i = 1; i < history.Count; i++)
            {
                float x1 = graphRect.x + (i - 1) * (graphRect.width / maxHistory);
                float y1 = graphRect.y + graphRect.height - Mathf.Lerp(0, graphRect.height, (history[i - 1] - minUtility) / (maxUtility - minUtility));
                float x2 = graphRect.x + i * (graphRect.width / maxHistory);
                float y2 = graphRect.y + graphRect.height - Mathf.Lerp(0, graphRect.height, (history[i] - minUtility) / (maxUtility - minUtility));
                Handles.DrawLine(new Vector3(x1, y1), new Vector3(x2, y2));
            }
        }
    }
}
