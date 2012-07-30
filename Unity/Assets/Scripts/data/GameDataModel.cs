using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class GameDataModel {
	
	public float FastestTime = 0.2f;
	public List<PackDataModel> packLevelList = new List<PackDataModel>();	
	
	
	//============================================================================================================================================//
	public IDictionary ConvertIntoDictionary() {
		
		Dictionary<string,object> dataModelDic = new Dictionary<string, object>();
		
		List<Dictionary<string,object>> listOfPackDicionaries = new List<Dictionary<string, object>>();
		foreach (PackDataModel pack in packLevelList) 
		{
			Dictionary<string,object> packDictionary = pack.ConvertIntoDictionary();
			listOfPackDicionaries.Add(packDictionary);
		}
		
		dataModelDic.Add ("fastestTime",FastestTime);
		dataModelDic.Add ("packLevelList",listOfPackDicionaries);
		
		return dataModelDic;
	}
	
	//============================================================================================================================================//
	public static GameDataModel GameDataModelFromJsonString(string jsonString) 
	{
		GameDataModel dataModel = new GameDataModel();
		
		Dictionary<string,object> gameDictionary = Json.Deserialize(jsonString) as Dictionary<string,object>;
		//retrieve fastest time
		float jsonFastestTime;
		if (gameDictionary["fastestTime"] is double) 
		{
			jsonFastestTime = (float) (double) gameDictionary["fastestTime"];	
		}else{
			jsonFastestTime = (float) (long) gameDictionary["fastestTime"];	
		}
		dataModel.FastestTime = jsonFastestTime;
		
		
		//retrieve pack list
		Debug.Log("Debug Pack list " + gameDictionary["packLevelList"]);
		List<object> packList = (List<object>) gameDictionary["packLevelList"];
		
		List<PackDataModel> packs = new List<PackDataModel>();
		foreach (Dictionary<string,object> packDico in packList) 
		{
			PackDataModel packModel = PackDataModel.LevelDataModelFromDictionary(packDico);
			packs.Add(packModel);
		}
		dataModel.packLevelList = packs;
		
		return dataModel;
	}
	
	//============================================================================================================================================//
	public void AddNewPack(PackDataModel newPack) 
	{
		bool isPackExistsAlready = false;
		foreach (PackDataModel pack in packLevelList) 
		{
			if (pack.PackId == newPack.PackId) {
				isPackExistsAlready = true;
			}
		}
		if (! isPackExistsAlready) 
		{
			packLevelList.Add(newPack);
		}
	}
	
	//============================================================================================================================================//
	public void UpdatePack(PackDataModel packToUpdate) 
	{
		PackDataModel existingPack = this.GetPackByID(packToUpdate.PackId);
		if (existingPack.PackId > 0) 
		{
			this.packLevelList.Remove(existingPack);
		}
		this.packLevelList.Add(packToUpdate);
	}
	
	//============================================================================================================================================//
	public PackDataModel GetPackByID(int packID) 
	{
		foreach (PackDataModel pack in packLevelList) 
		{
			if (pack.PackId == packID) {
				return pack;
			}
		}
		return new PackDataModel();
	}
	
	
	//============================================================================================================================================//
	public GameDataModel DummyDataModel() 
	{
		GameDataModel UserData = new GameDataModel();
		UserData.FastestTime = 0;
		
		
		
		//UserData.packLevelCompletion.Add(1,
		
		PackDataModel packEinstein = new PackDataModel();
		packEinstein.PackName = "Einstien";
		packEinstein.PackId = 1;
		
		LevelDataModel levelOne = new LevelDataModel();
		levelOne.levelName = "Inhuman";
		levelOne.levelCompletion = false;
		levelOne.levelCompletionTime = 0;
		
		
		packEinstein.Levels.Add (levelOne);
		
		
		//pack blackhole
		PackDataModel packBlackHole = new PackDataModel();
		packBlackHole.PackName = "BlackHole";
		packBlackHole.PackId = 2;
		
		LevelDataModel levelOneBlack = new LevelDataModel();
		levelOneBlack.levelName = "zoop";
		levelOneBlack.levelCompletion = false;
		levelOneBlack.levelCompletionTime = 0;;
		
		LevelDataModel levelTwoBlack = new LevelDataModel();
		levelTwoBlack.levelName = "zop Black";
		levelTwoBlack.levelCompletion = false;
		levelTwoBlack.levelCompletionTime = 0;;
		
		
		packBlackHole.Levels.Add(levelOneBlack);
		packBlackHole.Levels.Add(levelTwoBlack);
		
		
		//pack
		PackDataModel packFilthy = new PackDataModel();
		packFilthy.PackName = "filthy";
		packFilthy.PackId = 3;
		
		LevelDataModel levelOneFilthy = new LevelDataModel();
		levelOneFilthy.levelName = "Filthy one";
		levelOneFilthy.levelCompletion = true;
		levelOneFilthy.levelCompletionTime = 0.0f;
		
		packFilthy.Levels.Add(levelOneFilthy);
		
		
		
		UserData.packLevelList.Add(packEinstein);
		UserData.packLevelList.Add(packBlackHole);
		UserData.packLevelList.Add(packFilthy);
		
		return UserData;
	}
	
}
