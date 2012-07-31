using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorString : MonoBehaviour 
{
	
	//properties//
	public float CurveLength = 0f;	
	
	//logic//
	public bool IsCurveBeingDrawn = false;	
	public bool HasCurveReachedTarget = false;
	
	public ColorBase BaseStart;
	public List<ColorBase> BasesExpected = new List<ColorBase>();
	public List<ColorBase> BasesToAvoid = new List<ColorBase>();
	
	//ending management
	public GameObject TouchTracker;
	public GameObject ArrowTracker;
	GameObject CurrentTracker;
	
	
	//drawing//
	
	public float Stepper = 0.1f;
	public int MinLoopLength = 5;
	public float MinDistance = 0.25f;
	
	public Mesh mesh;
	
	public List<Vector3> Tail = new List<Vector3>();
	public float TailWidthStart = 1f;
	public float TailWidthEnd = 1f;
	public bool DrawWireframe = true;
	
    Vector3 SmoothPosition;
    public float SmoothAmount = 0.9f;
    public int SmoothCount = 5;
    public float CornerShift = 0.2f;

    public List<float> TailWidth = new List<float>();
    public float Width;
    public float WidthVelocity = 0;
    public float WidthVelocityMax = 0.1f;
    
    public float WidthMin = 0.3f;
    public float WidthMax = 0.1f;
    public float WidthChange = 0.1f;

    public int ColorIndex;
    public Material[] ColorMaterials;
    public Game Game;

    // Effects //
    public ParticleSystem FXComplete;
    public ParticleSystem FXDraw;
    public ParticleSystem FXCut;
    public ParticleSystem FXDrawObject;

    public Color[] fxColors = new Color[] { new Color(0, 1, 0), new Color(1, 1f, 0f), new Color(0.2f, 0, 1f), new Color(0f, 1f, 1), new Color(0.7f, 1, 0), new Color(0.5f, 0, 1f) };

    Mesh CurveMesh;
	
	//============================================================================================================================================//
    void Awake()
    {
        // Does this cause memory leak?? //
        MeshFilter m = GetComponent<MeshFilter>();
        CurveMesh = new Mesh();
        m.mesh = CurveMesh;

        Game = (Game)GameObject.FindObjectOfType(typeof(Game));

        FXDrawObject = (ParticleSystem)Game.Spawn(FXDraw, transform.position, Quaternion.identity);
        FXDrawObject.emissionRate = 0;          
    }
    
    //============================================================================================================================================//
	void Update () 
	{
        if (!Game.Paused)
        {
            bool hasTouchStarted = (Input.GetMouseButtonDown(0));
            bool isTouchUpdated = Input.GetMouseButton(0);

            if (Input.touches.Length <= 1)
            {

                if (hasTouchStarted)
                {

                    if (!IsCurveBeingDrawn)
                    {
                        CutAndResumeDrawingIfNecessary();
                        /*
                        if (HasCurveBeenHitAtPosition(Input.mousePosition))
                        {
                            //Game.Log("Curve has been Hit");
                            CutCurveIfLastPointDoesNotMatchPosition(Input.mousePosition);
                            InitializeCurveToResumeDrawingAtPosition(Input.mousePosition);
                        }*/
                    }
                    else
                    {
                        //should not be possible unless multi touch//
                        //Debug.LogError("Start a touch while being drawn. IsCurvedBeingDrawn is probably not being handle correctly");	
                    }
                }
                else if (isTouchUpdated)
                {
                    if (Tail.Count == 0)
                    {
                        InitializeCurveToResumeDrawingAtPosition(Input.mousePosition);
                    }

                    if (IsCurveBeingDrawn && !HasCurveReachedTarget)
                    {
                        if (HasFoundAnotherCurveDrawing())
                        {
                            IsCurveBeingDrawn = false;
                        }
                        else
                        {
                            
							
					Vector3 mousePosition =  Camera.main.ScreenToWorldPoint( Input.mousePosition);
						
					if (mousePosition.y <= -4.9f) 
						{
							mousePosition.y = -4.9f;	
						}
						
					if (mousePosition.y >= 4.9f) 
						{
							mousePosition.y = 4.9f;	
						}	
					
						if (mousePosition.x <= -4.9f) 
						{
							mousePosition.x = -4.9f;	
						}
						
					if (mousePosition.x >= 4.9f) 
						{
							mousePosition.x = 4.9f;	
						}	

							
							List<Vector3> pointsToAdd = GetPointsToAddIfTouchPositionMatchDistanceRequirement(mousePosition);
                            foreach (Vector3 point in pointsToAdd)
                            {
                                AddScreenPointToTail(point);
                                GameObject curveManager = GameObject.FindGameObjectWithTag("CurveManager");
                                CurveColliderDetector detector = curveManager.GetComponent<CurveColliderDetector>();
                                detector.CheckPositionForCurve(this);
                            }

                            if (Tail.Count > 2)
                            {
                                StopDrawingIfLastScreenPointEncouterBaseOrSelf(Input.mousePosition);
                            }
                        }
                    }
                    else
                    {
                        if (!IsCurveBeingDrawn)
                        {
                            //CutAndResumeDrawingIfNecessary();		
                            CutWithoutResumeDrawingIfNecessary();
                        }
                    }



                    //displaying of tracker
                    if (!HasCurveReachedTarget && IsCurveBeingDrawn)
                    {
                        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        UpdateTouchTrackerWithPosition(new Vector3(mousePosition.x, mousePosition.y, 0));
                    }

                }
                else
                {
                    //touch has been cancelled//
                    if (IsCurveBeingDrawn)
                    {
                        IsCurveBeingDrawn = false;
                    }
                    else
                    {
                        //Game.Log("No touch");	
                    }
                }
                BuildMesh();


                if (!IsCurveBeingDrawn)
                {
                    ShowArrowTracker();
                }
                else
                {
                    ShowTouchTracker();
                }

            }//end if touches count
        }
	}
					
	
	//============================================================================================================================================//
	void CutAndResumeDrawingIfNecessary()
	{
		if (HasCurveBeenHitAtPosition(Input.mousePosition))
		{
			Debug.Log("Curve has been Hit");
			CutCurveIfLastPointDoesNotMatchPosition(Input.mousePosition);
			InitializeCurveToResumeDrawingAtPosition(Input.mousePosition);
		}	
	}					
					
	//============================================================================================================================================//
	void CutWithoutResumeDrawingIfNecessary() 
	{
		if (HasCurveBeenHitAtPosition(Input.mousePosition))
		{
			Debug.Log("Curve has been Hit");
			CutCurveIfLastPointDoesNotMatchPosition(Input.mousePosition);
			if (!IsAnotherCurveBeingDrawnExists()) 
			{
				InitializeCurveToResumeDrawingAtPosition(Input.mousePosition);
			}
		}	
	}
	
	
	//============================================================================================================================================//
	public void InitializeTouchTrackerWithPosition(Vector3 touchPosition) 
	{
		ShowArrowTracker();
		UpdateTouchTrackerWithPosition(new Vector3(touchPosition.x,touchPosition.y,0));
	}
	
	//============================================================================================================================================//
	public void UpdateTouchTrackerWithPosition(Vector3 touchPosition)
	{
		CurrentTracker.transform.position = touchPosition;
	}
	
	//============================================================================================================================================//
	public void ShowTouchTracker() 
	{
		if (!HasCurveReachedTarget) 
        {		
			Vector3 currentTrackerPosition = new Vector3(CurrentTracker.transform.position.x,CurrentTracker.transform.position.y,-1);
			CurrentTracker = TouchTracker;
			ArrowTracker.transform.position = new Vector3 (ArrowTracker.transform.position.x,ArrowTracker.transform.position.y,-200);
			CurrentTracker.transform.position = currentTrackerPosition;
		}
	}
	
	//============================================================================================================================================//
	public void ShowArrowTracker() 
	{
		CurrentTracker = ArrowTracker;
		TouchTracker.transform.position = new Vector3 (ArrowTracker.transform.position.x,ArrowTracker.transform.position.y,-200);
		
		Vector3 arrowPosition = new Vector3(0,0,-200);
		if (Tail.Count > 3) 
		{
			Vector3 lastPosition = Tail[Tail.Count-3];
			arrowPosition = new Vector3 (lastPosition.x,lastPosition.y,lastPosition.z);
			
			//set up arrow orientation
			float angle = Mathf.Atan(lastPosition.y / lastPosition.x);
			if (lastPosition.y * lastPosition.x < 0) {
				angle += 180;
			}
			CurrentTracker.transform.rotation = Quaternion.AngleAxis(angle,CurrentTracker.transform.position);
			
		}
		CurrentTracker.transform.position = arrowPosition;
	}	
	
	//============================================================================================================================================//
	public bool IsAnotherCurveBeingDrawnExists () 
    {
		GameObject curveManager = GameObject.FindGameObjectWithTag("CurveManager");
		CurveColliderDetector curveColliderDetector = curveManager.GetComponent<CurveColliderDetector> ();
		int res = curveColliderDetector.GetNumberOfCurvesBeingDrawn();
		if (res <= 0) 
		{
			return false;
		}
		else 
		{
			return true;
		}
	}	
	
	//============================================================================================================================================//
	public bool HasFoundAnotherCurveDrawing() 
    {
		GameObject curveManager = GameObject.FindGameObjectWithTag("CurveManager");
		CurveColliderDetector curveColliderDetector = curveManager.GetComponent<CurveColliderDetector> ();
		int res = curveColliderDetector.GetNumberOfCurvesBeingDrawn();
		if (res <= 1) 
		{
			return false;
		}
		else 
		{
			return true;
		}
	}
	
	//============================================================================================================================================//
    Vector3[] GetLoop()
    {
        List<Vector3> loop = new List<Vector3>();
        int start = 0;
        int end = 0;

        for (int i = 0; i < Tail.Count; i++)
        {
            for (int c = i + MinLoopLength; c < Tail.Count; c++)
            {
                // Found Intersection //
                if (Vector3.Distance(Tail[i], Tail[c] ) < MinDistance)
                {
                    start = i;
                    end = c;

                    Vector3 lastPos = Vector3.zero;
                    for (int x = start; x <= end; x++)
                    {
                        loop.Add(Tail[x]);
                        //Debug.DrawLine(lastPos, curve.Tail[x], Color.black);
                        lastPos = Tail[x];
                    }
                    break;
                }
            }
        }

        return loop.ToArray();
    }

    //============================================================================================================================================//
    void OnDestroy()
    {
        // Remove FX //
        Destroy(FXDrawObject);
        Game.Log("Destroyed FX");

        // Remove Mesh //
        Destroy(CurveMesh);
    }	
	
	//============================================================================================================================================//
	public List<Vector3> GetPointsToAddIfTouchPositionMatchDistanceRequirement(Vector3 touchPosition) 
	{		
		List<Vector3> res = new List<Vector3> ();
		
		var worldTouchPosition = touchPosition;
		Vector3 worldTouchPosition2D = new Vector3(worldTouchPosition.x,worldTouchPosition.y,0);
		
		Vector3 lastPoint;
		if (Tail.Count > 1) 
		{
			lastPoint = Tail[Tail.Count - 1];	
		}
		else
		{
			lastPoint = new Vector3(BaseStart.transform.position.x,BaseStart.transform.position.y,0);
		}
		
		/*
        SmoothPosition =  Vector3.Lerp(worldTouchPosition, SmoothPosition, SmoothAmount);
		Vector3 smoothedVector = new Vector3(SmoothPosition.x, SmoothPosition.y, 0);*/
		
		float distance = Vector3.Distance(worldTouchPosition2D,lastPoint);
		int intermediatePointsToAdd = (int) (distance / Stepper );
		
		if (distance > Stepper) 
		{
			Vector3 differenceVector = worldTouchPosition2D - lastPoint;
			differenceVector.Normalize();
			
			for (int i=1;i<=intermediatePointsToAdd;i++)
			{
					Vector3 intermediateVector = differenceVector * Stepper * i + lastPoint;;
					res.Add(intermediateVector);
			}
		}
		return res;
	}
	
	//============================================================================================================================================//
	void curveDidConnectWithMatchingBase(ColorBase colorBase) 
	{
		//put here reactions to this event//
		HasCurveReachedTarget = true;
		
		//add missing points to connect to the base center
		Vector3 baseScreenPoint = Camera.main.WorldToScreenPoint(colorBase.transform.position);
		List<Vector3> pointsToAdd = GetPointsToAddIfTouchPositionMatchDistanceRequirement(baseScreenPoint);
		foreach (Vector3 point in pointsToAdd) 
		{
			AddScreenPointToTail(point);
		}
		
		//remove the trackers
		ArrowTracker.transform.position = new Vector3(CurrentTracker.transform.position.x,CurrentTracker.transform.position.y,-200);
		TouchTracker.transform.position = new Vector3(CurrentTracker.transform.position.x,CurrentTracker.transform.position.y,-200);

        // Play FX //
        Vector3 pos = colorBase.transform.position;
        pos.z = -1;
        
        Game.Spawn(FXComplete, pos, Quaternion.identity);

        Vector3 posStart = BaseStart.transform.position;
        posStart.z = -1;

        Game.Spawn(FXComplete, posStart, Quaternion.identity);        
	}
	
	//============================================================================================================================================//
	void curveDidConnectWithWrongBase(ColorBase colorBase) 
	{
		//Game.Log("Did connect with wrong base");
		Tail = new List<Vector3>();
		
		if (colorBase != BaseStart) {
			KillCurve();
		}
	}
	
    //============================================================================================================================================//
    public void SetColor(int color)
    {
        ColorIndex = color;
        renderer.material = ColorMaterials[ColorIndex];
    }
	
	//============================================================================================================================================//
	public void SetTouchTracker(int trackerIndex) 
	{
		tk2dSprite sprite = TouchTracker.GetComponent<tk2dSprite>();
        sprite.spriteId = trackerIndex;
	}
	
	//============================================================================================================================================//
	public void SetArrowTracker(int trackerIndex) 
	{
		tk2dSprite sprite = ArrowTracker.GetComponent<tk2dSprite>();
        //sprite.spriteId = trackerIndex;
		//ArrowTracker.transform.localScale += new Vector3(-0.5f, -0.5f, 0);
		
		//temp code
		sprite.spriteId = 14;
		ArrowTracker.transform.localScale += new Vector3(+0.5f, +0.5f, 0);
	}	
	
	//============================================================================================================================================//
	void StopDrawingIfLastScreenPointEncouterBaseOrSelf(Vector3 mousePosition) 
	{
		//code to manage colorBase encounter//
		foreach (ColorBase colorBase in BasesExpected)
		{
			//Game.Log("Check if collide with base named: " + colorBase.baseName);
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
		
         // Fast Loop Detection //
        float distance = 0;
        for (int i = 0; i < Tail.Count - MinLoopLength; i++)
        {
            distance = Vector3.Distance(Tail[i], Tail[Tail.Count - 1]);
            if (distance < MinDistance)
            {
                CurveDidEncounterSelfAtPosition(Tail[i]);
            }
        }
	}
	
	//============================================================================================================================================//
	void CurveDidEncounterSelfAtPosition(Vector3 intersectionPoint)
	{
		RemoveAllItemsFromTailAfterPoint(intersectionPoint);
		//KillCurve();
	}
	
	
	//============================================================================================================================================//
	void KillCurve() 
	{
		GameObject curveManager = GameObject.FindGameObjectWithTag("CurveManager");
		CurveColliderDetector colliderDetector = curveManager.GetComponent<CurveColliderDetector>();
		colliderDetector.RemoveCurveFromMonitoring(this);
        Destroy(FXDrawObject);
		Destroy(gameObject);
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
	public void InitializeCurveToResumeDrawingAtPosition(Vector3 position)
	{
        if (Tail.Count == 0)
            SmoothPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else
            SmoothPosition = Tail[Tail.Count - 1];
       
		IsCurveBeingDrawn = true;
		HasCurveReachedTarget = false;
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
	public void RemoveAllItemsFromTailAfterPoint(Vector3 position) 
	{
		int indexPosition = Tail.IndexOf(position);
		//Game.Log("Index Position for vector " + position + "is " + indexPosition);

        // Emit Cut Particles //
        ParticleSystem FXCutObject = (ParticleSystem)Game.Spawn(FXCut, transform.position, Quaternion.identity);
        FXCutObject.startColor = fxColors[ColorIndex];
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[(Tail.Count - indexPosition) * 10];
        for (int i = indexPosition; i < Tail.Count; i++)
        {
            for (int c = 0; c < 10; c++)
            {
                particles[(i - indexPosition) * c] = new ParticleSystem.Particle();
                particles[(i - indexPosition) * c].position = Tail[i];
                particles[(i - indexPosition) * c].size = FXCutObject.startSize;
                particles[(i - indexPosition) * c].lifetime = FXCutObject.startLifetime * ((float)(i - indexPosition) / (Tail.Count - indexPosition));
                particles[(i - indexPosition) * c].color = FXCutObject.startColor;
                particles[(i - indexPosition) * c].velocity = FXCutObject.startSpeed * (new Vector3(Random.value, Random.value, 0) - Vector3.one * 0.5f);
            }
            
        }
        FXCutObject.SetParticles(particles, particles.Length);
        FXCutObject.Play();

		Tail.RemoveRange (indexPosition,Tail.Count - indexPosition);
        TailWidth.RemoveRange(indexPosition, Tail.Count - indexPosition);
		UpdateCurveLength();
		
		UpdateTouchTrackerWithPosition(position);
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
			
			//Game.Log("Closest point found is point with X: " + closestCurvePoint.x + " Y: " + closestCurvePoint.y + " Z: " + closestCurvePoint.z);
			//Game.Log("World touch position has X " + touchPosition2D.x + "Y: " + touchPosition2D.y + " Z: " + touchPosition2D.z);
			
			float distance = Vector3.Distance(touchPosition2D,closestCurvePoint);
			//Game.Log("Distance between closest and touch is: " + distance + " and width of curve is: " + Width);
			
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
	public void UpdateCurveLength() 
	{
		CurveLength = 0;
		
		foreach (Vector3 point in Tail) 
		{
			CurveLength = CurveLength + point.magnitude;
		}
	}
	
	
	//============================================================================================================================================//
    public void AddScreenPointToTail(Vector3 touchPosition)
    {
		
		//temporary disabled it to handle intermediate points
		//var worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
		
		
		//smoothing the touch Position//	
		/*
        SmoothPosition =  Vector3.Lerp(worldTouchPosition, SmoothPosition, SmoothAmount);
		Vector3 smoothedVector = new Vector3(SmoothPosition.x, SmoothPosition.y, 0);
		Tail.Add(smoothedVector);*/
		
		//Tail.Add(new Vector3(worldTouchPosition.x,worldTouchPosition.y,0));
		Tail.Add (new Vector3(touchPosition.x,touchPosition.y,0));
		UpdateCurveLength();

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

        // Emit Draw Particles //
        FXDrawObject.transform.position = Tail[Tail.Count - 1];
        FXDrawObject.startColor = fxColors[ColorIndex];
        FXDrawObject.Emit(5);
	}
	
	//============================================================================================================================================//
    Vector3 SmoothAverage(int startIndex, int count)
    {
        Vector3 result = Vector3.zero;
        int actualCount = 0;

        for (int i = startIndex; i > startIndex - count; i--)
        {
            if (i >= 0)
	        {
		        result += Tail[i];
                actualCount++;
	        }
            else
                break;
        }

        result = result / actualCount;

        return result;
    }
    
    //============================================================================================================================================//
	float UVScroll = 0;
    Vector3 LastLeft;
    Vector3 LastRight;
    Vector3 LastLeftIntersection;
    Vector3 LastRightIntersection;

    void BuildMesh()
    {
        if (Tail.Count > 1)
        {
            // Then create triangles //
            Vector3[] vertices = new Vector3[Tail.Count * 2];
            Vector2[] uv = new Vector2[Tail.Count * 2];
            int[] triangles = new int[(Tail.Count - 1) * 6];

            // Smooth Curve //
            Vector3[] smoothTail = new Vector3[Tail.Count];
            for (int i = 0; i < Tail.Count; i++)
            {
                smoothTail[i] = SmoothAverage(i, SmoothCount);
                //smoothTail[i] = Tail[i];
            }

            // Generate Vertices //
            Vector3 prevVector = Vector3.zero;
            for (int i = 0; i < Tail.Count; i++)
            {
                // Generate the vertex positions //
                Vector3 vector;
                if (i == 0)
                {
                    vector = smoothTail[i] - smoothTail[i + 1];
                }
                else if (i == Tail.Count - 1)
                {
                    vector = smoothTail[i - 1] - smoothTail[i];
                }
                else
                {
                    vector = smoothTail[i - 1] - smoothTail[i + 1];
                }

                vector.Normalize();
                if (prevVector == Vector3.zero)
	                prevVector = vector;

                Vector3 left = new Vector3(vector.y * -1, vector.x, 0);
                Vector3 right = new Vector3(vector.y, vector.x * -1, 0);

                // from 0 to 1 along the length of the tail //
                float v = 1 - ((float)i / (Tail.Count - 1));
                float tailwidth = Mathf.Lerp(TailWidthStart, TailWidthEnd, v);
                //vertices[i * 2] = Tail[i] + left * tailwidth;
                //vertices[i * 2 + 1] = Tail[i] + right * tailwidth;

                // Shorten inner width when big angle //
                /*float angle = Mathf.Atan2(prevVector.y, prevVector.x);
                float x = Mathf.Cos(angle) * vector.x + Mathf.Sin(angle) * vector.y;
                float y = -Mathf.Sin(angle) * vector.x + Mathf.Cos(angle) * vector.y;
                y *= CornerShift;

                float shortener =  (Vector3.Dot(vector, prevVector) + 1) * 0.5f;
                shortener = Mathf.Pow(shortener, 1) * 0.4f;
                prevVector = vector;*/


                /*===================================================
                Vector3 bufferLeft, BufferRight;
                if (LastLeftIntersection != Vector3.zero)
                {
                    bufferLeft = LastLeftIntersection;
                    GetIntersectionPoint(smoothTail, i, left, LastLeft);
                    //LastLeftIntersection = (LastLeftIntersection + GetIntersectionPoint(smoothTail, i, left, LastLeft)) * 0.5f;  
                }
                else
                {
                    bufferLeft = GetIntersectionPoint(smoothTail, i, left, LastLeft);
                }

                if (intersection) LastLeftIntersection = bufferLeft;
                else LastLeftIntersection = Vector3.zero;

                if (LastRightIntersection != Vector3.zero)
                {
                    BufferRight = (LastRightIntersection + GetIntersectionPoint(smoothTail, i, right, LastRight)) * 0.5f;
                }
                else
                {
                    BufferRight = GetIntersectionPoint(smoothTail, i, right, LastRight);
                }

                if (intersection) LastRightIntersection = BufferRight;
                else LastRightIntersection = Vector3.zero;                
                
                vertices[i * 2] = bufferLeft;
                vertices[i * 2 + 1] = BufferRight;
                 =====================================================*/

                vertices[i * 2] = smoothTail[i] + left * TailWidth[i];
                vertices[i * 2 + 1] = smoothTail[i] + right * TailWidth[i];


                LastLeft = left;
                LastRight = right;

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
			MeshFilter m = GetComponent<MeshFilter>();
			
            m.mesh.Clear();
            m.mesh.vertices = vertices;
            m.mesh.uv = uv;
            m.mesh.triangles = triangles;
        }
    }

    //============================================================================================================================================//
    Vector3 GetIntersectionPoint(Vector3[] smoothTail, int i, Vector3 side, Vector3 lastSide)
    {
        // Testing Intersections to fix corners //              
        if (i > 0)
        {
            Vector3 v1 = smoothTail[i];
            Vector3 v2 = smoothTail[i] + side * TailWidth[i];
            Vector3 V3 = smoothTail[i - 1];
            Vector3 V4 = smoothTail[i - 1] + lastSide * TailWidth[i - 1];

            //Debug.DrawLine(v1, v2, Color.red);
            //Debug.DrawLine(V3, V4, Color.red);

            Vector2 p1 = new Vector2(v1.x, v1.y);
            Vector2 p2 = new Vector2(v2.x, v2.y);
            Vector2 p3 = new Vector2(V3.x, V3.y);
            Vector2 p4 = new Vector2(V4.x, V4.y);

            Intersect(p1, p2, p3, p4);
        }
        else
            return smoothTail[i] + side * TailWidth[i];

        float distance = Vector3.Distance(smoothTail[i], intersectionPoint);
        if (distance < TailWidth[i] && intersection)
        {
            Debug.DrawLine(smoothTail[i], new Vector3(intersectionPoint.x, intersectionPoint.y, 0), Color.red);
            return intersectionPoint;
        }
        else
            return smoothTail[i] + side * TailWidth[i];
       
    }

    //============================================================================================================================================//
    bool intersection = false;
    bool coincident = false;
    Vector2 intersectionPoint;
    void Intersect(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
    {
        float ua = (point4.x- point3.x) * (point1.y - point3.y) - (point4.y - point3.y) * (point1.x - point3.x);
        float ub = (point2.x - point1.x) * (point1.y - point3.y) - (point2.y - point1.y) * (point1.x - point3.x);
        float denominator = (point4.y - point3.y) * (point2.x - point1.x) - (point4.x - point3.x) * (point2.y - point1.y);

        intersection = coincident = false;

        if (Mathf.Abs(denominator) <= 0.00001f)
        {
            if (Mathf.Abs(ua) <= 0.00001f && Mathf.Abs(ub) <= 0.00001f)
            {
                intersection = coincident = true;
                intersectionPoint = (point1 + point2) / 2;
            }
        }
        else
        {
            ua /= denominator;
            ub /= denominator;

            /*if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
            {
                intersection = true;
                intersectionPoint.x = point1.x + ua * (point2.x - point1.x);
                intersectionPoint.y = point1.y + ua * (point2.y - point1.y);
            }*/
            if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
            {
                intersection = true;
                intersectionPoint.x = point1.x + ua * (point2.x - point1.x);
                intersectionPoint.y = point1.y + ua * (point2.y - point1.y);
            }
        }
    }
}
