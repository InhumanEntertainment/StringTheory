using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackDataProvider {
	
	
	public static int BASIC_PACK_ID = 1;
	
	//============================================================================================================================================//
	public static PackDataModel BasicPack ()
	{
		PackDataModel basicPack = new PackDataModel();
		basicPack.PackId = BASIC_PACK_ID;
		basicPack.PackName = "Noob";
		basicPack.Levels = new List<LevelDataModel> ();
		return basicPack;
	}
	
}
