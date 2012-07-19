using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor (typeof(GameCurve))]
class CurveEditor : Editor
{
    //======================================================================================================================================//
    int Selected = 0;
    int SelectedEdge = -1;
    bool vert = false;
    bool edge = false;

    Vector3[] StartCapVertices;
    Vector2[] StartCapUVs;
    int[] StartCapTriangles;

    Vector3[] EndCapVertices;
    Vector2[] EndCapUVs;
    int[] EndCapTriangles;

    Vector3[] BorderVertices;
    Vector2[] BorderUVs;
    int[] BorderTriangles;

    Vector3[] CollisionVertices;
    int[] CollisionTriangles;

    //======================================================================================================================================//
    void SelectClosestPoint(Vector2 position)
    {
        GameCurve Mesh = (GameCurve)target;

        float shortestdistance = 1000000;
        
        //Vertices //
        for (int i = 0; i < Mesh.Vertex.Count; i++)
        {
            Vector3 screenpos = Camera.current.WorldToScreenPoint(Mesh.Vertex[i]);

            float distance = Vector2.Distance(new Vector2(screenpos.x, Camera.current.pixelHeight - screenpos.y), position);

            if (distance < shortestdistance)
            {
                shortestdistance = distance;
                Selected = i;
                SelectedEdge = -1;
                vert = true;
                edge = false;
            }
        }

        // Edges //
        for (int i = 0; i < Mesh.Vertex.Count - 1; i++)
        {
            Vector3 screenpos = Camera.current.WorldToScreenPoint((Mesh.Vertex[i] + Mesh.Vertex[i + 1]) / 2);

            float distance = Vector2.Distance(new Vector2(screenpos.x, Camera.current.pixelHeight - screenpos.y), position);

            if (distance < shortestdistance)
            {
                shortestdistance = distance;
                SelectedEdge = i;
                Selected = -1;
                vert = false;
                edge = true;
            }
        }
        if (shortestdistance > 50)
        {
            Selected = -1;
            SelectedEdge = -1;
            vert = false;
            edge = false;
        }

    }

    //============================================================================================================================================//
    void GenerateMesh()
    {        
        GameCurve Mesh = (GameCurve)target;

        if (Mesh.MeshName == "Default")
        {
            EditorUtility.DisplayDialog("Mesh Generation Problem", "You must first change the generated mesh name", "Ok");
            return;
        }

        // Deleted Generated Meshes //
        AssetDatabase.MoveAssetToTrash("Assets/Models/Generated/" + Mesh.MeshName + ".asset");
        AssetDatabase.MoveAssetToTrash("Assets/Models/Generated/" + Mesh.MeshName + "_Collision.asset");

        // Build Border //
        Mesh.CreateCurvedLine();
        // if looped //
        //Mesh.CurvedLine.Add(Mesh.CurvedLine[0]);
        BuildBorder();
        BuildStartCap();
        BuildEndCap();


        // Combined Vertices and uvs //
        Vector3[] CombinedVertices = new Vector3[BorderVertices.Length + StartCapVertices.Length + EndCapVertices.Length];
        Vector2[] CombinedUVs = new Vector2[BorderVertices.Length + StartCapVertices.Length + EndCapVertices.Length];
        for (int i = 0; i < BorderVertices.Length; i++)
        {
            CombinedVertices[i] = BorderVertices[i];
            CombinedUVs[i] = BorderUVs[i];
        }
        for (int i = 0; i < StartCapVertices.Length; i++)
        {
            CombinedVertices[BorderVertices.Length + i] = StartCapVertices[i];
            CombinedUVs[BorderVertices.Length + i] = StartCapUVs[i];

        }
        for (int i = 0; i < EndCapVertices.Length; i++)
        {
            CombinedVertices[BorderVertices.Length + StartCapVertices.Length + i] = EndCapVertices[i];
            CombinedUVs[BorderVertices.Length + StartCapVertices.Length + i] = EndCapUVs[i];
        }

        // Offset Triangle Indexes //
        for (int i = 0; i < StartCapTriangles.Length; i++)
        {
            StartCapTriangles[i] += BorderVertices.Length;
        }
        for (int i = 0; i < EndCapTriangles.Length; i++)
        {
            EndCapTriangles[i] += (BorderVertices.Length + StartCapVertices.Length);
        }

        // Create the mesh 
        Mesh mesh = new Mesh();
        mesh.subMeshCount = 3;
        mesh.vertices = CombinedVertices;
        mesh.uv = CombinedUVs;
        mesh.SetTriangles(BorderTriangles, 0);
        mesh.SetTriangles(StartCapTriangles, 1);
        mesh.SetTriangles(EndCapTriangles, 2);
       
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshFilter filter;
        if (Mesh.GetComponent<MeshFilter>() == null)
        {
            filter = Mesh.gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        }
        else
        {
            filter = Mesh.GetComponent<MeshFilter>();
        }
        filter.mesh = mesh;  


        // Build Collision //
        BuildCollision();

        Mesh collisionmesh = new Mesh();
        collisionmesh.vertices = CollisionVertices;
        collisionmesh.triangles = CollisionTriangles;
        collisionmesh.RecalculateBounds();

        if (Mesh.CollisionMesh)
        {
            MeshCollider col = Mesh.GetComponent<MeshCollider>();
            col.sharedMesh = collisionmesh;
        }
        
    
        // Save Files //
        AssetDatabase.CreateAsset(mesh, "Assets/Models/Generated/" + Mesh.MeshName + ".asset");
        AssetDatabase.CreateAsset(collisionmesh, "Assets/Models/Generated/" + Mesh.MeshName + "_Collision.asset");
        AssetDatabase.SaveAssets();

    }

