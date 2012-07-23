using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBase : MonoBehaviour {
	
	public List<ColorBase> colorBasePeers;
	public string baseName;
		
	GameObject Curve;	
	GameObject ExpectedCurve;
	
	//============================================================================================================================================//
	void Update () 
	{
		bool hasTouchStarted = (Input.GetMouseButtonDown(0));
		
		if (hasTouchStarted) 
		{
			if (HasTouchedMe(Input.mousePosition)) 
			{
				if (! ExpectedCurve) 
				{
					if (Curve)
					{
						KillCurve(Curve);
						//Destroy(Curve);	
					}
				}else {
					Debug.Log("Touch peer base that was expecting a curve");
					KillCurve(ExpectedCurve);
				}	
				InstantiateBaseAwareCurve();
				InformPeersToExpectCurve(Curve);
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
	void InstantiateBaseAwareCurve()
	{
		Curve = (GameObject) Instantiate(Resources.Load("ColorCurve"));
		ColorString stringScript = Curve.GetComponent<ColorString> ();
		
		//set up original base and peeers base//
		ColorBase currentBase = GetComponent<ColorBase> ();
		stringScript.BaseStart = currentBase;
		//stringScript.BasesExpected.Add (currentBase); 
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
		Destroy(curveToKill);
	}
	
	//============================================================================================================================================//
	bool HasTouchedMe(Vector3 touchPosition)
    {
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x,touchPosition.y,0));	
		
		RaycastHit hit;
		
		bool res = false;
		if (gameObject.collider.Raycast(ray,out hit,50.0f)) 
        {
			res =  true;
		}
		Debug.DrawLine (ray.origin, hit.point);
        return res;
		
	}
}
