using UnityEngine;
using System.Collections;

public partial class Game : MonoBehaviour 
{	
	bool hadBeenConnected = false;

    //=====================================================================================================================================//
    void ReconnectBases() 
    {		
		//Game.Log("will try to connect bases");
        ClearAllBaseConnections();
        hadBeenConnected = false;

		if (!hadBeenConnected) 
		{
			 ColorBase[] bases = (ColorBase[]) GameObject.FindObjectsOfType(typeof(ColorBase));
			
			 for (int i=0;i<bases.Length;i++) 
			 {
				//Game.Log("try to connect base of index " +i);
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
    void ConnectPairForBase(ColorBase colorBase, ColorBase[] allBases) 
	{
        int sprite1 = colorBase.GetComponent<tk2dSprite>().spriteId;
                
		for (int i=0;i<allBases.Length;i++) 
        {
            ColorBase potentialPair = allBases[i];
            int sprite2 = potentialPair.GetComponent<tk2dSprite>().spriteId;
                
            if (potentialPair != colorBase && isSibling(colorBase.gameObject, potentialPair.gameObject) ) 
            {
                //Game.Log("find potential pair that does not match base at index: " + i);

                //bool isColorMatch = (potentialPair.baseName == colorBase.baseName);
                bool isColorMatch = (sprite1 == sprite2);
                if (isColorMatch) 
                {
		            print(" find matching base and pair pair with name " + colorBase.baseName + " matching name of pair: "  + potentialPair.baseName);
		            colorBase.colorBasePeers.Add(potentialPair);
	            }
            }
        }
	}	
}
