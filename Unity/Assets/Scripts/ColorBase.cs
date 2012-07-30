using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBase : MonoBehaviour 
{
	
	public List<ColorBase> colorBasePeers;
	public string baseName;
		
	public GameObject Curve;	
	public GameObject ExpectedCurve;
    public Game Game;
	
	//============================================================================================================================================//
    void Awake()
    {
        Game = (Game)GameObject.FindObjectOfType(typeof(Game));
    }
    
    //============================================================================================================================================//
	void Update () 
	{
		
	  if (Input.touches.Length<=1) 
      { 
		
		bool hasTouchStarted = (Input.GetMouseButtonDown(0));
		bool isTouchUpdated = Input.GetMouseButton(0);
		
		if (hasTouchStarted) 
		{
			if (HasTouchedMe(Input.mousePosition)) 
			{
				if (! ExpectedCurve) 
				{
					if (Curve)
					{
						KillCurve(Curve);
					}
				}
				else 
				{		
					KillCurve(ExpectedCurve);
				}	
				
				InstantiateBaseAwareCurve(Input.mousePosition);
				InformPeersToExpectCurve(Curve);
			}
		}
		else if (isTouchUpdated) 
		{
			if (!Curve) 
			{
				if (HasTouchedMe(Input.mousePosition)) 
				{
					if (! ExpectedCurve) 
					{	
						InstantiateBaseAwareCurve(Input.mousePosition);
						InformPeersToExpectCurve(Curve);	
					}
					else 
					{
						ColorString colorString = ExpectedCurve.GetComponent<ColorString>();
						if (! colorString.IsCurveBeingDrawn) {
							KillCurve(ExpectedCurve);
							InstantiateBaseAwareCurve(Input.mousePosition);
							InformPeersToExpectCurve(Curve);	
						}else {
							Debug.Log("");		
						}
					}
				}
			}
			else 
			{
				if (HasTouchedMe(Input.mousePosition)) 
				{
					/*KillCurve(Curve);
					InstantiateBaseAwareCurve(Input.mousePosition);
					InformPeersToExpectCurve(Curve);*/
				}	
			}
				
		}
		else
		{
			
			if (Curve) 
			{
				ColorString colorString = Curve.GetComponent<ColorString>();
				if (colorString.Tail.Count == 0) 
				{
					KillCurve(Curve);	
				}
			}
		}
			
		}
		
	}
	
	//============================================================================================================================================//
	public bool IsCollidingWithPoint(Vector3 point) 
	{
		//Debug.Log("Point position X :" + point.x + " Y:" + point.y + "Z:" + point.z);
		bool res = false;
		if (gameObject.collider.bounds.Contains(point)){
			res = true;
			//Debug.Log("The damn base box " + baseName + " contains this point" + point.x);
		}else{
			//Debug.Log("The damn base box " + baseName + " does not contains this point" + point.x);
		}
		return res;
	}
	
	//============================================================================================================================================//
    enum CurveColorMaterial : int {Teal=0, Orange=1, Purple=2, Aqua=3, Yellow=4, Pink=5};
	enum BaseColorSpriteID : int {Teal=1,Aqua=11,Orange=2,Pink=7,Purple=4,Yellow=8};
	
	enum CurveEndColorSpriteID : int {Teal=20,Aqua=24,Orange=19,Pink=22,Purple=23,Yellow=21};
	enum CurveArrowColorSpriteID : int {Teal=20,Aqua=24,Orange=19,Pink=22,Purple=23,Yellow=21};
	
	Dictionary<int, int> CurveColorIndexes = new Dictionary<int, int>() { 
		{(int)BaseColorSpriteID.Teal, (int)CurveColorMaterial.Teal}, 
		{(int)BaseColorSpriteID.Aqua, (int)CurveColorMaterial.Aqua},
		{(int)BaseColorSpriteID.Orange, (int)CurveColorMaterial.Orange},
		{(int)BaseColorSpriteID.Pink, (int)CurveColorMaterial.Pink},
		{(int)BaseColorSpriteID.Purple, (int)CurveColorMaterial.Purple},
		{(int)BaseColorSpriteID.Yellow, (int)CurveColorMaterial.Yellow}
	};
	
	Dictionary<int, int> CurveTrackerIndexes = new Dictionary<int, int> {
		{(int)BaseColorSpriteID.Teal, (int)CurveEndColorSpriteID.Teal}, 
		{(int)BaseColorSpriteID.Aqua, (int)CurveEndColorSpriteID.Aqua}, 
		{(int)BaseColorSpriteID.Orange, (int)CurveEndColorSpriteID.Orange}, 
		{(int)BaseColorSpriteID.Pink, (int)CurveEndColorSpriteID.Pink}, 
		{(int)BaseColorSpriteID.Purple, (int)CurveEndColorSpriteID.Purple}, 
		{(int)BaseColorSpriteID.Yellow, (int)CurveEndColorSpriteID.Yellow}, 
		
	};
	
	Dictionary<int, int> CurveTrackerArrowIndexes = new Dictionary<int, int> {
		{(int)BaseColorSpriteID.Teal, (int)CurveEndColorSpriteID.Teal}, 
		{(int)BaseColorSpriteID.Aqua, (int)CurveEndColorSpriteID.Aqua}, 
		{(int)BaseColorSpriteID.Orange, (int)CurveEndColorSpriteID.Orange}, 
		{(int)BaseColorSpriteID.Pink, (int)CurveEndColorSpriteID.Pink}, 
		{(int)BaseColorSpriteID.Purple, (int)CurveEndColorSpriteID.Purple}, 
		{(int)BaseColorSpriteID.Yellow, (int)CurveEndColorSpriteID.Yellow}, 
		
	};
	
	
	//Dictionary<int, int> CurveColorIndexes = new Dictionary<int, int>() { { 1, 0 }, { 2, 1 }, { 4, 2 }, { 11, 3 }, { 8, 4 }, { 7, 5 } };
	
	//Dictionary<int, int> CurveTrackerIndexes = new Dictionary<int, int> {};
    public void SetCurveColor(ColorString curve)
    {
        tk2dSprite sprite = GetComponent<tk2dSprite>();
        int index = sprite.spriteId;
        print(index + " : " + CurveColorIndexes[index]);

        curve.SetColor(CurveColorIndexes[index]);
		
		//set the colors of the trackers
		curve.SetTouchTracker(CurveTrackerIndexes[index]);
		curve.SetArrowTracker(CurveTrackerArrowIndexes[index]);
		//curve.SetArrowTracker(
    }
    
    //============================================================================================================================================//
	void InstantiateBaseAwareCurve(Vector3 mousePosition)
	{
        
		//Curve = (GameObject) Instantiate(Resources.Load("ColorCurve"));
        Curve = (GameObject)Game.Spawn(Resources.Load("ColorCurve"));
        ColorString stringScript = Curve.GetComponent<ColorString>();
        SetCurveColor(stringScript);
		
		//set up original base and peeers base//
		ColorBase currentBase = GetComponent<ColorBase> ();
		stringScript.BaseStart = currentBase;
		stringScript.InitializeTouchTrackerWithPosition(currentBase.transform.position);
		
		foreach (ColorBase colorBase in colorBasePeers) 
		{
			stringScript.BasesExpected.Add(colorBase);
		}
		
		//set up bases of other colors//
		ColorBase[] bases = FindObjectsOfType(typeof(ColorBase)) as ColorBase[];
		for (int i = 0;i<bases.Length;i++) 
		{
			ColorBase gameBase = bases[i];
			if (! stringScript.BasesExpected.Contains(gameBase)) 
			{
				stringScript.BasesToAvoid.Add(gameBase);
			}
			
		}
		
		GameObject curveManager = GameObject.FindGameObjectWithTag("CurveManager");
		curveManager.SendMessage ("AddCurveToControl", stringScript);
		
		Debug.Log ("Attention curve with " + stringScript.BasesExpected.Count + " bases to detect");
		Debug.Log ("Attention curve with " + stringScript.BasesToAvoid.Count + " bases to avoid");
        
	}
	
	//============================================================================================================================================//
	void InformPeersToExpectCurve(GameObject curve)
	{
		Debug.Log("Dispatch Peers Expect Curve Information");
		foreach (ColorBase colorBase in colorBasePeers)
		{
			colorBase.ExpectCurve(curve);
		}
	}
	
	//============================================================================================================================================//
	void ExpectCurve(GameObject curveToSet) 
	{
		Debug.Log("Node: " + name + " has received a curve to set");
		ExpectedCurve = curveToSet;
	}
	
	//============================================================================================================================================//
	void KillCurve(GameObject curveToKill)
	{
		GameObject curveManager = GameObject.FindGameObjectWithTag("CurveManager");
		
		
		ColorString colorString = curveToKill.GetComponent<ColorString>();
		
		curveManager.SendMessage ("RemoveCurveFromMonitoring", colorString);
		Destroy(curveToKill);
	}
	
	//============================================================================================================================================//
	bool HasTouchedMe(Vector3 touchPosition)
    {
        bool res = false;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0));
        RaycastHit hit;

        if (gameObject.collider.Raycast(ray, out hit, 50.0f))
        {
            res = true;
        }
        Debug.DrawLine(ray.origin, hit.point);
		
        return res;
		
	}
}
