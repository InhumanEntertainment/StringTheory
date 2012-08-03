using UnityEngine;
using System.Collections;

public class LevelConnectBases : MonoBehaviour {
	
	bool hadBeenConnected = false;
	
	// Use this for initialization
	void Awake () 
    {		
		Game.Log("will try to connect bases");
        ClearAllBaseConnections();

		if (!hadBeenConnected) 
		{
			 ColorBase[] bases = (ColorBase[]) GameObject.FindObjectsOfType(typeof(ColorBase));
			
			 for (int i=0;i<bases.Length;i++) 
			 {
				Game.Log("try to connect base of index " +i);
				ConnectPairForBase(bases[i],bases);		
				
			 }
			hadBeenConnected = true;
		}
	}

    //=====================================================================================================================================//
    bool isSibling(GameObject first, GameObject second)
    {
        Transform parent1 = first.transform.parent;
        Transform parent2 = second.transform.parent;
        Game.Log(parent1 == parent2);

        return (parent1 == parent2);
    }

    //=====================================================================================================================================//
    void ClearAllBaseConnections()
    {
        ColorBase[] bases = (ColorBase[])GameObject.FindObjectsOfType(typeof(ColorBase));

        for (int i = 0; i < bases.Length; i++)
        {
            bases[i].colorBasePeers.Clear();
        }
    }

    //=====================================================================================================================================//
    int count = 0;
    void ConnectPairForBase(ColorBase colorBase, ColorBase[] allBases) 
	{
		for (int i=0;i<allBases.Length;i++) 
        {
            ColorBase potentialPair = allBases[i];
            if (potentialPair != colorBase && isSibling(colorBase.gameObject, potentialPair.gameObject) ) 
            {
                Game.Log("find potential pair that does not match base at index: " + i);
				
	            bool isColorMatch = (potentialPair.baseName == colorBase.baseName);
	            if (isColorMatch) 
                {
		            print (" find matching base and pair pair with name " + colorBase.baseName + " matching name of pair: "  + potentialPair.baseName);
		            colorBase.colorBasePeers.Add(potentialPair);
	            }
            }
            Game.Log(count++);	
        }
	}	
}
