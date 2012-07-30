using UnityEngine;
using System.Collections;

public class GameDataProvider : MonoBehaviour {
	
	public GameDataModel DataModel;
	
	//============================================================================================================================================//
	void Awake() 
	{
   		LoadGameDataIfExists ();
	}
	
	//============================================================================================================================================//
	void LoadGameDataIfExists() 
	{
		
		DataModel = GameDataManager.GetUserDataFromFile();
	}
	
	//============================================================================================================================================//
	public void SaveModel() 
	{
		GameDataManager.SaveUserDataFromGameDataModel(DataModel);
	}
	
	//============================================================================================================================================//
	public void SaveNewModel(GameDataModel newDataModel) 
	{
		Debug.Log("Should save new data model");
		GameDataManager.SaveUserDataFromGameDataModel(newDataModel);
		DataModel = newDataModel;
	}
}
