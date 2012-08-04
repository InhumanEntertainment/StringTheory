using UnityEngine;
using System.Collections;

public partial class Game : MonoBehaviour 
{	
	bool hadBeenConnected = false;

    //=====================================================================================================================================//
    void ReconnectBases() 
    {		
        ColorBase[] bases = (ColorBase[]) GameObject.FindObjectsOfType(typeof(ColorBase));
        ClearAllBaseConnections(bases);
		
		for (int i=0;i<bases.Length;i++) 
		{
		    ConnectPairForBase(bases[i],bases);						
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
    void ClearAllBaseConnections(ColorBase[] bases)
    {
        for (int i = 0; i < bases.Length; i++)
        {
            bases[i].colorBasePeers.Clear();
        }
    }

    //=====================================================================================================================================//
    void ConnectPairForBase(ColorBase colorBase, ColorBase[] allBases) 
	{        
		for (int i=0;i<allBases.Length;i++) 
        {
            ColorBase potentialPair = allBases[i];
  
            if (potentialPair != colorBase && isSibling(colorBase.gameObject, potentialPair.gameObject) ) 
            {
                if (colorBase.Color == potentialPair.Color)
                {             
                    colorBase.colorBasePeers.Add(potentialPair);
                }
            }
        }
	}	
}
