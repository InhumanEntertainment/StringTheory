using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorString : MonoBehaviour {
	
	//logic//
	public bool IsCurveBeingDrawn = false;
	public bool HasCurveReachHisBases = false;
	
	public List<ColorBase> BasesExpected = new List<ColorBase>();
	public List<ColorBase> BasesToAvoid = new List<ColorBase>();
	List<ColorBase> BasesReached = new List<ColorBase>();
	
	//drawing//
	public List<Vector3> Tail = new List<Vector3>();
	public float TailWidthStart = 1f;
	public float TailWidthEnd = 1f;
	public bool DrawWireframe = true;
	
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
		bool hasTouchStarted = (Input.GetMouseButtonDown(0));
		bool isTouchUpdated = Input.GetMouseButton(0);
		
		if (hasTouchStarted) {
			
			if (! IsCurveBeingDrawn) 
			{
				if (HasCurveBeenHitAtPosition(Input.mousePosition))
				{
					Debug.Log("Curve has been Hit");
					CutCurveIfLastPointDoesNotMatchPosition(Input.mousePosition);
					InitializeCurveToResumeDrawingAtPosition(Input.mousePosition);
				}
			}
			else
			{
				//should not be possible unless multi touch//
				Debug.LogError("Start a touch while being drawn. IsCurvedBeingDrawn is probably not being handle correctly");	
			}
		}
		else if (isTouchUpdated) 
		{
			if (Tail.Count == 0) {
				InitializeCurveToResumeDrawingAtPosition(Input.mousePosition);
			}
			
			if (IsCurveBeingDrawn) 
			{
				AddScreenPointToTail(Input.mousePosition);
				if (Tail.Count > 2) {
					StopDrawingIfLastScreenPointEncouterBaseOrSelf(Input.mousePosition);
				}
			}
		}
		else 
		{
			//touch has been cancelled//
			if (IsCurveBeingDrawn) {
				IsCurveBeingDrawn = false;
			}
			else
			{
				Debug.Log("No touch");	
			}
		}
		BuildMesh();
	}
	
	
	//============================================================================================================================================//
	void curveDidConnectWithMatchingBase(ColorBase colorBase) 
	{
		//put here reactions to this event//
	}
	
	//============================================================================================================================================//
	void curveDidConnectWithWrongBase(ColorBase colorBase) 
	{
		Tail = new List<Vector3>();
		//TODO inform original base that the curve has been destroyed somehow//
	}
	
	//============================================================================================================================================//
	void StopDrawingIfLastScreenPointEncouterBaseOrSelf(Vector3 mousePosition) 
	{
		
		
		//code to manage colorBase encounter//
		foreach (ColorBase colorBase in BasesExpected)
		{
			Debug.Log("Check if collide with base named: " + colorBase.baseName);
			if (isLastPointCollidingWithBase(colorBase))
			{
				curveDidConnectWithMatchingBase(colorBase);
			}
		}
		
		foreach (ColorBase colorBase in BasesToAvoid) 
		{
			if (isLastPointCollidingWithBase(colorBase))
			{
				curveDidConnectWithWrongBase(colorBase);
			}	
		}
		
		//TODO code to manage self encounter//
		//TODO need to implement the meeting with original base to avoid circular matching//
	}
	
	//============================================================================================================================================//
	bool isLastPointCollidingWithBase(ColorBase colorBase)
	{
		bool res = false;
		if (Tail.Count > 1) {
			Vector3 lastPoint = Tail[Tail.Count-1];
			res =  colorBase.IsCollidingWithPoint(lastPoint);
		}
		return res;
	}
	
	
	//============================================================================================================================================//
	void InitializeCurveToResumeDrawingAtPosition(Vector3 position)
	{
		SmoothPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		IsCurveBeingDrawn = true;
	}
	
	
	//============================================================================================================================================//
	void CutCurveIfLastPointDoesNotMatchPosition(Vector3 position)
	{
		Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 curvePointTouched = GetPointTouchedOnCurvedIfExistAtPosition(worldTouchPosition);
		
		bool hasTouchedCurve = (curvePointTouched.z == 0);

		if (hasTouchedCurve) 
		{	
			Vector3 lastPointInTail = Tail[Tail.Count-1];
			bool lastPointHasBeenTouched = Vector3.Equals(curvePointTouched,lastPointInTail);
			
			if (! lastPointHasBeenTouched) 
			{
				RemoveAllItemsFromTailAfterPoint(curvePointTouched);
			}
		}
	}
	
	//============================================================================================================================================//
	void RemoveAllItemsFromTailAfterPoint(Vector3 position) 
	{
		int indexPosition = Tail.IndexOf(position);
		Tail.RemoveRange (indexPosition,Tail.Count - indexPosition);	
	}
	
	
	//============================================================================================================================================//
	bool HasCurveBeenHitAtPosition(Vector3 touchPosition) 
	{
		Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 curvePointTouched = GetPointTouchedOnCurvedIfExistAtPosition(worldTouchPosition);
		bool hasTouchedCurve = (curvePointTouched.z == 0);
		return hasTouchedCurve;
	}
	
	
	//============================================================================================================================================//
	Vector3 GetPointTouchedOnCurvedIfExistAtPosition(Vector3 touchPosition) 
	{	
		Vector3 pointTouched = new Vector3(0,0,-100);
		
		if (Tail.Count > 0)
		{	
			Vector3 touchPosition2D = new Vector3(touchPosition.x,touchPosition.y,0);
			
			Vector3 closestCurvePoint = GetClosestPoint(touchPosition2D);
			
			Debug.Log("Closest point found is point with X: " + closestCurvePoint.x + " Y: " + closestCurvePoint.y + " Z: " + closestCurvePoint.z);
			Debug.Log("World touch position has X " + touchPosition2D.x + "Y: " + touchPosition2D.y + " Z: " + touchPosition2D.z);
			
			float distance = Vector3.Distance(touchPosition2D,closestCurvePoint);
			Debug.Log("Distance between closest and touch is: " + distance + " and width of curve is: " + Width);
			
			bool hasCurveBeenTouched = (distance < Width * 2) ;
			if (hasCurveBeenTouched) {
				pointTouched = closestCurvePoint;
			}	
		}
		return pointTouched;
	}
	
	//============================================================================================================================================//
	Vector3 GetClosestPoint(Vector3 position)
    {
        int closest = 0;
        float closestdistance = Vector3.Distance(position, Tail[0]);

        for (int i = 1; i < Tail.Count; i++)
        {
            float distance = Vector3.Distance(position, Tail[i]);
            if (distance < closestdistance)
            {
                closestdistance = distance;
                closest = i;
            }
        }
        return Tail[closest];
    }
	
	//============================================================================================================================================//
    void AddScreenPointToTail(Vector3 touchPosition)
    {
		
		
		//smoothing the touch Position//
		
		var worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        SmoothPosition =  Vector3.Lerp(worldTouchPosition, SmoothPosition, SmoothAmount);
		Vector3 smoothedVector = new Vector3(SmoothPosition.x, SmoothPosition.y, 0);
		
		Tail.Add(smoothedVector);
		
		//Tail.Add(touchPosition);

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
