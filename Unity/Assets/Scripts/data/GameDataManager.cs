using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class GameDataManager {

	//============================================================================================================================================//
	public static void SaveUserDataFromGameDataModel(GameDataModel dataModel)
	{
		
		string userDataSerialized = Json.Serialize(dataModel.ConvertIntoDictionary());		
		string filePath = IOUtils.GetPathForDefaultDataFile();
		
		if (System.IO.File.Exists(filePath)) 
		{
			System.IO.File.Delete(filePath);
		} 
		System.IO.File.WriteAllText(filePath,userDataSerialized);
	}
	
	//============================================================================================================================================//
	public static GameDataModel GetUserDataFromFile() 
	{	
		GameDataModel gameDataModel = new GameDataModel();
		string userDataFilePath = IOUtils.GetPathForDefaultDataFile();
		
		if (System.IO.File.Exists(userDataFilePath)) 
		{
			string jsonString = System.IO.File.ReadAllText(userDataFilePath);
			gameDataModel = GameDataModel.GameDataModelFromJsonString(jsonString);
		}
		return gameDataModel;
	}
	
}
