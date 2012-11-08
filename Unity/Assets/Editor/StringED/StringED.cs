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
    string DataPath = Application.persistentDataPath + "/StringTheory.xml";          

    Vector2 ScrollPos = Vector2.zero;
    bool[] Foldouts = new bool[1000];
    bool[] PackFoldouts = new bool[1000];
    string SearchText = "";

    //============================================================================================================================================//
    [MenuItem("Inhuman/StringED")]
    static void Init () 
    { 
        EditorWindow.GetWindow(typeof(StringED));
    }

    //============================================================================================================================================//
    void Load()
    {
        Data = StringTheoryData.Load(DataPath);
    }

    //============================================================================================================================================//
    void Save()
    {
        if (Data != null)
        {
            Data.Save(DataPath);
        }       
    }

    //============================================================================================================================================//
    void SaveToBase()
    {
        if (Data != null)
        {
            string basePath = Application.dataPath + "/StringTheory.xml";
            Data.Save(basePath);
        }
    }

    //============================================================================================================================================//
    void ResetScores()
    {
        if (Data != null)
        {
            Debug.Log("Reset All Scores");

            for (int i = 0; i < Data.Levels.Count; i++)
            {
                Data.Levels[i].Completed = false;
                Data.Levels[i].BestTime = 0;
                Data.Levels[i].BestLength = 0;
            }
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

        if (GUILayout.Button("Save To Base"))
        {
            SaveToBase();
            
        }
        if (GUILayout.Button("Reset Scores"))
        {
            ResetScores();
        }
        if (GUILayout.Button("Set GUIDs from Scenes"))
        {
            SetAllGUIDs();
        }
        if (GUILayout.Button("Set Root from Filename"))
        {
            // Get Root Object //
            GameObject rootObject = (GameObject)UnityEngine.Object.FindObjectOfType(typeof(GameObject));
            rootObject = rootObject.transform.root.gameObject;
            string currentScene = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);

            if (currentScene != rootObject.name)
            {
                rootObject.name = currentScene;
            }


            // Check if Nodes Exist //
        }
        if (GUILayout.Button("Set Filenames to Names"))
        {
            foreach (StringTheoryLevel level in Data.Levels)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(level.GUID);
                string file = level.Name + ".unity";
                if (Path.GetFileNameWithoutExtension(assetPath) != level.Name)
                {
                    Debug.Log("Renamed: " + assetPath + " to " + file);
                    AssetDatabase.RenameAsset(assetPath, level.Name);                   
                }

                string assetPathAfter = AssetDatabase.GUIDToAssetPath(level.GUID);
                level.Scene = assetPathAfter;
            }     
        }
        if (GUILayout.Button("Set Scenes from Filenames"))
        {
            List<string> buildLevels = GetBuildLevels();
            List<string> buildScenes = new List<string>();

            foreach (string  scene in buildLevels)
	        {
                string levelName = Path.GetFileNameWithoutExtension(scene);
                buildScenes.Add(levelName);
	        }

            foreach (StringTheoryLevel level in Data.Levels)
            {
                if (buildScenes.Contains(level.Name))
                {
                    int index = buildScenes.IndexOf(level.Name);
                    level.Scene = buildLevels[index];
                }
            }
        }
        
        SearchText = GUILayout.TextArea(SearchText);

        if (Data == null)
        {
            //Load();
        }
        else
        {

            // Levels //====================================================================================//
            GUILayout.Label("Levels", EditorStyles.boldLabel);

            for (int i = 0; i < Data.Levels.Count; i++)
            {
                if (Data.Levels[i].Name.Contains(SearchText))
                {
                    GUI.backgroundColor = (Data.Levels[i].Index > -1) ? Color.white : Color.red;

                    GUILayout.BeginHorizontal();
                    Foldouts[i] = EditorGUILayout.Foldout(Foldouts[i], (Data.Levels[i].Index > -1 ? Data.Levels[i].Index.ToString("000") : "____") + ": " + Data.Levels[i].Name);

                    GUILayout.FlexibleSpace();
                    GUILayout.Label(Data.Levels[i].Difficulty.ToString());

                    if (GUILayout.Button("Open"))
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(Data.Levels[i].GUID);
                        EditorApplication.OpenScene(assetPath);
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
                        Data.Levels[i].GUID = EditorGUILayout.TextField("GUID:", Data.Levels[i].GUID);
                        
                        //Data.Levels[i].Index = EditorGUILayout.IntField("Index:", Data.Levels[i].Index);
                        Data.Levels[i].Difficulty = EditorGUILayout.IntSlider("Difficulty:", Data.Levels[i].Difficulty, 0, 10);

                        //Data.Levels[i].Completed = EditorGUILayout.Toggle("Completed:", Data.Levels[i].Completed);
                        //Data.Levels[i].BestTime = EditorGUILayout.FloatField("Best Time:", Data.Levels[i].BestTime);
                        //Data.Levels[i].BestLength = EditorGUILayout.FloatField("Best Length:", Data.Levels[i].BestLength);
                    }

                    // Screenshot //
                    /*
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
                    }*/
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
                PackFoldouts[i] = EditorGUILayout.Foldout(PackFoldouts[i], Data.Packs[i].Name + ": " + Data.Packs[i].Levels.Count);

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
                        //Data.Packs[i].Levels[c] = EditorGUILayout.TextField(Data.Packs[i].Levels[c]);
                        
                        // Get Level Name from GUID //
                        StringTheoryLevel level = GetLevel(Data.Packs[i].Levels[c]);
                        if (level != null)
                        {
                            EditorGUILayout.LabelField(level.Name + ": " + level.Difficulty);

                            if (GUILayout.Button("Open"))
                            {
                                EditorApplication.OpenScene(level.Scene);
                            } 
                        }
                        else
                            EditorGUILayout.LabelField(Data.Packs[i].Levels[c]);


                        
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
                                string temp = Data.Packs[i].Levels[c];
                                Data.Packs[i].Levels[c] = Data.Packs[i].Levels[c + 1];
                                Data.Packs[i].Levels[c + 1] = temp;
                            }
                        }
                        GUILayout.EndHorizontal();
                        Rect packRect = GUILayoutUtility.GetLastRect();

                        // Drag and Drop ==============================================================//
                        if (Event.current.type == EventType.DragUpdated)
                        {
                            if (packRect.Contains(Event.current.mousePosition))
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                            }
                        }
                        if (Event.current.type == EventType.DragPerform)
                        {
                            if (packRect.Contains(Event.current.mousePosition))
                            {
                                List<string> newPacks = new List<string>();
                                for (int x = DragAndDrop.objectReferences.Length - 1; x >= 0; x--)
                                {
                                    string assetPath = AssetDatabase.GetAssetPath(DragAndDrop.objectReferences[x]);
                                    string guid = AssetDatabase.AssetPathToGUID(assetPath);

                                    if (Path.GetExtension(assetPath) == ".unity")
                                    {
                                        newPacks.Add(guid);
                                    }
                                }
                                newPacks.Sort();

                                int offset = 0;
                                for (int x = 0; x < newPacks.Count; x++)
                                {
                                    Data.Packs[i].Levels.Insert(c + offset, newPacks[x]);
                                    offset++;
                                }
                            }
                        }
                    }

                    /*if (GUILayout.Button("Add Level"))
                    {
                        Data.Packs[i].Levels.Add("");
                    }*/
                }
            }

            GUI.backgroundColor = Color.white;

            if (GUILayout.Button("New Pack"))
            {
                var pack = new StringTheoryPack();
                Data.Packs.Add(pack);
            }
        }

        GUILayout.EndScrollView();
        EditorGUILayout.EndVertical();      
    }

    //============================================================================================================================================//
    public StringTheoryLevel GetLevel(string guid)
    {
        StringTheoryLevel result = null;

        foreach (StringTheoryLevel level in Data.Levels)
        {
            if (level.GUID == guid)
            {
                return level;
            }
        }

        return result;
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
    public void SetAllGUIDs()
    {
        foreach (StringTheoryLevel level in Data.Levels)
        {
            string guid = AssetDatabase.AssetPathToGUID(level.Scene);
            level.GUID = guid;
        }
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