using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BigNumber))]
public class BigNumberDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty mantissaProp = property.FindPropertyRelative("mantissa");
        SerializedProperty exponentProp = property.FindPropertyRelative("exponent");

        // Label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        float fieldWidth = position.width / 3f;
        Rect mantissaRect = new Rect(position.x, position.y, fieldWidth, position.height);
        Rect exponentRect = new Rect(position.x + fieldWidth + 5, position.y, fieldWidth * 0.8f, position.height);
        Rect formatRect = new Rect(position.x + (fieldWidth * 1.8f) + 10, position.y, fieldWidth * 1.2f, position.height);

        // Draw fields
        mantissaProp.doubleValue = EditorGUI.DoubleField(mantissaRect, mantissaProp.doubleValue);
        exponentProp.intValue = EditorGUI.IntField(exponentRect, exponentProp.intValue);

        // Display formatted value
        BigNumber bigNumber = new BigNumber(mantissaProp.doubleValue, exponentProp.intValue);
        EditorGUI.LabelField(formatRect, bigNumber.GetFormat(), EditorStyles.boldLabel);

        EditorGUI.EndProperty();
    }
}