using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class GameColor
{
    // Sprites //
    public int ID = 0;
    public int TrackerID = 0;
    public int ArrowID = 0;

    // Materials //
    public Material Material;
    public Material TrackerMaterial;
    public Material ArrowMaterial;

    // Colors //
    public UnityEngine.Color FXColor = UnityEngine.Color.white;
    public UnityEngine.Color ProgressColor = UnityEngine.Color.white;

    Dictionary<int, Material> MaterialLookup = new Dictionary<int, Material>();
    //Dictionary<iMaterial> MaterialLookup = new Dictionary<int, Material>();

    //============================================================================================================================================//
    public Material GetStringMaterial(int id)
    {
        return MaterialLookup[id];
    }

    //============================================================================================================================================//
    public Material GetBaseID(Material material)
    {
        //MaterialLookup.
        return null;
    }
    
    public static bool operator == (GameColor x, GameColor y)
    {
        return (x.ID == y.ID);
    }

    public static bool operator !=(GameColor x, GameColor y)
    {
        return (x.ID != y.ID);
    }

}