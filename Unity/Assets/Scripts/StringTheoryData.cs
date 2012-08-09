using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


//=====================================================================================================================================//
//=====================================================================================================================================//
//=====================================================================================================================================//
[System.Serializable]
public class StringTheoryLevel
{
    public string Name = "";
    public string Scene = "";
    public int Index;

    public bool Completed;
    public float BestTime;
    public float BestLength;
}

//=====================================================================================================================================//
//=====================================================================================================================================//
//=====================================================================================================================================//
[System.Serializable]
public class StringTheoryPack
{
    public string Name = "";
    public List<string> Levels = new List<string>();
}

//=====================================================================================================================================//
//=====================================================================================================================================//
//=====================================================================================================================================//
[XmlRoot("StringTheory")]
public class StringTheoryData
{
    [XmlArray("Levels"), XmlArrayItem("Level")]
    public List<StringTheoryLevel> Levels = new List<StringTheoryLevel>();

    [XmlArray("Packs"), XmlArrayItem("Pack")]
    public List<StringTheoryPack> Packs = new List<StringTheoryPack>();

    //=====================================================================================================================================//
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(StringTheoryData));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    //=====================================================================================================================================//
    public static StringTheoryData Load(string path)
    {
        var serializer = new XmlSerializer(typeof(StringTheoryData));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as StringTheoryData;
        }
    }
}
