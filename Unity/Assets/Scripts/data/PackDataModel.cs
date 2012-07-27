using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackDataModel {
	
	public int PackId = -1;
	public string PackName = "";
	public List<LevelDataModel> Levels = new List<LevelDataModel>();
	
	
	//============================================================================================================================================//
	public Dictionary<string,object> ConvertIntoDictionary()
	{
		Dictionary<string,object> levelModelDic = new Dictionary<string, object>();
		List<Dictionary<string,object>> levelList = new List<Dictionary<string, object>>();
		
		foreach (LevelDataModel level in Levels) 
		{
			Dictionary<string,object> levelDic = level.ConvertIntoDictionary(); 
			levelList.Add(levelDic);
		}
		
		levelModelDic.Add("packName",PackName);
		levelModelDic.Add ("levels",levelList);
		levelModelDic.Add ("packId",PackId);
		return levelModelDic;
	}
	
	//============================================================================================================================================//
	public static PackDataModel LevelDataModelFromDictionary(Dictionary<string,object> levelDictionary) 
	{
		PackDataModel dataModel = new PackDataModel();
		
		string packName = (string) levelDictionary["packName"];
		//object[] levels = (object [])levelDictionary["levels"];
		List<object> levels = (List<object>) levelDictionary["levels"];
		
		//TODO transform the levels array into a list
		
		//Debug.Log (" Find " + levels.Length + "levels for the package: " + packName);
		List<LevelDataModel> levelListFromJson = new List<LevelDataModel>();
		for (int i=0;i<levels.Count;i++)
		{
			Dictionary<string,object> levelDico = (Dictionary<string,object>) levels[i];
			LevelDataModel level = LevelDataModel.LevelDataModelFromDictionary(levelDico);
			levelListFromJson.Add(level);
		}
		
		int packId = (int) (long)levelDictionary["packId"];
		
		dataModel.PackName = packName;
		dataModel.Levels = levelListFromJson;
		dataModel.PackId = packId;
		
		
		return dataModel;
	}
	
}
