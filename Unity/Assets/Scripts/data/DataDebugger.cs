using UnityEngine;
using System.Collections;

public class DataDebugger : MonoBehaviour {

	public GameDataProvider DataProvider;
	
	
	//============================================================================================================================================//
	void OnGUI() 
    {
		if (GUI.Button (new Rect (0,0,250,100), " Reset Data To Default ")) 
		{
			CreateExampleGameDataConfiguration();
		}
		
		if (GUI.Button (new Rect (255,0,250,100), " Add One Level To Noob Pack ")) 
		{
			AddLevelToNoobPack();
		}
		
		if (GUI.Button (new Rect (505,0,100,100), " Update Noob Pack ")) 
		{
			UpdateNoobPack();
		}
		
		if (GUI.Button (new Rect (450,120,150,100), " Update Last Level Noob ")) 
		{
			UpdateNoobLastLevel();
		}
		
		
		DisplayGameDataModelInfos();
    }
	
	//============================================================================================================================================//
	void DisplayGameDataModelInfos () 
	{
		DisplayAllPackInfos();
	}
	
	//============================================================================================================================================//
	void DisplayAllPackInfos () 
	{
		int spacerY = 100 + 5;
		foreach (PackDataModel pack in DataProvider.DataModel.packLevelList) 
		{
			GUI.Label(new Rect(0, spacerY, 250, 20), "Pack Name: " + pack.PackName);
			spacerY = spacerY + 20 + 5;
			
			GUI.Label(new Rect(0, spacerY, 150, 20), "Pack ID: " + pack.PackId);
			spacerY = spacerY + 20 + 5;
			
			foreach (LevelDataModel level in pack.Levels) 
			{
				GUI.Label(new Rect(50, spacerY, 150, 20), "Level ID: " + level.levelId);
				spacerY = spacerY + 20 + 5;
				
				GUI.Label(new Rect(50, spacerY, 250, 20), "Level Name: " + level.levelName);
				spacerY = spacerY + 20 + 5;
				
				GUI.Label(new Rect(50, spacerY, 150, 20), "Level Completion: " + level.levelCompletion);
				spacerY = spacerY + 20 + 5;
				GUI.Label(new Rect(50, spacerY, 150, 50), "Level Completion Time: " + level.levelCompletionTime);
				spacerY = spacerY + 20 + 5;
			}
			
			
		}
		
	}
	
	//============================================================================================================================================//
	public void UpdateNoobLastLevel() 
	{
		PackDataModel noobPack = DataProvider.DataModel.GetPackByID(PackDataProvider.BASIC_PACK_ID);
		LevelDataModel lastlevel = noobPack.Levels[noobPack.Levels.Count-1];
		lastlevel.levelCompletion = true;
		lastlevel.levelCompletionTime = 12345234.23424f;
		
		noobPack.UpdateLevel(lastlevel);
		
		DataProvider.SaveModel();
	}
	
	//============================================================================================================================================//
	public void UpdateNoobPack() 
	{
		PackDataModel noobPack = DataProvider.DataModel.GetPackByID(PackDataProvider.BASIC_PACK_ID);
		noobPack.PackName = "Noob Updated";
		
		DataProvider.DataModel.UpdatePack(noobPack);
		DataProvider.SaveModel();
	}
	
	
	//============================================================================================================================================//
	public void AddLevelToNoobPack() 
	{
		DataProvider.DataModel.GetPackByID(PackDataProvider.BASIC_PACK_ID).AddLevel(LevelDataProvider.DummyLevel());
		DataProvider.SaveModel();
	}
	
	//============================================================================================================================================//
	void CreateExampleGameDataConfiguration() 
	{	
		GameDataModel dataModel = new GameDataModel();
		
		dataModel.AddNewPack(PackDataProvider.BasicPack());
		
		dataModel.GetPackByID(PackDataProvider.BASIC_PACK_ID).AddLevel(LevelDataProvider.DummyLevel());
		
		DataProvider.SaveNewModel(dataModel);
		
	}

}
