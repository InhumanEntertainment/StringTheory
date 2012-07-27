using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class LevelDataModel {
	
	public int		levelId = -1;
	public string 	levelName = "undefined";
	public bool 	levelCompletion = false;
	public float 	levelCompletionTime = 0.0f;
	
	//============================================================================================================================================//
	public Dictionary<string,object> ConvertIntoDictionary()
	{
		Dictionary<string,object> levelModelDic = new Dictionary<string, object>();
		
		levelModelDic.Add("levelName",levelName);
		levelModelDic.Add ("levelCompletion",levelCompletion);
		levelModelDic.Add ("levelCompletionTime",levelCompletionTime);
		levelModelDic.Add ("levelId",levelId);
			
		return levelModelDic;
	}
	
	//============================================================================================================================================//
	public static LevelDataModel LevelDataModelFromDictionary(Dictionary<string,object> levelDictionary) 
	{
		LevelDataModel dataModel = new LevelDataModel();
		string 	levelName = (string) levelDictionary["levelName"];
		bool 	levelCompletion = (bool) levelDictionary["levelCompletion"];
		
		
		object tempLevelCompletionTime = levelDictionary["levelCompletionTime"];
		float levelCompletionTime;
		if (tempLevelCompletionTime is long) {
			levelCompletionTime = (float) (double) (long) tempLevelCompletionTime;
		} else {
			levelCompletionTime = (float) (double) tempLevelCompletionTime;
		}
		
		int		levelId	= (int) ((long) levelDictionary["levelId"]);
		
		dataModel.levelName = levelName;
		dataModel.levelCompletion = levelCompletion;
		dataModel.levelCompletionTime = levelCompletionTime;
		dataModel.levelId = levelId;
		return dataModel;
	}
	
	
}
