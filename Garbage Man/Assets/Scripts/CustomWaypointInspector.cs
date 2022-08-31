using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointManager))]
public class CustomWaypointInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WaypointManager line = (WaypointManager)target;
        if(GUILayout.Button("Add New Position"))
        {
            line.AddWirePosition();
        }

        if(GUILayout.Button("Remove Position"))
        {
            line.RemoveWirePosition();
        }
    }
}
