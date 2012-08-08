using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    ColorBase SelectedBase;
    Vector2 DownPosition;
    Vector3 WorldDownPosition;

    static public Vector3[] SavedPositions;

    //======================================================================================================================================//
    void SavePositions()
    {
        ColorBase[] bases = (ColorBase[])GameObject.FindObjectsOfType(typeof(ColorBase));
        SavedPositions = new Vector3[bases.Length];

        for (int i = 0; i < bases.Length; i++)
        {
            SavedPositions[i] = bases[i].transform.position;
            Debug.Log("Saved " + i + ": " + SavedPositions[i]);
        }
        
    }

    //======================================================================================================================================//
    void RestorePositions()
    {
        ColorBase[] bases = (ColorBase[])GameObject.FindObjectsOfType(typeof(ColorBase));

        Undo.RegisterUndo(bases, "Level Editor: Restore Positions");

        if (bases.Length == SavedPositions.Length)
        {
            for (int i = 0; i < bases.Length; i++)
            {
                bases[i].transform.position = SavedPositions[i];
                Debug.Log("Restored " + i + ": " + SavedPositions[i]);
            }

            SavedPositions = null;
        }
        else
        {
            Debug.LogError("Number of bases is not the same as number of stored base positions");
        }
    }

    //======================================================================================================================================//
    void OnSceneGUI()
    {
        Level level = (Level)target;
        serializedObject.Update();

        // User Control //
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        
        // Repaint //
        if (Event.current.type == EventType.Repaint)
        {
            if (SelectedBase != null && level.EditMode)
            {
                Handles.color = Color.white;
                Handles.SphereCap(controlID, SelectedBase.transform.position, Quaternion.identity, 0.5f);
                //Handles.SphereCap(controlID, WorldDownPosition, Quaternion.identity, 0.3f);
            }

            //DrawGrid(level);
        }

        // UI //
        Handles.BeginGUI(new Rect(Screen.width - 370, Screen.height - 100, 360, 100));

        string editText = level.EditMode ? "Stop Editing" : "Edit";
        if (GUI.Button(new Rect(240, 0, 120, 50), editText))
            level.EditMode = !level.EditMode;

        string snapText = level.Snapping ? "Snapping On" : "Snapping Off";
        if (GUI.Button(new Rect(120, 0, 120, 50 ), snapText))
            level.Snapping = !level.Snapping;

        // Restore Positions //
        if (!Application.isPlaying && SavedPositions != null)
        {
            if (GUI.Button(new Rect(0, 0, 120, 50), "Restore Positions"))
                RestorePositions();
        }       

        GUI.EndGroup();
        Handles.EndGUI();

        // Edit Mode //
        if (level.EditMode)
        {
            if (Event.current.type == EventType.layout)
                HandleUtility.AddDefaultControl(controlID);
  
            if (Event.current.type == EventType.MouseMove)
            {
                ColorBase colorBase = SelectClosestBase(Event.current.mousePosition);

                if (colorBase != null && SelectedBase != colorBase)
                {
                    SelectedBase = colorBase;                  
                }               
            }

            if (Event.current.type == EventType.mouseDown)
            {
                if (Event.current.button == 0 && SelectedBase != null)
                {
                    Undo.RegisterUndo(SelectedBase, "Level Editor: Move");

                    DownPosition = Event.current.mousePosition;
                    WorldDownPosition = SelectedBase.transform.position;
                }
            }


            if (Application.isPlaying && Event.current.type == EventType.mouseUp)
            {
                SavePositions();
            }

            
            if (Event.current.type == EventType.mouseDrag)
            {
                if (Event.current.button == 0 && SelectedBase != null)
                {
                    Vector3 worldMouse = Camera.current.ScreenToWorldPoint(new Vector2(Event.current.mousePosition.x, Camera.current.pixelHeight - Event.current.mousePosition.y));
                    Vector3 worldDown = Camera.current.ScreenToWorldPoint(new Vector2(DownPosition.x, Camera.current.pixelHeight - DownPosition.y));
                    Vector3 offset = worldMouse - worldDown; 
                    offset.z = 0;

                    SelectedBase.transform.position = WorldDownPosition + offset;

                    // Snap to Grid //  
                    if (level.Snapping)
                    {
                        Transform xform = SelectedBase.transform;

                        float stepX = 1f / (level.GridX) * level.ScreenSize * 2;
                        float stepY = 1f / (level.GridY) * level.ScreenSize * 2;
                        float offsetX = ((level.GridX % 2) == 1) ? 0 : 0.5f * stepX;
                        float offsetY = ((level.GridY % 2) == 1) ? 0 : 0.5f * stepY;

                        float x = Mathf.Round((xform.position.x - offsetX) / stepX);
                        float y = Mathf.Round((xform.position.y - offsetY) / stepY);
                        float hexY = (xform.position.y + level.ScreenSize) / (level.ScreenSize * 2);
                        hexY = Mathf.Floor(hexY * level.GridY);

                        float hexOffset = 0;
                        if (level.HexGrid)
                            hexOffset = ((hexY % 2) == 0) ? 0 : stepX * 0.5f;

                        Vector3 newPosition = new Vector3(x * stepX + offsetX + hexOffset, y * stepY + offsetY, xform.position.z);
                        xform.position = newPosition;
                    }
                }
            }
        }

        //if (GUI.changed)
        EditorUtility.SetDirty(target);
        //SceneView.RepaintAll();
    }

    //======================================================================================================================================//
    public override void OnInspectorGUI()
    {
        Level level = (Level)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name");
        level.Name = EditorGUILayout.TextField(level.Name);
        EditorGUILayout.EndHorizontal();
        
        string editText = level.EditMode ? "Stop Editing" : "Edit";
        if (GUILayout.Button(editText))
        {
            level.EditMode = !level.EditMode;
            EditorUtility.SetDirty(target);
        }

        
        if (GUILayout.Button("ReSnap All Bases"))
        {
            SnapBases();
            EditorUtility.SetDirty(target);
        } 
        
        if (GUILayout.Button("Resize All Bases"))
        {
            ResizeBases();
            EditorUtility.SetDirty(target);
        }

        EditorGUILayout.LabelField("Base Scale");
        level.BaseScale = EditorGUILayout.Slider(level.BaseScale, 0.1f, 8f);


        string gridText = level.HexGrid ? "Grid: Hex" : "Grid: Square";
        if (GUILayout.Button(gridText))
        {
            level.HexGrid = !level.HexGrid;
            EditorUtility.SetDirty(target);
        }

        string snapText = level.Snapping ? "Snapping On" : "Snapping Off";
        if (GUILayout.Button(snapText))
        {
            level.Snapping = !level.Snapping;
            EditorUtility.SetDirty(target);
        }

        EditorGUILayout.LabelField("Grid X");
        level.GridX = EditorGUILayout.IntSlider(level.GridX, 4, 40);

        EditorGUILayout.LabelField("Grid Y");
        level.GridY = EditorGUILayout.IntSlider(level.GridY, 4, 40);

        //this.DrawDefaultInspector();
        EditorUtility.SetDirty(target);
        this.Repaint();
    }

    //======================================================================================================================================//
    public void SnapBases()
    {
        Level level = (Level)target;
        Object[] objects = GameObject.FindObjectsOfType(typeof(ColorBase));

        Undo.RegisterUndo(objects, "Level Editor: Snap Bases");

        for (int i = 0; i < objects.Length; i++)
        {
            Transform xform = ((ColorBase)objects[i]).transform;

            float stepX = 1f / (level.GridX) * level.ScreenSize * 2;
            float stepY = 1f / (level.GridY) * level.ScreenSize * 2;
            float offsetX = ((level.GridX % 2) == 1) ? 0 : 0.5f * stepX;
            float offsetY = ((level.GridY % 2) == 1) ? 0 : 0.5f * stepY;

            float x = Mathf.Round((xform.position.x - offsetX) / stepX);
            float y = Mathf.Round((xform.position.y - offsetY) / stepY);
            float hexY = (xform.position.y + level.ScreenSize) / (level.ScreenSize * 2);
            hexY = Mathf.Floor(hexY * level.GridY);

            float hexOffset = 0;
            if (level.HexGrid)
                hexOffset = ((hexY % 2) == 0) ? 0 : stepX * 0.5f;

            Vector3 newPosition = new Vector3(x * stepX + offsetX + hexOffset, y * stepY + offsetY, xform.position.z);
            xform.position = newPosition;
        }
    }

    //======================================================================================================================================//
    public void ResizeBases()
    {
        Level level = (Level)target;
        Object[] objects = GameObject.FindObjectsOfType(typeof(ColorBase));
        Undo.RegisterUndo(objects, "Level Editor: Resize Bases");

        for (int i = 0; i < objects.Length; i++)
        {
            Transform xform = ((ColorBase)objects[i]).transform;

            float stepX = 1f / (level.GridX) * level.ScreenSize * 2;
            float stepY = 1f / (level.GridY) * level.ScreenSize * 2;

            float scale = Mathf.Min(stepX, stepY) * level.BaseScale;
            xform.localScale = new Vector3(scale, scale, scale);
        }
    }

    //======================================================================================================================================//
    static void DrawGrid(Level level)
    {
        if (!level.HexGrid)
        {
            // Draw Grid //
            if (level.GridX > 0)
            {
                float stepX = 1f / level.GridX * level.ScreenSize * 2;

                for (float x = -level.ScreenSize; x <= level.ScreenSize; x += stepX)
                {
                    Debug.DrawLine(new Vector3(x, -level.ScreenSize, 0), new Vector3(x, level.ScreenSize, 0));
                }
            }

            if (level.GridY > 0)
            {
                float stepY = 1f / level.GridY * level.ScreenSize * 2;

                for (float y = -level.ScreenSize; y <= level.ScreenSize; y += stepY)
                {
                    Debug.DrawLine(new Vector3(-level.ScreenSize, y, 0), new Vector3(level.ScreenSize, y, 0));
                }
            }
        }
        else
        {
            // Draw Hex //
            if (level.GridY > 0)
            {
                float stepY = 1f / level.GridY * level.ScreenSize * 2;

                for (float y = -level.ScreenSize; y <= level.ScreenSize; y += stepY)
                {
                    Debug.DrawLine(new Vector3(-level.ScreenSize, y, 0), new Vector3(level.ScreenSize, y, 0));
                }
            }

            if (level.GridY > 0 && level.GridX > 0)
            {
                float stepX = 1f / level.GridX * level.ScreenSize * 2;
                float stepY = 1f / level.GridY * level.ScreenSize * 2;
                bool offset = false;

                for (float y = level.ScreenSize; y > -level.ScreenSize; y -= stepY)
                {
                    offset = !offset;
                    for (float x = -level.ScreenSize; x <= level.ScreenSize; x += stepX)
                    {
                        float offsetX = offset ? 0 : stepX * 0.5f;
                        //print(offsetX);

                        Debug.DrawLine(new Vector3(x + offsetX, y, 0), new Vector3(x + offsetX, y - stepY, 0));
                    }
                }
            }
        }

        //Debug.color = Color.cyan;
        Debug.DrawLine(new Vector3(-level.ScreenSize, -level.ScreenSize, 0), new Vector3(level.ScreenSize, -level.ScreenSize, 0));
        Debug.DrawLine(new Vector3(-level.ScreenSize, level.ScreenSize, 0), new Vector3(level.ScreenSize, level.ScreenSize, 0));
        Debug.DrawLine(new Vector3(-level.ScreenSize, -level.ScreenSize, 0), new Vector3(-level.ScreenSize, level.ScreenSize, 0));
        Debug.DrawLine(new Vector3(level.ScreenSize, -level.ScreenSize, 0), new Vector3(level.ScreenSize, level.ScreenSize, 0));
    }

    //======================================================================================================================================//
    ColorBase SelectClosestBase(Vector2 position)
    {
        ColorBase[] bases = (ColorBase[])GameObject.FindObjectsOfType(typeof(ColorBase));


        float shortestdistance = 1000000;
        ColorBase closestBase = null;

        //Vertices //
        for (int i = 0; i < bases.Length; i++)
        {
            Vector3 screenpos = Camera.current.WorldToScreenPoint(bases[i].transform.position);
            float distance = Vector2.Distance(new Vector2(screenpos.x, Camera.current.pixelHeight - screenpos.y), position);

            if (distance < shortestdistance)
            {
                shortestdistance = distance;
                closestBase = bases[i];             
            }
        }

        if (shortestdistance > 50)
        {
            closestBase = null;
            SelectedBase = null;
        }

        return closestBase;
    }

    //=====================================================================================================================================//
    static bool hadBeenConnected = false;
    static void ConnectBases()
    {
        Debug.Log("will try to connect bases");
        ClearAllBaseConnections();

        if (!hadBeenConnected)
        {
            ColorBase[] bases = (ColorBase[])GameObject.FindObjectsOfType(typeof(ColorBase));

            for (int i = 0; i < bases.Length; i++)
            {
                Debug.Log("try to connect base of index " + i);
                ConnectPairForBase(bases[i], bases);

            }
            hadBeenConnected = true;
        }
    }

    //=====================================================================================================================================//
    static void ClearAllBaseConnections()
    {
        ColorBase[] bases = (ColorBase[])GameObject.FindObjectsOfType(typeof(ColorBase));

        for (int i = 0; i < bases.Length; i++)
        {
            bases[i].colorBasePeers.Clear();
        }
    }

    //=====================================================================================================================================//
    static void ConnectPairForBase(ColorBase colorBase, ColorBase[] allBases)
    {
        for (int i = 0; i < allBases.Length; i++)
        {
            ColorBase potentialPair = allBases[i];
            if (potentialPair != colorBase)
            {
                Debug.Log("find potential pair that does not match base at index: " + i);

                bool isColorMatch = (potentialPair.baseName == colorBase.baseName);
                if (isColorMatch)
                {
                    Debug.Log(" find matching base and pair pair with name " + colorBase.baseName + " matching name of pair: " + potentialPair.baseName);
                    colorBase.colorBasePeers.Add(potentialPair);
                }
            }
        }
    }
}
