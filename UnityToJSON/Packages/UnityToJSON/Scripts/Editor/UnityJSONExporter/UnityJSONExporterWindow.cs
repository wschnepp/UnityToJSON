using UnityEngine;
using UnityEditor;
using System.IO;
using JSONExporter;
using Newtonsoft.Json;

public class UnityJSONExporterWindow : EditorWindow
{
    string exportFilePath = "";
    bool includeDisabledGameObjects;
    bool includeDisabledComponents;
    bool includeUnknownComponentTypes;

    // Add menu named "My Window" to the Window menu
    [MenuItem("UnityToJSON/Export")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        UnityJSONExporterWindow window = (UnityJSONExporterWindow)EditorWindow.GetWindow(typeof(UnityJSONExporterWindow));
         window.Show();
    }

    private void OnEnable()
    {
    }

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
            
  
            if(!exists || overwrite)
            {
                DoExport(exportFilePath, includeDisabledGameObjects, includeDisabledComponents, includeUnknownComponentTypes);
            }          

        }

        
    }

    private static void DoExport(string path, bool disabledGOs, bool disabledComponents, bool includeUnknown)
    {
        var jsonScene = UnityJSONExporter.GenerateJSONScene(disabledGOs, disabledComponents, includeUnknown);
        JsonConverter[] converters = new JsonConverter[] { new BasicTypeConverter() };
        string json = JsonConvert.SerializeObject(jsonScene, Formatting.Indented, converters);

        System.IO.File.WriteAllText(path, json);
        EditorUtility.DisplayDialog("UnityToJSON", "Export Successful", "OK");
    }

    private string ChooseExportPath()
    {

        return EditorUtility.OpenFilePanel(
                          "Export Scene to JSON",
                          "",
                          "json");
    }
}