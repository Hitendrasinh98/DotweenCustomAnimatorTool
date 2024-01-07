using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomAnimatorTool))]
public class EditorCustomAnimatorTool : Editor
{
    /// <summary>
    /// Just to draw 3 buttons end of this inspector 
    /// so we can easily call sequence funtions
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Set"))
        {
            CustomAnimatorTool customAnimatorTool = target as CustomAnimatorTool;
            customAnimatorTool.Set_Sequence();
        }

        if (GUILayout.Button("Play"))
        {
            CustomAnimatorTool customAnimatorTool = target as CustomAnimatorTool;
            customAnimatorTool.Start_Animation();
        }

        if (GUILayout.Button("Rewind"))
        {
            CustomAnimatorTool customAnimatorTool = target as CustomAnimatorTool;
            customAnimatorTool.Set_SequenceRewindState();
        }

        GUILayout.EndHorizontal();

    }
}
