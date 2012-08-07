using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class TestLevel : MonoBehaviour 
{
    AsyncOperation Async;
    public string CurrentLevel;
    public string LastLevel;

    public GameScreen[] Packs;

	//=====================================================================================================================================//
	void Awake () 
    {
        MonsterContainer monsters = new MonsterContainer();

        monsters.Level = new LevelDataModel() { levelId = 1, levelCompletion = true, levelName = "N00bs" };
        monsters.Level = new LevelDataModel() { levelId = 2, levelCompletion = false, levelName = "World Tour" };
        monsters.Level = new LevelDataModel() { levelId = 3, levelCompletion = true, levelName = "Ape Shit" };

        monsters.Save(Application.persistentDataPath + "/monsters.xml");
	}

    //=====================================================================================================================================//
    void OnGUI() 
    {
        CreateButton("Level001", 0);
        CreateButton("Level002", 100);
        CreateButton("Level003", 200);     

        if (Async != null)
        {
            GUI.Label(new Rect(0, 100, 100, 100), Async.progress.ToString());

            if (Async.isDone)
	        {
		        // Delete Old Level Objects //
                var root = GameObject.Find(LastLevel);
                if (root != null && CurrentLevel != LastLevel)
                {
                    Destroy(root);                   
                }
	        } 
        }
	}

    //=====================================================================================================================================//
    void CreateButton(string level, int x)
    {
        if (GUI.Button(new Rect(x, 0, 100, 50), level))
        {
            LastLevel = CurrentLevel;
            CurrentLevel = level;
            if (CurrentLevel != LastLevel)
            {
                Async = Application.LoadLevelAdditiveAsync(CurrentLevel);
            }
        }
    }
}

//=====================================================================================================================================//
//=====================================================================================================================================//
//=====================================================================================================================================//
public class Monster
{
    [XmlAttribute("name")]
    public string Name;

    public int Health;
}

//=====================================================================================================================================//
//=====================================================================================================================================//
//=====================================================================================================================================//
[XmlRoot("MonsterCollection")]
public class MonsterContainer
{
    [XmlArray("Monsters"),XmlArrayItem("Monster")]
    public List<Monster> Monsters = new List<Monster>();
    public LevelDataModel Level;
    public Dictionary<string, string> Dick;

    //=====================================================================================================================================//
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(MonsterContainer));
        using(var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    //=====================================================================================================================================//
    public static MonsterContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(MonsterContainer));
        using(var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as MonsterContainer;
        }
    }
}