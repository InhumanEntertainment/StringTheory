using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Levels : MonoBehaviour 
{
    AsyncOperation Async;
    public int CurrentLevel;
    public int LastLevel;
    public List<string> LevelList;
    public List<int> IndexList;
    public List<string> IgnoreList;

    //=====================================================================================================================================//
    void Start()
    {
		Application.targetFrameRate = 60;
        LoadLevel(0);
    }
    
    //=====================================================================================================================================//
    void OnGUI() 
    {
        for (int i = 0; i < LevelList.Count; i++)
        {
            CreateButton(i, (float)i / LevelList.Count * Screen.width);
        }           

        // Previous Level //
        float aspect = (float)Screen.width / Screen.height;
        float top = (Screen.height - Screen.width) / 2f + Screen.width;
		float height = Screen.height * 0.05f;

        if (GUI.Button(new Rect(Screen.width * 0.0f, top, Screen.width * 0.33f, height), "Previous"))
        {
            if (CurrentLevel > 0)
	        {
                LoadLevel(CurrentLevel - 1);
	        }
        }

        // Retry Level //
        if (GUI.Button(new Rect(Screen.width * 0.33f, top, Screen.width * 0.34f, height), "Retry"))
        {
            var curves = GameObject.FindObjectsOfType(typeof(ColorString));
            for (int i = 0; i < curves.Length; i++)
            {
                Destroy(((ColorString)curves[i]).gameObject);
            }
        }

        // Next Level //
        if (GUI.Button(new Rect(Screen.width * 0.67f, top, Screen.width * 0.33f, height), "Next"))
        {
            if (CurrentLevel + 1 < LevelList.Count)
            {
                LoadLevel(CurrentLevel + 1);
            }
        }

        if (Async != null)
        {
            //GUI.Label(new Rect(0, 100, 100, 100), Async.progress.ToString());

            if (Async.isDone)
	        {
		        // Delete Old Level Objects //
                GameObject root = GameObject.Find(LevelList[LastLevel]);
                if (root != null && CurrentLevel != LastLevel)
                {
                    // Play Transition //
                    //Destroy(root); 
                    Animation anim = root.transform.FindChild("Nodes").animation;
                    //root.transform.position = new Vector3(0, 0, 50);
                    anim.PlayQueued("Level_Close", QueueMode.PlayNow);

                    // Play Level Open //
                    GameObject rootNew = GameObject.Find(LevelList[CurrentLevel]);
                    Animation animNew = rootNew.transform.FindChild("Nodes").animation;
                    animNew.PlayQueued("Level_Open", QueueMode.PlayNow);

                    Async = null;
                  
                    // Destroy All Curves //
                    var curves = GameObject.FindObjectsOfType(typeof(ColorString));
                    for (int i = 0; i < curves.Length; i++)
                    {
                        GameObject obj = ((ColorString)curves[i]).gameObject;
                        Destroy(obj);
                    }
                }
	        } 
        }
	}

    //=====================================================================================================================================//
    void CreateButton(int index, float x)
    {
        float height = Screen.height * 0.05f;
		float top = (Screen.height - Screen.width) / 2f - height;	

        if (GUI.Button(new Rect(x, top, 1f / LevelList.Count * Screen.width, height), index.ToString()))
        {
            LoadLevel(index);
        }
    }

    //=====================================================================================================================================//
    public void LoadLevel(int index)
    {
        LastLevel = CurrentLevel;
        CurrentLevel = index;
        if (CurrentLevel != LastLevel)
        {
            Async = Application.LoadLevelAdditiveAsync(IndexList[index]);
        }
    }

    //=====================================================================================================================================//
    public void LoadLevel(string level)
    {
        // Find index //
        if (LevelList.Contains(level))
	    {
		    int index = LevelList.IndexOf(level);
            LoadLevel(index);
	    }
        else
	    {
              print("Level '" + level + "' Not Found");  
	    }
    }
 
#if UNITY_EDITOR
    //=====================================================================================================================================//
    static List<string> ReadNames()
    {
        List<string> temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                temp.Add(name);
            }
        }
        return temp;
    }

    //=====================================================================================================================================//
    [UnityEditor.MenuItem("CONTEXT/Levels/Update Levels")]
    static void UpdateNames(UnityEditor.MenuCommand command)
    {
        Levels context = (Levels)command.context;
        List<string> levels = ReadNames();
        List<string> gameLevels = new List<string>();
        List<int> gameIndexes = new List<int>();

        for (int i = 0; i < levels.Count; i++)
        {
            if (!context.IgnoreList.Contains(levels[i]))
	        {
		        //levels.Remove(levels[i]);
                gameLevels.Add(levels[i]);
                gameIndexes.Add(i);
	        }
        }

        context.LevelList = gameLevels;
        context.IndexList = gameIndexes;
    }
#endif
}
