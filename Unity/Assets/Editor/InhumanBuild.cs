using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class InhumanBuild : EditorWindow
{
    //============================================================================================================================================//
    [MenuItem("Inhuman/Build All")]
    static void Init()
    {
        string[] levels = { "Assets/Scenes/Main.unity", "Assets/Scenes/Game.Unity" };        
        string path = Application.dataPath.Replace("Assets","") + "Build/Test/";
        BuildOptions options = BuildOptions.None;

        BuildPipeline.BuildPlayer(levels, path, BuildTarget.StandaloneWindows, options);
    }

    //============================================================================================================================================//
    void GenerateLevelList()
    {
    }
}