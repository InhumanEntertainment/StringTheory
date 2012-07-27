using UnityEngine;
using System.Collections;

public class IOUtils : MonoBehaviour {
	
	
	public static void CreateGameDataFileIfNotExists() 
	{
		string filePath = IOUtils.GetPathForDefaultDataFile();
		if (! System.IO.File.Exists (filePath)) {
			CreateDefaultFile();
		}		
	}
	
	//will crete the file prizes.txt with the default json record in it
	public static void CreateDefaultFile() {
		string defaultJsonString = "";
		System.IO.File.WriteAllText(IOUtils.GetPathForDefaultDataFile(),defaultJsonString); 	
	}
	
	public static string GetPathForDefaultDataFile() 
	{
		return GetPathForFileWithName("gamedata.txt");
	}
	
	public static string GetPathForFileWithName(string filename) {
		Debug.Log(Application.persistentDataPath);
		return Application.persistentDataPath + "/" + filename;
	}
}
