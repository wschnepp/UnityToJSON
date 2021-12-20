using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//thanks https://www.youtube.com/watch?v=pdDrY8Mc2lU
[InitializeOnLoad]
public class CustomHierarchyTagDrawer : MonoBehaviour
{
    // Start is called before the first frame update

    static GUIStyle guiStyle = null;

    static readonly Vector2 offset = new Vector2(18, 0);
    static CustomHierarchyTagDrawer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemDraw;
        guiStyle = new GUIStyle()
        {
            normal = new GUIStyleState() { textColor= new Color(1.0f, 0.33f, 0.0f, 1.0f) }
        };
    }


    private static void OnHierarchyItemDraw(int instanceId, Rect selection)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceId);
        if(obj && obj is GameObject go)
        {
            if(go.tag == "IgnoreForExport")
            {
                Rect offsetRect = new Rect(selection.position + offset, selection.size);
                EditorGUI.LabelField(offsetRect, go.name, guiStyle);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
