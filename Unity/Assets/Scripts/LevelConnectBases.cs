using UnityEngine;
using System.Collections;

public class LevelConnectBases : MonoBehaviour {
	
	bool hadBeenConnected = false;
	
	// Use this for initialization
	void Awake () {
		
		print ("will try to connect bases");
	
		if (!hadBeenConnected) 
		{
			 ColorBase[] bases = (ColorBase[]) GameObject.FindObjectsOfType(typeof(ColorBase));
			
			 for (int i=0;i<bases.Length;i++) 
			 {
				print ("try to connect base of index " +i);
				ConnectPairForBase(bases[i],bases);		
				
			 }
			hadBeenConnected = true;
		}
	}
	
	void ConnectPairForBase(ColorBase colorBase, ColorBase[] allBases) 
	{
		 for (int i=0;i<allBases.Length;i++) 
			 {
				ColorBase potentialPair = allBases[i];
				if (potentialPair != colorBase) {
					print ("find potential pair that does not match base at index: " + i);
				
					bool isColorMatch = (potentialPair.baseName == colorBase.baseName);
					if (isColorMatch) {
						print (" find matching base and pair pair with name " + colorBase.baseName + " matching name of pair: "  + potentialPair.baseName);
						colorBase.colorBasePeers.Add(potentialPair);
					}
				}
				
			 }
	}
	
	
}
