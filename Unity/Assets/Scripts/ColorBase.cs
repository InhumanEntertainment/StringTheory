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
		
		if (hasTouchStarted) {

			if (HasTouchedMe(Input.mousePosition)) 
			{
				if (!Curve)
				{
					InstantiateBaseAwareCurve();
					//InformPeersToExpectCurve(Curve);
				}
				else
				{
					Destroy(Curve);
					//InformPeersToRemoveExpectedCurve();
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
	void InstantiateBaseAwareCurve()
	{
		Curve = (GameObject) Instantiate(Resources.Load("ColorCurve"));
		ColorString stringScript = Curve.GetComponent<ColorString> ();
		
		//set up original base and peeers base//
		ColorBase currentBase = GetComponent<ColorBase> ();
		stringScript.BasesExpected.Add (currentBase); 
		foreach (ColorBase colorBase in colorBasePeers) 
		{
			stringScript.BasesExpected.Add(colorBase);
		}
		
		//set up bases of other colors//
		ColorBase[] bases = FindObjectsOfType(typeof(ColorBase)) as ColorBase[];
		for (int i = 0;i<bases.Length;i++) 
		{
			ColorBase gameBase = bases[i];
			if (! stringScript.BasesExpected.Contains(gameBase)) {
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
	void InformPeersToRemoveExpectedCurve()
	{
		Debug.Log("Dispatch Inform Peers to Remove");
		foreach (ColorBase colorBase in colorBasePeers)
		{
			//ColorBase colorBaseScript = colorBase.GetComponent<ColorBase>();
			//colorBaseScript.RemoveCurve(Curve);
			
		}
	}
	
	
	//============================================================================================================================================//
	void ExpectCurve(GameObject curveToSet) 
	{
		Debug.Log("Node: " + name + " has received a curve to set");
		ExpectedCurve = curveToSet;
	}
	
	//============================================================================================================================================//
	void RemoveCurve(GameObject curveToSet)
	{
		//object should been destroyed already. nothing to do yet
	}
	
	//============================================================================================================================================//
	bool HasTouchedMe(Vector3 touchPosition)
    {
		
		Debug.Log("Has touched me debug touchPosition X: " + touchPosition.x + " Y: " + touchPosition.y + " Z: " + touchPosition.z);
		
		if (Curve) 
		{
			ColorString stringScript = Curve.GetComponent<ColorString> ();
			if (stringScript.Tail.Count > 1) 
			{
				Vector3 lastPoint = stringScript.Tail[stringScript.Tail.Count-1];
				Debug.Log("Has touched me debug lastPoint X: " + lastPoint.x + " Y: " + lastPoint.y + " Z: " + lastPoint.z);	
			}
		}
		
		
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
