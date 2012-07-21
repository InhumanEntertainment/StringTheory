using UnityEngine;
using System.Collections;

public class TestLevel : MonoBehaviour 
{
    AsyncOperation Async;
    public string CurrentLevel;
    public string LastLevel;

	//=====================================================================================================================================//
	void Update () 
    {
        
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
