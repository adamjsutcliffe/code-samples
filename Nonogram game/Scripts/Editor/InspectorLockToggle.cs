using System;
using System.Reflection;
using UnityEditor;

public class InspectorLockToggle
{
    [MenuItem("Tools/Toggle Inspector Lock (shortcut) &q")]
    static void SelectLockableInspector()
    {
        EditorWindow inspectorToBeLocked = EditorWindow.mouseOverWindow; // "EditorWindow.focusedWindow" can be used instead
        if (inspectorToBeLocked != null && inspectorToBeLocked.GetType().Name == "InspectorWindow")
        {
            Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
            PropertyInfo propertyInfo = type.GetProperty("isLocked");
            bool value = (bool)propertyInfo.GetValue(inspectorToBeLocked, null);
            propertyInfo.SetValue(inspectorToBeLocked, !value, null);

            inspectorToBeLocked.Repaint();
        }
    }
}