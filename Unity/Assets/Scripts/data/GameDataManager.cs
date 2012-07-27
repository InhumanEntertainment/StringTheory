using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class GameDataManager : MonoBehaviour {

	//============================================================================================================================================//
	public void SaveUserDataFromGameDataModel(GameDataModel dataModel)
	{
		//Dictionary<string,GameDataModel> gameUserDataDictionary = new Dictionary<string, GameDataModel>();
		//Debug.Log("Data model should not be null " +  dataModel.fastestTime);
		//gameUserDataDictionary.Add("gameDataModel",);
		
		string userDataSerialized = Json.Serialize(dataModel.ConvertIntoDictionary());	
		System.IO.File.WriteAllText(IOUtils.GetPathForDefaultDataFile(),userDataSerialized);
	}
	
	//============================================================================================================================================//
	public GameDataModel GetUserDataFromFile() 
	{	
		GameDataModel gameDataModel = new GameDataModel();
		string userDataFilePath = IOUtils.GetPathForDefaultDataFile();
		
		if (System.IO.File.Exists(userDataFilePath)) 
		{
			string jsonString = System.IO.File.ReadAllText(userDataFilePath);
			gameDataModel = GameDataModel.GameDataModelFromJsonString(jsonString);
			//Dictionary<string,object> userDataJson = (IDictionary) Json.Deserialize(jsonPrizesList);
			//GameDataModel.
		}
		return gameDataModel;
	}
	
}
