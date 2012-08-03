using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBar : MonoBehaviour {

	public Game currentGame;
	//public UISlicedSprite[] colorSprites;
	public List<UISlicedSprite> colorSpritesList;
	
	
	public float BarDistance;
	public float BarDistanceStep; 
	float DefaultBarDistance;
	
	public float LastDistance = 45;
	
	public UILabel DistanceLabel;
	public UILabel CurrentDistanceLabel;
	public UILabel LastDistanceLabel;
	
	//public float MaximumPointsAllowed = 800;
	public float MaximumScreenSized = 1536;
	
	
	public List<UISlicedSprite> SpritesBeingDisplayed = new List<UISlicedSprite>();
	
	
	const int AQUA_SPRITE_INDEX = 0;
	const int ORANGE_SPRITE_INDEX = 1;
	const int PINK_SPRITE_INDEX = 2;
	const int PURPLE_SPRITE_INDEX = 3;
	const int TEAL_SPRITE_INDEX = 4;
	const int YELLOW_SPRITE_INDEX = 5;
	
	
	Dictionary<int, int> ColorBarSpriteIndexes = new Dictionary<int, int>() { 
		{(int)ColorBase.CurveColorMaterial.Aqua, (int) AQUA_SPRITE_INDEX},
		{(int)ColorBase.CurveColorMaterial.Orange, (int) ORANGE_SPRITE_INDEX},
		{(int)ColorBase.CurveColorMaterial.Pink, (int) PINK_SPRITE_INDEX},
		{(int)ColorBase.CurveColorMaterial.Purple, (int) PURPLE_SPRITE_INDEX},
		{(int)ColorBase.CurveColorMaterial.Teal, (int) TEAL_SPRITE_INDEX},
		{(int)ColorBase.CurveColorMaterial.Yellow, (int) YELLOW_SPRITE_INDEX}
	};
	
	
	Dictionary <int,Vector3> OriginalScalings = new Dictionary<int, Vector3> () ;
	
	Dictionary<ColorString,float> LastRecordedLength = new Dictionary<ColorString, float>();
	
	
	//============================================================================================================================================//
    void Awake() 
	{
		SetUpOriginalScale();
		DefaultBarDistance = BarDistance;
		ResetColorBar();
		UpdateLabelMaxDistance(BarDistance); 
		UpdateLabelLastDistance();
	}
	
	//============================================================================================================================================//
	void Update() 
	{	
		UpdateSpriteListFromCurvesDetected();
		UpdateScaleAndPositionsOfSpritesBeingDisplayed();
	}
	
	//============================================================================================================================================//
	public void ResetColorBar() 
	{
		BarDistance = DefaultBarDistance;
		UpdateLabelMaxDistance(BarDistance); 
		UpdateLabelLastDistance();
		UpdateLabelCurrentDistance(0,CurrentDistanceLabel.transform.localPosition);
		
		foreach (UISlicedSprite sprite in colorSpritesList) 
		{
			sprite.transform.localPosition = new Vector3(sprite.transform.localPosition.x,sprite.transform.localPosition.y,-1);
			sprite.transform.localScale = new Vector3(0,0,0);
		}
		
		SpritesBeingDisplayed = new List<UISlicedSprite>();
	}

	//============================================================================================================================================//
	void SetUpOriginalScale() 
	{
		OriginalScalings[AQUA_SPRITE_INDEX] 	= colorSpritesList[AQUA_SPRITE_INDEX].transform.localScale;
		OriginalScalings[ORANGE_SPRITE_INDEX] 	= colorSpritesList[ORANGE_SPRITE_INDEX].transform.localScale;
		OriginalScalings[PINK_SPRITE_INDEX] 	= colorSpritesList[PINK_SPRITE_INDEX].transform.localScale;
		OriginalScalings[PURPLE_SPRITE_INDEX] 	= colorSpritesList[PURPLE_SPRITE_INDEX].transform.localScale;
		OriginalScalings[TEAL_SPRITE_INDEX] 	= colorSpritesList[TEAL_SPRITE_INDEX].transform.localScale;
		OriginalScalings[YELLOW_SPRITE_INDEX] 	= colorSpritesList[YELLOW_SPRITE_INDEX].transform.localScale;
	}
	
	//============================================================================================================================================//
	void UpdateLabelCurrentDistance(float totalCurveDistanceMeter,Vector3 newPosition) 
	{
		CurrentDistanceLabel.text = "" + totalCurveDistanceMeter.ToString("N1")  + "M";
		CurrentDistanceLabel.transform.localPosition = newPosition;
	}
	
	//============================================================================================================================================//
	void UpdateLabelMaxDistance (float maxDistanceMeter) 
	{
		if (LastDistance > BarDistance) 
		{
			BarDistance = LastDistance + BarDistanceStep;
		}	
		DistanceLabel.text = "" + BarDistance.ToString("N1") + "M";
	}
	
	//============================================================================================================================================//
	void UpdateLabelLastDistance () 
	{
		LastDistanceLabel.text = "" + LastDistance.ToString("N1") + "M";
		float res = LastDistance / BarDistance;
		res = res * MaximumScreenSized;
		
		Vector3 newPosition = new Vector3(res,LastDistanceLabel.transform.localPosition.y,0);
		LastDistanceLabel.transform.localPosition = newPosition;
	}
	
	//============================================================================================================================================//
	ColorString CurveMatchingSprite(UISlicedSprite sprite) 
	{
		
		ColorString[] curves = GameObject.FindObjectsOfType(typeof(ColorString)) as ColorString[];
		
		for (int i=0;i<curves.Length;i++) 
		{
			ColorString c = curves[i];
		
			int indexColor = c.ColorIndex;
			int spriteIndex = ColorBarSpriteIndexes[indexColor];
			UISlicedSprite s =  colorSpritesList[spriteIndex]; //colorSprites[spriteIndex];
			if (s == sprite) 
			{
				return c;
			}
		}
		return null;
	}
	
	
	
	//============================================================================================================================================//
	UISlicedSprite SpriteMatchingCurve(ColorString curve) 
	{
		
		ColorString[] curves = GameObject.FindObjectsOfType(typeof(ColorString)) as ColorString[];
		
		for (int i=0;i<curves.Length;i++) 
		{
			ColorString c = curves[i];
			if (c == curve) 
			{
				int indexColor = curve.ColorIndex;
				int spriteIndex = ColorBarSpriteIndexes[indexColor];
				UISlicedSprite sprite =  colorSpritesList[spriteIndex]; //colorSprites[spriteIndex];
				return sprite;
			}
		}
		return null;
	}
	
	
		//============================================================================================================================================//
	void UpdateSpriteListFromCurvesDetected() 
	{
		ColorString[] curves = GameObject.FindObjectsOfType(typeof(ColorString)) as ColorString[];
		
		//SpritesBeingDisplayed = new List<UISlicedSprite>();
		
		for (int i=0;i<curves.Length;i++) 
		{
			ColorString curve = curves[i];
			
			int indexColor = curve.ColorIndex;
			int spriteIndex = ColorBarSpriteIndexes[indexColor];
			
			UISlicedSprite sprite =  colorSpritesList[spriteIndex]; //colorSprites[spriteIndex];
			
			if (! SpritesBeingDisplayed.Contains(sprite)) 
			{
				SpritesBeingDisplayed.Add(sprite);	
			}
			
		}
		
		
	}
	
		//============================================================================================================================================//
	void UpdateScaleAndPositionsOfSpritesBeingDisplayed() 
	{
		
		ScaleAllSprites();
		
		foreach (UISlicedSprite sprite in SpritesBeingDisplayed) 
		{
			SetPositionForSprite(sprite);
		}
		
	}
	
	
	//============================================================================================================================================//
	
	float CalculatePercentageForSprite(UISlicedSprite sprite, ColorString curve) 
	{
		float res = 0.0f;
		ColorString[] curves = GameObject.FindObjectsOfType(typeof(ColorString)) as ColorString[];
		
		float totalMeters = 0.0f;
		for (int i=0;i<curves.Length;i++)
		{
			totalMeters += curves[i].GetCurveDistanceInMeters();	
		}
		
		//computation of ratio of the percentBarOfAllCurves. If greater than 100% resize the bar ratio
		float percentBarOfAllCurves = totalMeters / BarDistance;
		while (percentBarOfAllCurves >= 1) 
		{	
			BarDistance = BarDistance + BarDistanceStep;
			UpdateLabelMaxDistance(BarDistance);
			UpdateLabelLastDistance();
			percentBarOfAllCurves = percentBarOfAllCurves - 1;
		}
		
		//calculation of the percentage of the current segment
		res = curve.GetCurveDistanceInMeters() / BarDistance;
		res = res * MaximumScreenSized;
		return res;
	}
	

	
	//============================================================================================================================================//
	void ScaleSpriteWithPercentage(UISlicedSprite colorSprite, ColorString curve, float percentage) 
	{
		int spriteIndex = colorSpritesList.IndexOf(colorSprite);
		if (HasCurveLengthHasChangeSinceLastScale(curve)) {
			colorSprite.transform.localScale = OriginalScalings[spriteIndex] + new Vector3(percentage, 0, 0);
		}
	}
	
	
	bool HasCurveLengthHasChangeSinceLastScale(ColorString curve) 
	{
		if (LastRecordedLength.ContainsKey(curve)) 
		{
			float oldLength = LastRecordedLength[curve];
			return (oldLength != curve.CurveLength);
		}
		else 
		{
			LastRecordedLength[curve] = curve.CurveLength;
			return true;
		}	
	}
	
	
	
	//============================================================================================================================================//
	UISlicedSprite FindLastSegmentBeingDisplayed() 
	{
		
		if (SpritesBeingDisplayed.Count >=1)
		{
			UISlicedSprite res = SpritesBeingDisplayed[0];
			for(int i =1;i<  SpritesBeingDisplayed.Count;i++)
			{
				UISlicedSprite sprite = SpritesBeingDisplayed[i];
				bool hasFoundSprite = sprite.transform.localPosition.x > res.transform.localPosition.x;
				if (hasFoundSprite) 
				{
					res = sprite;
				}
				
			}
			return res;
		}
		else
		{
			return null;	
		}
	}
	
	
	//============================================================================================================================================//
	void ScaleAllSprites() 
	{
		foreach (UISlicedSprite sprite in SpritesBeingDisplayed) 
		{
			ColorString curve = CurveMatchingSprite(sprite);
			
			if (curve) {
				float percent = CalculatePercentageForSprite(sprite,curve);
				ScaleSpriteWithPercentage(sprite,curve,percent);	
			}
		}
		
		if (SpritesBeingDisplayed.Count > 0) 
		{
			ColorString[] curves = GameObject.FindObjectsOfType(typeof(ColorString)) as ColorString[];
		
			float totalMeters = 0.0f;
			for (int i=0;i<curves.Length;i++)
			{
				totalMeters += curves[i].GetCurveDistanceInMeters();	
			}
			
			UISlicedSprite lastSegment =   FindLastSegmentBeingDisplayed();
			Vector3 markerPosition = new Vector3 (lastSegment.transform.localPosition.x + lastSegment.transform.localScale.x,CurrentDistanceLabel.transform.localPosition.y ,0);
			UpdateLabelCurrentDistance(totalMeters,markerPosition);
		}
	}
	
	//============================================================================================================================================//
	
	void SetPositionForSprite(UISlicedSprite sprite) 
	{	
		float accumulator = 0.0f;	
		int indexSprite = SpritesBeingDisplayed.IndexOf(sprite);
		
			for(int i=0;i<=indexSprite;i++) 
			{
				UISlicedSprite colorSprite = SpritesBeingDisplayed[i];
				
				if (colorSprite != sprite) {
					accumulator += colorSprite.transform.localScale.x;
				}
			}
		sprite.transform.localPosition =  new Vector3(OriginalScalings[indexSprite].x  + accumulator, sprite.transform.localPosition.y,0);
	}
	
}
