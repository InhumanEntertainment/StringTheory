using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurveBuilder : MonoBehaviour {
	
	
	//logic
	public bool shouldStartDrawing = false;
	
	//drawing
	public List<Vector3> Tail = new List<Vector3>();
	public float TailWidthStart = 1f;
	public float TailWidthEnd = 1f;
	public bool DrawWireframe = true;
	
	//need to start drawing only if we are in drawing mode

    Vector3 SmoothPosition;
    public float SmoothAmount = 0.9f;

    public List<float> TailWidth = new List<float>();
    public float Width;
    public float WidthVelocity = 0;
    public float WidthVelocityMax = 0.1f;
    
    public float WidthMin = 0.3f;
    public float WidthMax = 0.1f;
    public float WidthChange = 0.1f;
    
	 //============================================================================================================================================//
	void Update () 
    {
		if (Input.GetMouseButton(0)) 
        {
		 	Debug.Log("Touch Position: " + Input.mousePosition + " " + Input.mousePosition);
			if (hasTouchedMe(Input.mousePosition))
            {
				shouldStartDrawing = true;
                SmoothPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
		}
        else
        {
			
			if (shouldStartDrawing) 
            {
				shouldStartDrawing = false;	
				GameObject cloneNode = (GameObject) Instantiate(Resources.Load("StringNode"));
			}
		}
		
		if (shouldStartDrawing) 
        {
			var worldTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SmoothPosition =  Vector3.Lerp(worldTouchPosition, SmoothPosition, SmoothAmount);
            addPointToTail(new Vector3(SmoothPosition.x, SmoothPosition.y, 0));
		}

		BuildMesh();					
	}

    //============================================================================================================================================//
    bool hasTouchedMe(Vector3 touchPosition)
    {
		Ray ray = Camera.main.ScreenPointToRay(touchPosition);
		RaycastHit hit;
		if (gameObject.collider.Raycast(ray,out hit,50.0f)) 
        {
			Debug.Log("Hit");
			 Debug.DrawLine (ray.origin, hit.point);
			return true;
		}
        else
        {
			return false;
		}
	}

    //============================================================================================================================================//
    void addPointToTail(Vector3 touchPosition)
    {
		Tail.Add(touchPosition);

        // Varying Line Width //
        float acceleration = (WidthChange * Random.value - WidthChange * 0.5f);
        WidthVelocity += acceleration;
        WidthVelocity = Mathf.Clamp(WidthVelocity, -WidthVelocityMax, WidthVelocityMax);
        Width += WidthVelocity;

        if (Width > WidthMax)
        {
            Width = WidthMax;
            WidthVelocity = 0;
        } 
        if (Width < WidthMin)
        {
            Width = WidthMin;
            WidthVelocity = 0;
        }

        TailWidth.Add(Width);
	}
	
	//============================================================================================================================================//
	float UVScroll = 0;
	void BuildMesh()
    {
        if (Tail.Count > 1)
        {
            // Then create triangles //
            Vector3[] vertices = new Vector3[Tail.Count * 2];
            Vector2[] uv = new Vector2[Tail.Count * 2];
            int[] triangles = new int[(Tail.Count - 1) * 6];

            // Generate Vertices //
            for (int i = 0; i < Tail.Count; i++)
            {
                // Generate the vertex positions //
                Vector3 vector;
                if (i == 0)
                {
                    vector = Tail[i] - Tail[i + 1];
                }
                else if (i == Tail.Count - 1)
                {
                    vector = Tail[i - 1] - Tail[i];
                }
                else
                {
                    vector = Tail[i - 1] - Tail[i + 1];
                }

                vector.Normalize();

                Vector3 left = new Vector3(vector.y * -1, vector.x, 0);
                Vector3 right = new Vector3(vector.y, vector.x * -1, 0);

                // from 0 to 1 along the length of the tail //
                float v = 1 - ((float)i / (Tail.Count - 1));
                float tailwidth = Mathf.Lerp(TailWidthStart, TailWidthEnd, v);
                //vertices[i * 2] = Tail[i] + left * tailwidth;
                //vertices[i * 2 + 1] = Tail[i] + right * tailwidth;

                vertices[i * 2] = Tail[i] + left * TailWidth[i];
                vertices[i * 2 + 1] = Tail[i] + right * TailWidth[i];

                UVScroll += Time.deltaTime * 0.0f;
                uv[i * 2] = new Vector2(0, v * 1 + UVScroll);
                uv[i * 2 + 1] = new Vector2(1, v * 1 + UVScroll);

                //Debug.DrawLine(Tail[i] + left, Tail[i] + right, Color.blue);
            }

            // Generate Triangles //
            for (int i = 0; i < Tail.Count - 1; i++)
            {
                int t1 = i * 2;
                int t2 = i * 2 + 1;
                int t3 = i * 2 + 2;
                int t4 = i * 2 + 3;

                triangles[i * 6] = t1;
                triangles[i * 6 + 1] = t2;
                triangles[i * 6 + 2] = t3;

                triangles[i * 6 + 3] = t3;
                triangles[i * 6 + 4] = t2;
                triangles[i * 6 + 5] = t4;

                // Draw Wireframe //
                if (DrawWireframe)
                {
                    Debug.DrawLine(vertices[t1], vertices[t2], Color.black);
                    Debug.DrawLine(vertices[t3], vertices[t4], Color.black);
                    Debug.DrawLine(vertices[t1], vertices[t3], Color.black);
                    Debug.DrawLine(vertices[t2], vertices[t4], Color.black);
                }
            }

            // Draw Tail Mesh //
            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            MeshFilter m = GetComponent<MeshFilter>();
			//m.mesh = mesh;
            Graphics.DrawMesh(mesh, Matrix4x4.identity, renderer.material, 0);
        }
    }
}
