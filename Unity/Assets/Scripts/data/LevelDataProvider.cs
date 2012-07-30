using UnityEngine;
using System.Collections;

public class LevelDataProvider {

	//============================================================================================================================================//
	public static LevelDataModel DummyLevel() 
	{
		LevelDataModel dummyLevel = new LevelDataModel();
		
		dummyLevel.levelId = (int) (Random.value * 100);
			
		dummyLevel.levelName = "Dummy Level " + dummyLevel.levelId  + ".";
		
		return dummyLevel;
	}
}