    //============================================================================================================================================//
    void BuildStartCap()
    {
        GameCurve Mesh = (GameCurve)target;

        if (Mesh.Vertex.Count > 1)
        {
            // Create Vertices //
            Vector2[] vertices2D = Mesh.Vertex.ToArray();

            // Create Triangles //
            StartCapVertices = new Vector3[4];
            StartCapUVs = new Vector2[4];
            StartCapTriangles = new int[6];

            // Generate Vertices //
            Vector3 vector;
            vector = vertices2D[0] - vertices2D[1];
            vector.Normalize();

            Vector3 left = new Vector3(vector.y * -1, vector.x, 0) + new Vector3(vector.y * -1 * Mesh.BorderOffset, vector.x * Mesh.BorderOffset, 0);
            Vector3 right = new Vector3(vector.y, vector.x * -1, 0) + new Vector3(vector.y * -1 * Mesh.BorderOffset, vector.x * Mesh.BorderOffset, 0);

            StartCapVertices[0] = new Vector3(vertices2D[0].x, vertices2D[0].y, 0) + left * Mesh.BorderThickness + (vector * Mesh.StartCapWidth);
            StartCapVertices[1] = new Vector3(vertices2D[0].x, vertices2D[0].y, 0) + right * Mesh.BorderThickness + (vector * Mesh.StartCapWidth);
            StartCapVertices[2] = new Vector3(vertices2D[0].x, vertices2D[0].y, 0) + left * Mesh.BorderThickness;
            StartCapVertices[3] = new Vector3(vertices2D[0].x, vertices2D[0].y, 0) + right * Mesh.BorderThickness;

            // Generate UVs //
            StartCapUVs[0] = new Vector2(0, 1);
            StartCapUVs[1] = new Vector2(0, 0);
            StartCapUVs[2] = new Vector2(1, 1);
            StartCapUVs[3] = new Vector2(1, 0);

            // Generate Triangles //
            for (int i = 0; i < 1; i++)
            {
                short t1 = (short)(0);
                short t2 = (short)(1);
                short t3 = (short)(2);
                short t4 = (short)(3);

                StartCapTriangles[i * 6] = t1;
                StartCapTriangles[i * 6 + 1] = t2;
                StartCapTriangles[i * 6 + 2] = t3;

                StartCapTriangles[i * 6 + 3] = t3;
                StartCapTriangles[i * 6 + 4] = t2;
                StartCapTriangles[i * 6 + 5] = t4;
            }
        }
    }

