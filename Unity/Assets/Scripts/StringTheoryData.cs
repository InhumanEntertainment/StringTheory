using UnityEngine;
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
[System.Serializable]
public class StringTheorySettings
{
    public float MusicVolume = 1;
    public bool MusicMute = false;
    public float SoundVolume = 1;
    public bool SoundMute = false;
}

//=====================================================================================================================================//
//=====================================================================================================================================//
//=====================================================================================================================================//
[XmlRoot("StringTheory"), System.Serializable]
public class StringTheoryData
{
    public StringTheorySettings Settings = new StringTheorySettings();

    [XmlArray("Levels"), XmlArrayItem("Level")]
    public List<StringTheoryLevel> Levels = new List<StringTheoryLevel>();

    [XmlArray("Packs"), XmlArrayItem("Pack")]
    public List<StringTheoryPack> Packs = new List<StringTheoryPack>();

    //=====================================================================================================================================//
    public void Save(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(StringTheoryData));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    //=====================================================================================================================================//
    public static StringTheoryData Load(string path, TextAsset text)
    {
        if (File.Exists(path))
        {
            return Load(path);
        }
        else
        {
            return Load(text);
        }       
    }

    //=====================================================================================================================================//
    public static StringTheoryData Load(TextAsset text)
    {
        Debug.Log("Loaded from Memory");
        //InhumanIOS.Popup("Loaded Data", "From Memory", "OK"); 
        
        XmlSerializer serializer = new XmlSerializer(typeof(StringTheoryData));
        MemoryStream stream = new MemoryStream(text.bytes);       

        return serializer.Deserialize(stream) as StringTheoryData;
    }

    //=====================================================================================================================================//
    public static StringTheoryData Load(string path)
    {
        Debug.Log("Loaded from File: " + path);
        //InhumanIOS.Popup("Loaded Data", "From File: " + path, "OK");

        XmlSerializer serializer = new XmlSerializer(typeof(StringTheoryData));

        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as StringTheoryData;
        }
    }
}
