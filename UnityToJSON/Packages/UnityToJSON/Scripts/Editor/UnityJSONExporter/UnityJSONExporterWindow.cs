using UnityEngine;
using UnityEditor;
using System.IO;
using JSONExporter;
using Newtonsoft.Json;
using System.Collections.Generic;
using static JSONExporter.UnityJSONExporter;

public class UnityJSONExporterWindow : EditorWindow
{
    string exportFilePath = "";
    bool includeDisabledGameObjects;
    bool includeDisabledComponents;
    bool includeUnknownComponentTypes;

    RegisterCallback registerCallback;

    // Add menu named "My Window" to the Window menu
    [MenuItem("UnityToJSON/Export")]
    public static void Init()
    {
        WithAdditionalRegistrations("JSON Export", null);
    }

    public static void WithAdditionalRegistrations( string title,
        RegisterCallback registerCallback)
    {
        // Get existing open window or if none, make a new one:
        UnityJSONExporterWindow window = (UnityJSONExporterWindow)EditorWindow.GetWindow<UnityJSONExporterWindow>(title);
        window.registerCallback = registerCallback;
        window.Show();
    }

    private void OnEnable()
    {
    }
    int successfulMessageTTL = 0;

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        
        exportFilePath = EditorGUILayout.TextField("File Path", exportFilePath);
        if(GUILayout.Button("Select"))
        {
            exportFilePath = ChooseExportPath();
        }
        EditorGUILayout.EndHorizontal();

        includeDisabledGameObjects = EditorGUILayout.Toggle("Export disabled gameobjects", includeDisabledGameObjects);
        includeDisabledComponents = EditorGUILayout.Toggle("Export disabled components", includeDisabledComponents);
        includeUnknownComponentTypes = EditorGUILayout.Toggle("Export components with unknown type too", includeUnknownComponentTypes);

        if (GUILayout.Button("Export") && exportFilePath != null && exportFilePath.Length > 0)
        {
            bool exists = File.Exists(exportFilePath);
            bool overwrite = false;
            
            if(exists)
            {
                overwrite = EditorUtility.DisplayDialog("File already exists", "Do you wish to overwrite it?", "Yes", "No");
            }


            if (!exists || overwrite)
            {
                if(DoExport(exportFilePath, includeDisabledGameObjects, includeDisabledComponents, includeUnknownComponentTypes, registerCallback))
                {
                    successfulMessageTTL = 10;
                }               
            
            }          

        }

        if (successfulMessageTTL > 0)
        {
            EditorGUILayout.HelpBox("Text Export successful", MessageType.Info);

            successfulMessageTTL--;
        }


    }

    private static bool DoExport(string path, bool disabledGOs, bool disabledComponents, bool includeUnknown, RegisterCallback registerCallback)
    {
        var jsonScene = UnityJSONExporter.GenerateJSONScene(disabledGOs, disabledComponents, includeUnknown, registerCallback);
        JsonConverter[] converters = new JsonConverter[] { new BasicTypeConverter() };
        string json = JsonConvert.SerializeObject(jsonScene, Formatting.Indented, converters);

        System.IO.File.WriteAllText(path, json);
        return true;

    }

    private string ChooseExportPath()
    {

        return EditorUtility.OpenFilePanel(
                          "Export Scene to JSON",
                          "",
                          "json");
    }
}