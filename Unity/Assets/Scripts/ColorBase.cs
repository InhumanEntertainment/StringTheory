using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBase : MonoBehaviour 
{	
	public List<ColorBase> colorBasePeers;
	public string baseName;
		
	public ColorString Curve;
    public ColorString ExpectedCurve;
    public Game Game;

    public GameColor Color;
    public ColorString StringPrefab;
	
	//============================================================================================================================================//
    void Awake()
    {
        Game = (Game)GameObject.FindObjectOfType(typeof(Game));
    }
    
    //============================================================================================================================================//
	void Update () 
	{
        if (!Game.Paused && !Game.LevelHasCompleted)
        {
            if (Input.touches.Length <= 1)
            {
                bool hasTouchStarted = (Input.GetMouseButtonDown(0));
                bool isTouchUpdated = Input.GetMouseButton(0);

                if (hasTouchStarted)
                {
                    if (HasTouchedMe(Input.mousePosition))
                    {
                        if (!ExpectedCurve)
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
                            if (!ExpectedCurve)
                            {
                                InstantiateBaseAwareCurve(Input.mousePosition);
                                InformPeersToExpectCurve(Curve);
                            }
                            else
                            {
                                if (!ExpectedCurve.IsCurveBeingDrawn)
                                {
                                    KillCurve(ExpectedCurve);
                                    InstantiateBaseAwareCurve(Input.mousePosition);
                                    InformPeersToExpectCurve(Curve);
                                }
                                else
                                {
                                    //Debug.Log("");
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
                        if (Curve.Tail.Count == 0)
                        {
                            KillCurve(Curve);
                        }
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
	void InstantiateBaseAwareCurve(Vector3 mousePosition)
	{
        Curve = (ColorString)Game.Spawn(StringPrefab);        
        Curve.SetColor(Color);
		
		//set up original base and peeers base//
		Curve.BaseStart = this;
		Curve.InitializeTouchTrackerWithPosition(transform.position);
		
		foreach (ColorBase colorBase in colorBasePeers) 
		{
			Curve.BasesExpected.Add(colorBase);
		}
		
		//set up bases of other colors//
		ColorBase[] bases = FindObjectsOfType(typeof(ColorBase)) as ColorBase[];
		for (int i = 0;i<bases.Length;i++) 
		{
			ColorBase gameBase = bases[i];
			if (! Curve.BasesExpected.Contains(gameBase)) 
			{
				Curve.BasesToAvoid.Add(gameBase);
			}			
		}

        CurveColliderDetector.Instance.AddCurveToControl(Curve);
        //ColorBar.Instance.ResetColorBar();

		//Debug.Log ("Attention curve with " + Curve.BasesExpected.Count + " bases to detect");
		//Debug.Log ("Attention curve with " + Curve.BasesToAvoid.Count + " bases to avoid");       
	}
	
	//============================================================================================================================================//
	void InformPeersToExpectCurve(ColorString curve)
	{
		//Debug.Log("Dispatch Peers Expect Curve Information");
		foreach (ColorBase colorBase in colorBasePeers)
		{
			colorBase.ExpectCurve(curve);
		}
	}
	
	//============================================================================================================================================//
	void ExpectCurve(ColorString curveToSet) 
	{
		//Debug.Log("Node: " + name + " has received a curve to set");
		ExpectedCurve = curveToSet;
	}
	
	//============================================================================================================================================//
	void KillCurve(ColorString curveToKill)
	{
        CurveColliderDetector.Instance.RemoveCurveFromMonitoring(curveToKill);
		Destroy(curveToKill.gameObject);
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
