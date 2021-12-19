using JSONExporter;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static JSONExporter.UnityJSONExporter;

public class CustomText 
{

    class JSONTextRenderer : JSONComponent
    {
        public string mesh;
        public string text;
        public int fontSize;
        public Color textColor;
    }

    class JETextComponent : JEComponent
    {
        JEMesh mesh;

        MeshFilter unityMeshFilter;
        override public void Preprocess()
        {
            base.Preprocess();
            unityMeshFilter = jeGameObject.unityGameObject.GetComponent<MeshFilter>();

        }

        public override void Process()
        {
            base.Process();
        }

        override public void QueryResources()
        {
            mesh = JEMesh.RegisterMesh(unityMeshFilter.sharedMesh);
        }

        public override JSONComponent ToJSON()
        {
            var json = new JSONTextRenderer();
            var textRenderer = unityComponent as TextRenderer;

            json.mesh = "Plane";
            json.type = "TextRenderer";
            json.text = textRenderer.text;
            json.fontSize = textRenderer.fontSize;
            json.textColor = textRenderer.textColor;
            return json;
        }
    }

    // Add menu named "My Window" to the Window menu
    [MenuItem("UnityToJSON/Custom Export")]
    public static void RegisterAdditional()
    {
        RegisterCallback lambda = () => {
            Debug.Log("Registered");
            JEComponent.RegisterConversion(typeof(TextRenderer), typeof(JETextComponent));
        };
        UnityJSONExporterWindow.WithAdditionalRegistrations("JSON Export (with Registrations)", lambda);
    }

}
