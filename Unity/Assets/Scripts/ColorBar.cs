using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBar : MonoBehaviour {

	public Game currentGame;
	public UISlicedSprite[] colorSprites;
	public List<UISlicedSprite> colorSpritesList;
	
	public float MaximumPointsAllowed = 800;
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
	/*{
		{(int) AQUA_SPRITE_INDEX,0.0f},
		{(int) ORANGE_SPRITE_INDEX,0.0f},
		{(int) PINK_SPRITE_INDEX,0.0f},
		{(int) PURPLE_SPRITE_INDEX,0.0f},
		{(int) TEAL_SPRITE_INDEX,0.0f},
		{(int) YELLOW_SPRITE_INDEX,0.0f}
	};*/
	
	Dictionary<ColorString,float> LastRecordedLength = new Dictionary<ColorString, float>();
	Dictionary<int,float> OriginalScales = new Dictionary<int, float>();
	

	
	//============================================================================================================================================//
    void Awake() 
	{
		SetUpOriginalScale();
	}
	
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
	void Update() 
	{
	
		//update list of sprite depending of curve being detected.
		//loop on the list of sprite
		//detect the lenghth of the sprite in the order of the list
		
		UpdateSpriteListFromCurvesDetected();
		UpdateScaleAndPositionsOfSpritesBeingDisplayed();
		
		
		
		//legacy code
		//RetrieveSpriteMathingCurves();
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
			UISlicedSprite s = colorSprites[spriteIndex];
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
				UISlicedSprite sprite = colorSprites[spriteIndex];
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
			
			UISlicedSprite sprite = colorSprites[spriteIndex];
			
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
		int totalCurves = curves.Length;
		
		//print (totalCurves);
		if (totalCurves > 0) {
			res = curve.CurveLength / (MaximumPointsAllowed * totalCurves);
		}
		
		//TODO compute the depassement.
		if (res > 1) {res = 1;}
		//print (res);
		res = res * MaximumScreenSized;
		return res;
	}
	

	
	//============================================================================================================================================//
	void ScaleSpriteWithPercentage(UISlicedSprite colorSprite, ColorString curve, float percentage) 
	{
		//print ("Float percentage in Scale SPrite with percentage " + percentage);
		
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
	void ScaleAllSprites() 
	{
		foreach (UISlicedSprite sprite in SpritesBeingDisplayed) 
		{
			ColorString curve = CurveMatchingSprite(sprite);
			float percent = CalculatePercentageForSprite(sprite,curve);
			ScaleSpriteWithPercentage(sprite,curve,percent);
		}
	}
	
	//============================================================================================================================================//
	
	void SetPositionForSprite(UISlicedSprite sprite) 
	{
		
		//Vector3 accumulator = new Vector3(0,0,0);
		
		float accumulator = 0.0f;
		
		int indexSprite = SpritesBeingDisplayed.IndexOf(sprite);
		
		
		print ("SetPosition: " + indexSprite);
		
			//print ("Inside SetPositionForSprite " + accumulator.x);
			for(int i=0;i<=indexSprite;i++) 
			{
				print ("SetPositionForSprite set accumulator for index: " + indexSprite);
				
				UISlicedSprite colorSprite = SpritesBeingDisplayed[i];
				print ("Scale X: " + colorSprite.transform.localScale);
				
				if (colorSprite != sprite) {
					print ("Scale X INSIDE: " + colorSprite.transform.localScale);
					accumulator += colorSprite.transform.localScale.x;
				}
			}
		
		print ("Accumulator: " + accumulator);
		
		sprite.transform.localPosition =  new Vector3(OriginalScalings[indexSprite].x  + accumulator, sprite.transform.localPosition.y,0);
	}
	

	
	
	
	
	
	
	//============================================================================================================================================//
	//============================================================================================================================================//
	//============================================================================================================================================//
	//============================================================================================================================================//
	
				//JUNK CODE
	
	//============================================================================================================================================//
	//============================================================================================================================================//
	
	
	
	//============================================================================================================================================//
	/*
	void RetrieveSpriteMathingCurves() 
	{
		ColorString[] curves = GameObject.FindObjectsOfType(typeof(ColorString)) as ColorString[];
		for (int i=0;i<curves.Length;i++) 
		{
			ColorString curve = curves[i];
			
			int indexColor = curve.ColorIndex;
			
			int spriteIndex = ColorBarSpriteIndexes[indexColor];
			
			float percentage = CalculatePercentageForSprite(colorSprites[spriteIndex],curve);
			//print (percentage);
			ScaleSpriteWithPercentage (colorSprites[spriteIndex],curve,percentage);
			
			//ScaleSpriteDependingOnCurveLength (colorSprites[spriteIndex], curve);
		}
	}
	
	
	
	
	//============================================================================================================================================//
	void ScaleSpriteDependingOnCurveLength(UISlicedSprite colorSprite, ColorString curve) 
	{
		
		int spriteIndex = colorSpritesList.IndexOf(colorSprite);
		//float scalingFactor = ScalingFactors[spriteIndex];
		
		print (spriteIndex);
		
		if (HasCurveLengthHasChangeSinceLastScale(colorSprite,curve))
		{
			//float scaleThing = Mathf.Atan(curve.CurveLength) / Mathf.PI / 2;
			//scaleThing *= 10;
			float scaleThing = curve.CurveLength;
			
			print (curve.CurveLength);
			
			colorSprite.transform.localScale =  OriginalScalings[spriteIndex] + new Vector3(scaleThing, 0, 0);
			UpdatedLenthForCurve(curve);
		};
	}
			
	
	//============================================================================================================================================//		
	void UpdatedLenthForCurve(ColorString curve) 
	{
		LastRecordedLength[curve] = curve.CurveLength;
	}
	
	//============================================================================================================================================//		
	bool HasCurveLengthHasChangeSinceLastScale(UISlicedSprite sprite,ColorString curve) 
	{
		if (LastRecordedLength.ContainsKey(curve)) 
		{
			float oldLength = LastRecordedLength[curve];
			return (oldLength != curve.CurveLength);
		}
		else 
		{
			LastRecordedLength[curve] = curve.CurveLength;
			/*
			Vector3 accumulator = new Vector3(0,0,0);
			foreach (UISlicedSprite colorSprite in colorSpritesList) 
			{
				if (colorSprite != sprite) 
				{
					accumulator += colorSprite.transform.localScale;
				}
			}*/
	//		return true;
//		}	
//	}
	
	
	
}
