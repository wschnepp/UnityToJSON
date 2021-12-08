using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(Camera))]
public class CustomCameraEditor : Editor
{
    private Camera camera;

    public override void OnInspectorGUI()
    {
        //We need this for all OnInspectorGUI sub methods
        camera = (Camera)target;
        DrawDefaultInspector();


        EditorGUILayout.LabelField("View Matrix");
        OutputMatrix(camera.worldToCameraMatrix);
        EditorGUILayout.LabelField("Projection Matrix");
        OutputMatrix(camera.projectionMatrix);


        EditorGUILayout.LabelField("Projection View Matrix");
        OutputMatrix(camera.projectionMatrix * camera.worldToCameraMatrix);

    }

    private void OutputMatrix(Matrix4x4 mat)
    {
        var previousGUI = GUI.enabled;
        GUI.enabled = false;
        EditorGUILayout.Vector4Field("", mat.GetRow(0));
        EditorGUILayout.Vector4Field("", mat.GetRow(1));
        EditorGUILayout.Vector4Field("", mat.GetRow(2));
        EditorGUILayout.Vector4Field("", mat.GetRow(3));
        GUI.enabled = previousGUI;
    }
}