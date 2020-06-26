using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AutomaticVerticalSize))]
public class AutomaticVerticalSizeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();

        if (GUILayout.Button("Recalc Size"))
        {
            AutomaticVerticalSize script = ((AutomaticVerticalSize)target);
            script.AdjustSize();
        }
    }
}