    //============================================================================================================================================//
    void BuildEndCap()
    {
        GameCurve Mesh = (GameCurve)target;

        if (Mesh.Vertex.Count > 1)
        {
            // Create Vertices //
            Vector2[] vertices2D = Mesh.Vertex.ToArray();

            // Create Triangles //
            EndCapVertices = new Vector3[4];
            EndCapUVs = new Vector2[4];
            EndCapTriangles = new int[6];

            // Generate Vertices //
            Vector3 vector;
            vector = vertices2D[vertices2D.Length - 2] - vertices2D[vertices2D.Length - 1];
            vector.Normalize();

            Vector3 left = new Vector3(vector.y * -1, vector.x, 0) + new Vector3(vector.y * -1 * Mesh.BorderOffset, vector.x * Mesh.BorderOffset, 0);
            Vector3 right = new Vector3(vector.y, vector.x * -1, 0) + new Vector3(vector.y * -1 * Mesh.BorderOffset, vector.x * Mesh.BorderOffset, 0);

            EndCapVertices[0] = new Vector3(vertices2D[vertices2D.Length - 1].x, vertices2D[vertices2D.Length - 1].y, 0) + left * Mesh.BorderThickness;
            EndCapVertices[1] = new Vector3(vertices2D[vertices2D.Length - 1].x, vertices2D[vertices2D.Length - 1].y, 0) + right * Mesh.BorderThickness;
            EndCapVertices[2] = new Vector3(vertices2D[vertices2D.Length - 1].x, vertices2D[vertices2D.Length - 1].y, 0) + left * Mesh.BorderThickness - (vector * Mesh.StartCapWidth);
            EndCapVertices[3] = new Vector3(vertices2D[vertices2D.Length - 1].x, vertices2D[vertices2D.Length - 1].y, 0) + right * Mesh.BorderThickness - (vector * Mesh.StartCapWidth);
            
            // Generate UVs //
            EndCapUVs[0] = new Vector2(0, 1);
            EndCapUVs[1] = new Vector2(0, 0);
            EndCapUVs[2] = new Vector2(1, 1);
            EndCapUVs[3] = new Vector2(1, 0);

            // Generate Triangles //
            for (int i = 0; i < 1; i++)
            {
                short t1 = (short)(0);
                short t2 = (short)(1);
                short t3 = (short)(2);
                short t4 = (short)(3);

                EndCapTriangles[i * 6] = t1;
                EndCapTriangles[i * 6 + 1] = t2;
                EndCapTriangles[i * 6 + 2] = t3;

                EndCapTriangles[i * 6 + 3] = t3;
                EndCapTriangles[i * 6 + 4] = t2;
                EndCapTriangles[i * 6 + 5] = t4;
            }
        }
    }

    //============================================================================================================================================//
    void BuildBorder()
    {
        GameCurve Mesh = (GameCurve)target;

        if (Mesh.Vertex.Count > 1)
        {
            Vector2[] vertices2D = Mesh.CurvedLine.ToArray();

            // Then create triangles //
            BorderVertices = new Vector3[vertices2D.Length * 2];
            BorderUVs = new Vector2[vertices2D.Length * 2];
            BorderTriangles = new int[(vertices2D.Length - 1) * 6];

            // Get Total Distance //
            float DistanceTravelled = 0;
            float TotalDistance = 0;
            
            for (int i = 0; i < vertices2D.Length - 1; i++)
            {
                TotalDistance += Vector2.Distance(vertices2D[i + 1], vertices2D[i]);           
            }

            // Generate Vertices //
            for (int i = 0; i < vertices2D.Length; i++)
            {
                // Generate the vertex positions //
                Vector3 vector;
                if (i == 0)
                {
                    vector = vertices2D[i] - vertices2D[i + 1];
                }
                else if (i == vertices2D.Length - 1)
                {
                    vector = vertices2D[i - 1] - vertices2D[i];
                }
                else
                {
                    vector = vertices2D[i - 1] - vertices2D[i + 1];
                }

                vector.Normalize();

                Vector3 left = new Vector3(vector.y * -1, vector.x, 0) + new Vector3(vector.y * -1 * Mesh.BorderOffset, vector.x * Mesh.BorderOffset, 0);
                Vector3 right = new Vector3(vector.y, vector.x * -1, 0) + new Vector3(vector.y * -1 * Mesh.BorderOffset, vector.x * Mesh.BorderOffset, 0);

                BorderVertices[i * 2] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0) + left * Mesh.BorderThickness;
                BorderVertices[i * 2 + 1] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0) + right * Mesh.BorderThickness;

                BorderUVs[i * 2] = new Vector2((DistanceTravelled / TotalDistance) * Mesh.BorderUVRepeat + Mesh.BorderUVOffset, 1);
                BorderUVs[i * 2 + 1] = new Vector2((DistanceTravelled / TotalDistance) * Mesh.BorderUVRepeat + Mesh.BorderUVOffset, 0);

                if (i < vertices2D.Length - 1)
                {
                    DistanceTravelled += Vector2.Distance(vertices2D[i + 1], vertices2D[i]);
                }
            }

