// Copyright (c) 2014-2015, THUNDERBEAST GAMES LLC
// Licensed under the MIT license, see LICENSE for details

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;

namespace JSONExporter
{

    public class UnityJSONExporter : ScriptableObject
    {
        static void reset()
        {
            JEResource.Reset();
            JEComponent.Reset();
            JEScene.Reset();
            JEGameObject.Reset();

            JEComponent.RegisterStandardComponents();
        }

        public static JSONScene GenerateJSONScene(bool disabledGOs, bool disabledComponents, bool includeUnknown)
        {
            // reset the exporter in case there was an error, Unity doesn't cleanly load/unload editor assemblies
            reset();

            JEScene.sceneName = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);

            JEScene scene = JEScene.TraverseScene(disabledGOs, disabledComponents, includeUnknown);

            scene.Preprocess();
            scene.Process();
            scene.PostProcess();

            JSONScene jsonScene = scene.ToJSON() as JSONScene;

            reset();

            return jsonScene;
        }
    }

}
