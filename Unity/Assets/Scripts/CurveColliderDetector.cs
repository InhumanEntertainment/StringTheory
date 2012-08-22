using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurveColliderDetector : MonoBehaviour 
{
    public static CurveColliderDetector Instance;
	public float DistanceMaxAllowed = 0.1f;
	public List<ColorString> Curves = new List<ColorString> ();

    //============================================================================================================================================//
    void Awake()
    {
        Instance = this;
    }
    
    //============================================================================================================================================//
	public void AddCurveToControl (ColorString curve) 
    {
        Curves.Add(curve);
	}
	
	//============================================================================================================================================//
	public void RemoveCurveFromMonitoring (ColorString curve) 
	{
		Curves.Remove(curve);
	}
	
	//============================================================================================================================================//
	public int GetNumberOfCurvesBeingDrawn() 
	{
		int res = 0;

        foreach (ColorString curve in Curves)
		{
            if (curve.IsCurveBeingDrawn) 
            {
				res ++;
			}
		}
		return res;
	}
	
	
	//============================================================================================================================================//
	public void CheckPositionForCurve (ColorString colorString) 
	{	
		int currentIndex = Curves.IndexOf(colorString);
		
		ColorString[] tempList = Curves.ToArray();
		//Debug.Log("Size of the list done by the curve " +tempList.Length);
		
		for (int i=0;i<tempList.Length;i++) 
		{
			//Debug.Log("Inside loop of colorString " + i);
			if (currentIndex != i) 
			{
				Vector3 collisionPoint = GetIntersectionPointIfExistsForCurve(colorString,tempList[i]);
				bool collisionHasBeenDetected = collisionPoint.z == 0;
				if (collisionHasBeenDetected) 
				{
					//Debug.Log("AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
					//Debug.Log("Collision Point Find  IS:" + collisionPoint);
					
					PeformCutOnCurve(tempList[i],collisionPoint);
					break;
				}
			}
		}
		
		if (colorString.Tail.Count > 0) {
			//Debug.Log("Last point of curve that updated is: " + colorString.Tail[colorString.Tail.Count - 1]);
		}
	}
	
	//============================================================================================================================================//
	void PeformCutOnCurve(ColorString curve,Vector3 collisionPoint)
	{	
		curve.RemoveAllItemsFromTailAfterPoint(collisionPoint);
	}
		
	//============================================================================================================================================//
	public Vector3 GetIntersectionPointIfExistsForCurve(ColorString curve1, ColorString curve2) 
	{
		if (curve1.Tail.Count > 0 && curve2.Tail.Count > 0)
		{
			Vector3 lastPointCurve1 = curve1.Tail[curve1.Tail.Count - 1];
			
			foreach (Vector3 point in curve2.Tail) 
			{
				float distance = Vector3.Distance(lastPointCurve1,point);
				//Debug.Log("Distance between the two curve is: " + distance);
				//if (distance < 0.1f) {
				if (distance < DistanceMaxAllowed)
				{
					return point;
				}	
				
			}
		}
		return new Vector3(0,0,-100);
	}	
}
