using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	
	float PlayTime;
	float StartTime;
	string PlayTimeLabel;
	bool  HasLevelBeenCompleted = false;
	
	//============================================================================================================================================//
	void Update()
	{
		if (PairOfCurvesToConnect() == 0 && ! HasLevelBeenCompleted) {
			LevelCompleted();
		}else{
			if (! HasLevelBeenCompleted) 
			{
				UpdateStringTime ();	
			}
		}
	}
	
	//============================================================================================================================================//
	void LevelCompleted()
	{
		HasLevelBeenCompleted = true;
	}
	
	
	//============================================================================================================================================//
	void Awake() 
	{
   		StartTime = Time.time;
	}
	
	//============================================================================================================================================//
	void UpdateStringTime() 
	{
		PlayTime = Time.time - StartTime;

  		int minutes = (int) PlayTime / 60;
   		int seconds = (int) PlayTime % 60;
   		int fraction = (int) (PlayTime * 100) % 100;

   		PlayTimeLabel =  minutes + "min " + seconds + "sec " +  fraction + "m"; 
	}
	
	
	//============================================================================================================================================//
	int PairOfCurvesToConnect() 
	{
		ColorBase [] bases = GameObject.FindObjectsOfType(typeof(ColorBase)) as ColorBase[];
		int total = 0;
		
		for (int i=0; i<bases.Length;i++) 
		{
			bool shouldCountTheBase = true;
			
			GameObject curve = bases[i].Curve;
			if (curve) 
			{	
				ColorString baseColorString = curve.GetComponent<ColorString>();	
				 if ( baseColorString.HasCurveReachedTarget ) 
				{
					shouldCountTheBase = false;
				}
			}else{
				GameObject expectedCurve = bases[i].ExpectedCurve;
				if (expectedCurve) {
					ColorString expectedColorString = expectedCurve.GetComponent<ColorString>();	
					if (expectedColorString.HasCurveReachedTarget) 
					{
						shouldCountTheBase = false;
					}	
				}
			} 
			
			if (shouldCountTheBase) 
			{
				total ++;
			}
		}
		return (int) total/2;
	}
	
	//============================================================================================================================================//
	void OnGUI() {
		
		ColorString[] curves = GameObject.FindObjectsOfType(typeof(ColorString)) as ColorString[];
		for (int i=0; i<curves.Length;i++) 
		{
			GUI.Label(new Rect(0, i * 40, 100, 100), "Curve Length is: " + curves[i].CurveLength);			
		}
		
		GUI.Label(new Rect(250, 0, 100, 100), "Play Time is: " + PlayTimeLabel);
		GUI.Label(new Rect(250, 40, 100, 100), "Pair to connect are: " + PairOfCurvesToConnect());
		
		if (HasLevelBeenCompleted) 
		{
			GUI.Label(new Rect(250, 80, 100, 100), "Level Finished");
		}
    }
}
