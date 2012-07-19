using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameCurve : MonoBehaviour 
{
    public bool EditMode = false;
    public bool Snapping = true;
    public bool AutoGenerate = true;
    
    public List<Vector2> Vertex = new List<Vector2>();
    public List<float> VertexSmooth = new List<float>();
    public List<Vector2> CurvedLine = new List<Vector2>();


    public float Smoothness = 0.5f;
    public float SmoothStep = 0.1f;
    public string MeshName = "Default";
    public float BorderThickness = 1;
    public bool CollisionMesh = true;
    public float ShapeUVScale = 1;
    public int BorderUVRepeat = 20;
    public float BorderUVOffset = 0;
    public float BorderOffset = 0;
    public float StartCapWidth = 0.5f;

    public float GridSpacing = 0.5f;
    public int GridSize = 40;

    // New //
    public bool Smoothed = true;
    public bool Looping = false;

    //======================================================================================================================================//
    public void CreateCurvedLine()
    {
        CurvedLine.Clear();
        GameCurve Mesh = this;

        for (int i = 0; i < Mesh.Vertex.Count; i++)
        {
            int index1 = i < Mesh.Vertex.Count - 1 ? i + 1 : i;
            int index2 = i < Mesh.Vertex.Count ? i : 0;
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

            // if looped //
            //CurvedLine.Add(p2);

            for (float a = 1f; a >= 0f; a -= SmoothStep)
            {
                Vector2 pos = p1 * Mathf.Pow(1 - a, 2f) + p2 * 2f * a * (1 - a) + p3 * Mathf.Pow(a, 2f);

                CurvedLine.Add(pos);
            }
        }
    }
}