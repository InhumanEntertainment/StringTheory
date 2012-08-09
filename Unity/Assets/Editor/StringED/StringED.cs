using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class StringED : EditorWindow 
{
    public StringTheoryData Data;
    string FileName = "C:/Users/Erik/Desktop/LT/SVN/Projects/StringTheory/StringTheory.xml";
    Vector2 ScrollPos = Vector2.zero;
    bool[] Foldouts = new bool[1000];
    bool[] PackFoldouts = new bool[1000];	

    //============================================================================================================================================//
    [MenuItem("Inhuman/StringED")]
    static void Init () 
    { 
        EditorWindow.GetWindow(typeof(StringED));
    }

    //============================================================================================================================================//
    void Load()
    {
        Data = StringTheoryData.Load(FileName);
    }

    //============================================================================================================================================//
    void Save()
    {
        if (Data != null)
        {
            Data.Save(FileName);
        }       
    }

    //============================================================================================================================================//
    void OnGUI() 
    {
        GUI.backgroundColor = Color.grey;
        EditorGUILayout.BeginVertical();
        ScrollPos = GUILayout.BeginScrollView(ScrollPos);

        // File //====================================================================================//
        if (GUILayout.Button("Load"))
        {
            Load();
        }

        if (GUILayout.Button("Save"))
        {
            Save();
        }

        if (Data == null)
        {
            Load();
        }

        // Levels //====================================================================================//
        GUILayout.Label("Levels", EditorStyles.boldLabel);
              
        for (int i = 0; i < Data.Levels.Count; i++)
        {
            GUI.backgroundColor = (Data.Levels[i].Index > -1) ? Color.white : Color.red;

            GUILayout.BeginHorizontal();
            Foldouts[i] = EditorGUILayout.Foldout(Foldouts[i], (Data.Levels[i].Index > -1 ? Data.Levels[i].Index.ToString("0000") : "____") + ": " + Data.Levels[i].Name);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Open"))
            {
                EditorApplication.OpenScene(Data.Levels[i].Scene);
            }
                
            if (GUILayout.Button("X"))
            {
                Data.Levels.RemoveAt(i);
            }
            if (GUILayout.Button("+"))
            {
                var level = new StringTheoryLevel();
                Data.Levels.Insert(i, level);
            }
            if (GUILayout.Button("^"))
            {
                if (i > 0)
                {
                    var temp = Data.Levels[i];
                    Data.Levels[i] = Data.Levels[i - 1];
                    Data.Levels[i - 1] = temp;
                    //Foldouts[i] = false;
                    //Foldouts[i - 1] = true;
                }

            }
            if (GUILayout.Button("v"))
            {
                if (i < Data.Levels.Count - 1)
                {
                    var temp = Data.Levels[i];
                    Data.Levels[i] = Data.Levels[i + 1];
                    Data.Levels[i + 1] = temp;
                    //Foldouts[i] = false;
                    //Foldouts[i + 1] = true;
                }
            }

            GUILayout.EndHorizontal();

            if (Foldouts[i])
            {
                    
                Data.Levels[i].Name = EditorGUILayout.TextField("Name:", Data.Levels[i].Name);
                Data.Levels[i].Scene = EditorGUILayout.TextField("Scene:", Data.Levels[i].Scene);
                Data.Levels[i].Index = EditorGUILayout.IntField("Index:", Data.Levels[i].Index);

                Data.Levels[i].Completed = EditorGUILayout.Toggle("Completed:", Data.Levels[i].Completed);
                Data.Levels[i].BestTime = EditorGUILayout.FloatField("Best Time:", Data.Levels[i].BestTime);
                Data.Levels[i].BestLength = EditorGUILayout.FloatField("Best Length:", Data.Levels[i].BestLength);

                
            }

            // Screenshot //
            
            string scene = Data.Levels[i].Scene.Substring(Data.Levels[i].Scene.LastIndexOf('/') + 1);
            if (scene.Length > 6)
            {
                scene = scene.Substring(0, scene.Length - 6);
                string screenshotFilename = "Assets/Screenshots/" + scene + ".png";

                var assetimporter = TextureImporter.GetAtPath(screenshotFilename);
                if (assetimporter != null)
                {
                    TextureImporter texImport = (TextureImporter)assetimporter;
                    //Debug.Log(texImport);
                    texImport.isReadable = true;
                    texImport.npotScale = TextureImporterNPOTScale.None;
                    Texture2D screenshot = (Texture2D)Resources.LoadAssetAtPath(screenshotFilename, typeof(Texture2D));
                    //GUILayout.Box(screenshot, GUILayout.Width(75), GUILayout.Height(100));
                    if (GUILayout.Button(screenshot, GUILayout.Width(150), GUILayout.Height(200)))
                    {
                        EditorApplication.OpenScene(Data.Levels[i].Scene);

                        // Screenshots //
                        string currentScene = EditorApplication.currentScene;
                        string name = currentScene.Substring(currentScene.LastIndexOf('/') + 1);
                        name = name.Substring(0, name.Length - 6);
                        string filename = "Assets/Screenshots/" + name + ".png";

                        Application.CaptureScreenshot(filename);
                        AssetDatabase.ImportAsset(filename);
                        AssetDatabase.Refresh();
                        AssetDatabase.Refresh();
                    }
                }
                else
                {
                    if (GUILayout.Button("Screenshot"))
                    {
                        EditorApplication.OpenScene(Data.Levels[i].Scene);

                        // Screenshots //
                        string currentScene = EditorApplication.currentScene;
                        string name = currentScene.Substring(currentScene.LastIndexOf('/') + 1);
                        name = name.Substring(0, name.Length - 6);
                        string filename = "Assets/Screenshots/" + name + ".png";

                        Application.CaptureScreenshot(filename);
                        AssetDatabase.ImportAsset(filename);
                        AssetDatabase.Refresh();
                        AssetDatabase.Refresh();
                    }
                }
            }
            
        }

        GUI.backgroundColor = Color.white;

        if (GUILayout.Button("New Level"))
        {
            var level = new StringTheoryLevel();
            Foldouts[Data.Levels.Count] = true;
            Data.Levels.Add(level);
        }

        if (GUILayout.Button("Generate Index"))
        {
            GenerateIndexes();                
        }

        if (GUILayout.Button("Add Missing Levels"))
        {
            AddMissingLevels();
        }
        if (GUILayout.Button("Sort Levels"))
        {
            SortLevels();
        }

        // Packs //====================================================================================//
        GUI.backgroundColor = Color.cyan;
        GUILayout.Label("Packs", EditorStyles.boldLabel);

        for (int i = 0; i < Data.Packs.Count; i++)
        {
            GUILayout.BeginHorizontal();
            PackFoldouts[i] = EditorGUILayout.Foldout(PackFoldouts[i], Data.Packs[i].Name);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("X"))
            {
                Data.Packs.RemoveAt(i);
            }
            if (GUILayout.Button("+"))
            {
                var pack = new StringTheoryPack();
                Data.Packs.Insert(i, pack);
            }
            if (GUILayout.Button("^"))
            {
                if (i > 0)
                {
                    var temp = Data.Packs[i];
                    Data.Packs[i] = Data.Packs[i - 1];
                    Data.Packs[i - 1] = temp;
                    //Foldouts[i] = false;
                    //Foldouts[i - 1] = true;
                }

            }
            if (GUILayout.Button("v"))
            {
                if (i < Data.Packs.Count - 1)
                {
                    var temp = Data.Packs[i];
                    Data.Packs[i] = Data.Packs[i + 1];
                    Data.Packs[i + 1] = temp;
                    //Foldouts[i] = false;
                    //Foldouts[i + 1] = true;
                }
            }
            GUILayout.EndHorizontal();
            

            if (PackFoldouts[i])
            {
                Data.Packs[i].Name = EditorGUILayout.TextField("Pack Name:", Data.Packs[i].Name);

                GUILayout.Label("Levels", EditorStyles.boldLabel);
                for (int c = 0; c < Data.Packs[i].Levels.Count; c++)
                {
                    GUILayout.BeginHorizontal();
                    Data.Packs[i].Levels[c] = EditorGUILayout.TextField(Data.Packs[i].Levels[c]);
                    
                    if (GUILayout.Button("X"))
                    {
                        Data.Packs[i].Levels.RemoveAt(c);
                    }
                    if (GUILayout.Button("+"))
                    {
                        Data.Packs[i].Levels.Insert(c, "");
                    }
                    if (GUILayout.Button("^"))
                    {
                        if (c > 0)
                        {
                            string temp = Data.Packs[i].Levels[c];
                            Data.Packs[i].Levels[c] = Data.Packs[i].Levels[c - 1];
                            Data.Packs[i].Levels[c - 1] = temp;
                        }

                    }
                    if (GUILayout.Button("v"))
                    {
                        if (c < Data.Packs[i].Levels.Count - 1)
                        {
                            string temp = Data.Packs[i].Levels[i];
                            Data.Packs[i].Levels[c] = Data.Packs[i].Levels[c + 1];
                            Data.Packs[i].Levels[c + 1] = temp;
                        }
                    }
                    GUILayout.EndHorizontal(); 
                }

                if (GUILayout.Button("Add Level"))
                {
                    Data.Packs[i].Levels.Add("");
                }
            }
        }

        GUI.backgroundColor = Color.white;
        
        if (GUILayout.Button("New Pack"))
        {
            var pack = new StringTheoryPack();
            Data.Packs.Add(pack);
        }

        GUILayout.EndScrollView();             
        EditorGUILayout.EndVertical();
    }

    //============================================================================================================================================//
    void SortLevels()
    {
        List<StringTheoryLevel> sortedLevels = new List<StringTheoryLevel>();

        List<string> levels = GetBuildLevels();

        for (int i = 0; i < levels.Count; i++)
        {
            foreach (var dataLevel in Data.Levels)
            {
                if (dataLevel.Scene == levels[i])
                {
                    sortedLevels.Add(dataLevel);
                    break;
                }
            }
        }

        Data.Levels = sortedLevels;
    }

    //============================================================================================================================================//
    void AddMissingLevels()
    {
        List<string> levels = GetBuildLevels();

        for (int i = 0; i < levels.Count; i++)
        {
            bool foundLevel = false;
            foreach (var dataLevel in Data.Levels)
            {
                if (dataLevel.Scene == levels[i])
                {
                    dataLevel.Index = i;
                    Debug.Log("Set Index on " + levels[i] + " to " + i.ToString());
                    foundLevel = true;
                }
            }

            if (!foundLevel)
            {
                StringTheoryLevel level = new StringTheoryLevel();
                level.Index = i;
                level.Scene = levels[i];

                string name = levels[i].Substring(levels[i].LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                level.Name = name;
                
                Data.Levels.Add(level);
            }
        }
    }
    
    //============================================================================================================================================//
    void GenerateIndexes()
    {
        foreach (var dataLevel in Data.Levels)
        {
            dataLevel.Index = -1;
        }

        List<string> levels = GetBuildLevels();

        for (int i = 0; i < levels.Count; i++)
        {
            foreach (var dataLevel in Data.Levels)
            {
                if (dataLevel.Scene == levels[i])
                {
                    dataLevel.Index = i;
                    Debug.Log("Set Index on " + levels[i] + " to " + i.ToString());
                }
            }
        }
    }

    //============================================================================================================================================//
    void OnInspectorUpdate() 
    { 
        this.Repaint();
    }

    //=====================================================================================================================================//
    public StringTheoryLevel GetLevelData(string scene)
    {
        foreach (StringTheoryLevel level in Data.Levels)
        {
            if (level.Scene == scene)
            {
                return level;
            }
        }

        return null;
    }

    //=====================================================================================================================================//
    List<string> GetBuildLevels()
    {
        List<string> temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                //temp.Add(name);
                temp.Add(S.path);
            }
        }
        return temp;
    }
}