            // Generate Triangles //
            for (int i = 0; i < vertices2D.Length - 1; i++)
            {
                short t1 = (short)(i * 2);
                short t2 = (short)(i * 2 + 1);
                short t3 = (short)(i * 2 + 2);
                short t4 = (short)(i * 2 + 3);

                BorderTriangles[i * 6] = t1;
                BorderTriangles[i * 6 + 1] = t2;
                BorderTriangles[i * 6 + 2] = t3;

                BorderTriangles[i * 6 + 3] = t3;
                BorderTriangles[i * 6 + 4] = t2;
                BorderTriangles[i * 6 + 5] = t4;

                // Draw Wireframe //
                if (true)
                {
                    Debug.DrawLine(BorderVertices[t1], BorderVertices[t2], Color.black);
                    Debug.DrawLine(BorderVertices[t3], BorderVertices[t4], Color.black);
                    Debug.DrawLine(BorderVertices[t1], BorderVertices[t3], Color.black);
                    Debug.DrawLine(BorderVertices[t2], BorderVertices[t4], Color.black);
                }
            }
        }
    }

    //============================================================================================================================================//
    void BuildCollision()
    {
        GameCurve Mesh = (GameCurve)target;

        if (Mesh.Vertex.Count > 1)
        {
            Vector2[] vertices2D = Mesh.CurvedLine.ToArray();
            CollisionVertices = new Vector3[vertices2D.Length * 2];
            CollisionTriangles = new int[(vertices2D.Length - 1) * 6];

            // Generate Vertices //
            for (int i = 0; i < vertices2D.Length; i++)
            {
                Vector3 vfront = new Vector3(vertices2D[i].x, vertices2D[i].y, -0.5f);
                Vector3 vback = new Vector3(vertices2D[i].x, vertices2D[i].y, 0.5f);

                CollisionVertices[i * 2] = vfront;
                CollisionVertices[i * 2 + 1] = vback;
            }

            // Generate Triangles //
            for (int i = 0; i < vertices2D.Length - 1; i++)
            {
                short t1 = (short)(i * 2);
                short t2 = (short)(i * 2 + 1);
                short t3 = (short)(i * 2 + 2);
                short t4 = (short)(i * 2 + 3);

                CollisionTriangles[i * 6] = t1;
                CollisionTriangles[i * 6 + 1] = t2;
                CollisionTriangles[i * 6 + 2] = t3;

                CollisionTriangles[i * 6 + 3] = t3;
                CollisionTriangles[i * 6 + 4] = t2;
                CollisionTriangles[i * 6 + 5] = t4;
            }
        }
    }
    
    //======================================================================================================================================//
    void DrawCurvedLine()
    {
        GameCurve Mesh = (GameCurve)target;

        Color drawcolor = Color.white;

        for (int i = 0; i < Mesh.Vertex.Count; i++)
        {
            int index1 = i < Mesh.Vertex.Count - 1 ? i + 1 : i;
            int index2 = i;
            int index3 = i > 0 ? i - 1 : i; 
            
            Vector2 direction = (Mesh.Vertex[index2] - Mesh.Vertex[index1]);
            float length = direction.magnitude;
            length = (length / 2 < Mesh.Smoothness) ? length / 2 : Mesh.Smoothness;
            direction = direction.normalized * length;

            Vector2 direction2 = (Mesh.Vertex[index3] - Mesh.Vertex[index2]);
            float length2 = direction2.magnitude;
            length2 = (length2 / 2 < Mesh.Smoothness) ? length2 / 2 : Mesh.Smoothness;
            direction2 = direction2.normalized * length2;

            Vector2 p1 = Mesh.Vertex[index2] - direction;
            Vector2 p2 = Mesh.Vertex[index2];
            Vector2 p3 = Mesh.Vertex[index2] + direction2;

            drawcolor = i == SelectedEdge ? Color.red : Color.white;
            Debug.DrawLine(Mesh.Vertex[index2] - direction, Mesh.Vertex[index1] + direction, drawcolor);
            Vector2 prevpos = p1;
            for (float a = 0; a <= 1.1f; a += 0.4f)
            {
                Vector2 pos = p1 * Mathf.Pow(1 - a, 2f) + p2 * 2f * a  * (1 - a) + p3 * Mathf.Pow(a, 2f);
                Debug.DrawLine(prevpos, pos, Color.white);
                prevpos = pos;
            }           
        }
    }

    //======================================================================================================================================//
    void OnSceneGUI()
    {
        GameCurve Mesh = (GameCurve)target;

        serializedObject.Update();

        // Draw Grid //
        float AspectRatio = 1;

        for (float x = -Mesh.GridSize * AspectRatio; x <= Mesh.GridSize * AspectRatio; x += Mesh.GridSpacing)
        {
            Debug.DrawLine(new Vector3(x, -Mesh.GridSize, 100), new Vector3(x, Mesh.GridSize, 100), Color.grey);
        }
        for (float y = -Mesh.GridSize; y <= Mesh.GridSize; y += Mesh.GridSpacing)
        {
            Debug.DrawLine(new Vector3(-Mesh.GridSize * AspectRatio, y, 100), new Vector3(Mesh.GridSize * AspectRatio, y, 100), Color.grey);
        }

        
        if (Mesh.EditMode)
        {
            // User Controll //
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            if (Event.current.type == EventType.layout)
                HandleUtility.AddDefaultControl(controlID);

            // Draw Line //
            for (int i = 0; i < Mesh.Vertex.Count; i++)
            {
                Handles.color = Color.white;
                Handles.SphereCap(controlID, Mesh.Vertex[i], Quaternion.identity, 0.2f);

                if (i < Mesh.Vertex.Count - 1)
                {
                    Handles.color = Color.grey;
                    Handles.SphereCap(controlID, (Mesh.Vertex[i] + Mesh.Vertex[i + 1]) / 2, Quaternion.identity, 0.2f);
                }
            }
            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.RightBracket)
                {
                    float dist = Mathf.Clamp(Mesh.VertexSmooth[Selected] + 0.2f, 0.2f, 2);
                    Mesh.VertexSmooth[Selected] = dist;
                    GenerateMesh();
                }
                else if (Event.current.keyCode == KeyCode.LeftBracket)
                {
                    float dist = Mathf.Clamp(Mesh.VertexSmooth[Selected] - 0.2f, 0.2f, 2);
                    Mesh.VertexSmooth[Selected] = dist;
                    Generate();
                }
            }
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 1 && Selected > -1)
                {
                    Mesh.Vertex.RemoveAt(Selected);
                    Selected = -1;
                }
                if (Event.current.button == 0 && SelectedEdge > -1 && edge)
                {
                    Mesh.Vertex.Insert(SelectedEdge + 1, Camera.current.ScreenToWorldPoint(new Vector2(Event.current.mousePosition.x, Camera.current.pixelHeight - Event.current.mousePosition.y)));
                    vert = true;
                    edge = false;
                    Selected = SelectedEdge + 1;
                }
                if (Event.current.button == 0 && !vert && !edge)
                {
                    Mesh.Vertex.Add(Camera.current.ScreenToWorldPoint(new Vector2(Event.current.mousePosition.x, Camera.current.pixelHeight - Event.current.mousePosition.y)));
                    vert = true;
                    edge = false;
                    Selected = Mesh.Vertex.Count - 1;
                }
            }

            if (Event.current.type == EventType.MouseMove)
            {
                SelectClosestPoint(Event.current.mousePosition);
            }
            
            if (Event.current.type == EventType.MouseDrag)
            {     
                if (Event.current.button == 0)
                {
                    Vector2 pos = Camera.current.ScreenToWorldPoint(new Vector2(Event.current.mousePosition.x, Camera.current.pixelHeight - Event.current.mousePosition.y));
                    if (Mesh.Snapping)
                    {
                        pos.x = pos.x - (pos.x % Mesh.GridSpacing);
                        pos.y = pos.y - (pos.y % Mesh.GridSpacing);
                    }                   

                    if (Mesh.Vertex[Selected] != pos)
                    {
                        Mesh.Vertex[Selected] = pos;
                        Generate();
                    }
                    
                }
            }

            if (Selected > -1 && vert)
            {
                Handles.color = Color.red;                   
                Handles.SphereCap(controlID, Mesh.Vertex[Selected], Quaternion.identity, 0.3f);
            }

            DrawCurvedLine();
        }
        //if (GUI.changed)
        EditorUtility.SetDirty(target);
        //SceneView.RepaintAll();
    }

    //======================================================================================================================================//
    void Generate()
    {
        if (((GameCurve)target).AutoGenerate)
        {
            GenerateMesh();
        }
    }

    //======================================================================================================================================//
    // Inspector User Interface //
    //======================================================================================================================================//
    public override void OnInspectorGUI()
    {
        var Mesh = (GameCurve)target;

        if (GUILayout.Button(Mesh.EditMode ? "Stop" : "Edit Shape"))
        {
            Mesh.EditMode = Mesh.EditMode ? false : true;
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Generate Mesh"))
        {
            GenerateMesh();
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Generated Mesh Name");
        Mesh.MeshName = EditorGUILayout.TextField(Mesh.MeshName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Auto Generate");
        Mesh.AutoGenerate = EditorGUILayout.Toggle(Mesh.AutoGenerate);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Attach Collision");
        bool bufferbool = EditorGUILayout.Toggle(Mesh.CollisionMesh);
        if (bufferbool != Mesh.CollisionMesh)
        {
            Mesh.CollisionMesh = bufferbool;
            Generate();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Smoothness");
        float buffer = EditorGUILayout.Slider(Mesh.Smoothness, 0.01f, 2);
        if (buffer != Mesh.Smoothness)
        {
            Mesh.Smoothness = buffer;
            Generate();
        }

        EditorGUILayout.LabelField("Smooth Step");
        buffer = EditorGUILayout.Slider(Mesh.SmoothStep, 0.01f, 2);
        if (buffer != Mesh.SmoothStep)
        {
            Mesh.SmoothStep = buffer;
            Generate();
        }

        EditorGUILayout.LabelField("Width");
        buffer = EditorGUILayout.Slider(Mesh.BorderThickness, 0f, 0.5f);
        if (buffer != Mesh.BorderThickness)
        {
            Mesh.BorderThickness = buffer;
            Generate();
        }

        EditorGUILayout.LabelField("Offset");
        buffer = EditorGUILayout.Slider(Mesh.BorderOffset, -1f, 1);
        if (buffer != Mesh.BorderOffset)
        {
            Mesh.BorderOffset = buffer;
            Generate();
        }

        EditorGUILayout.LabelField("UV Scale");
        int bufferint = EditorGUILayout.IntSlider(Mesh.BorderUVRepeat, 1, 500);
        if (bufferint != Mesh.BorderUVRepeat)
        {
            Mesh.BorderUVRepeat = bufferint;
            Generate();
        }

        EditorGUILayout.LabelField("UV Offset");
        buffer = EditorGUILayout.Slider(Mesh.BorderUVOffset, 0f, 1);
        if (buffer != Mesh.BorderUVOffset)
        {
            Mesh.BorderUVOffset = buffer;
            Generate();
        }

        EditorGUILayout.LabelField("Start Cap Width");
        buffer = EditorGUILayout.Slider(Mesh.StartCapWidth, 0f, 2);
        if (buffer != Mesh.StartCapWidth)
        {
            Mesh.StartCapWidth = buffer;
            Generate();
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Snapping");
        Mesh.Snapping = EditorGUILayout.Toggle(Mesh.Snapping);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Grid Spacing");
        Mesh.GridSpacing = EditorGUILayout.Slider(Mesh.GridSpacing, 0.1f, 2);

        EditorGUILayout.LabelField("Grid Size");
        Mesh.GridSize = EditorGUILayout.IntSlider(Mesh.GridSize, 1, 100);

        //this.DrawDefaultInspector();
        this.Repaint();

        //SceneView.RepaintAll();
    }
}