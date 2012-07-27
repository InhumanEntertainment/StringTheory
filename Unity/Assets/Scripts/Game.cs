using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{	
	float PlayTime;
	float StartTime;
	string PlayTimeLabel;
	bool  HasLevelBeenCompleted = false;

    public enum GameScreen { Splash, Menu, About, Packs, Levels, Game, Pause, Complete };
    public GameScreen CurrentScreen = GameScreen.Splash;
    public Transform MenuGroup;
    public Transform GameGroup;

    public bool Logging = true;
	
    //============================================================================================================================================//
	void Awake() 
	{
   		StartTime = Time.time;
        Application.targetFrameRate = 60;
	}

    //============================================================================================================================================//
	void Update()
	{
        DebugMode = Logging;

        if (CurrentScreen == GameScreen.Game)
	    {    
		    if (PairOfCurvesToConnect() == 0 && ! HasLevelBeenCompleted) {
			    LevelCompleted();
		    }else{
			    if (! HasLevelBeenCompleted) 
			    {
				    UpdateStringTime ();	
			    }
		    }

            UpdateStringTime();
            UpdateHud();
        }

        // Handle Async Level Loading //
        if (Async != null)
        {
            if (Async.isDone)
            {
                // Delete Old Level Objects //
                if (LastLevel != -1)
                {
                    if (CurrentLevel != LastLevel || LastLevel == -1)
                    {
                        GameObject root = GameObject.Find(LevelList[LastLevel]);
                        if (root != null)
                        {
                            // Play Transition //
                            Animation anim = root.transform.FindChild("Nodes").animation;
                            root.transform.position = new Vector3(0, 0, -5);
                            anim.PlayQueued("Level_Close", QueueMode.PlayNow);
                        }

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
	}

    //============================================================================================================================================//
	void UpdateHud()
    {
        // Update Hud Labels //
        ColorString[] curves = GameObject.FindObjectsOfType(typeof(ColorString)) as ColorString[];
		float totalCurveLength = 0;
		for (int i=0; i<curves.Length;i++) 
		{
            totalCurveLength += curves[i].CurveLength;
		}

        GameObject distanceLabel = GameObject.Find("DistanceLabel");
        GameObject timeLabel = GameObject.Find("TimeLabel");

        if (timeLabel != null)
        {
            UILabel time = timeLabel.GetComponent<UILabel>();
            time.text = PlayTime.ToString("N1");

            UILabel distance = distanceLabel.GetComponent<UILabel>();
            distance.text = (totalCurveLength / 100f).ToString("N1") + "m";
        }
    }
	
    //============================================================================================================================================//
	public void SetScreen(GameScreen screen)
    {
        if (screen != CurrentScreen)
        {
            CurrentScreen = screen;

            if (screen == GameScreen.Menu)
            {
                GameGroup.gameObject.SetActiveRecursively(false);
                MenuGroup.gameObject.SetActiveRecursively(true);
            }
            else if (screen == GameScreen.Game)
            {
                GameGroup.gameObject.SetActiveRecursively(true);
                MenuGroup.gameObject.SetActiveRecursively(false);
            }
        }       
    }

	//============================================================================================================================================//
	void LevelCompleted()
	{
		HasLevelBeenCompleted = true;
	}	
	
	//============================================================================================================================================//
	void UpdateStringTime() 
	{
		PlayTime = Time.time - StartTime;

  		int minutes = (int) PlayTime / 60;
   		int seconds = (int) PlayTime % 60;
   		int fraction = (int) (PlayTime * 100) % 100;

   		PlayTimeLabel =  minutes + "min " + seconds + "sec " +  fraction + "m"; 
	}
	
	
	//============================================================================================================================================//
	int PairOfCurvesToConnect() 
	{
		ColorBase [] bases = GameObject.FindObjectsOfType(typeof(ColorBase)) as ColorBase[];
		int total = 0;
		
		for (int i=0; i<bases.Length;i++) 
		{
			bool shouldCountTheBase = true;
			
			GameObject curve = bases[i].Curve;
			if (curve) 
			{	
				ColorString baseColorString = curve.GetComponent<ColorString>();	
				 if ( baseColorString.HasCurveReachedTarget ) 
				{
					shouldCountTheBase = false;
				}
			}else{
				GameObject expectedCurve = bases[i].ExpectedCurve;
				if (expectedCurve) {
					ColorString expectedColorString = expectedCurve.GetComponent<ColorString>();	
					if (expectedColorString.HasCurveReachedTarget) 
					{
						shouldCountTheBase = false;
					}	
				}
			} 
			
			if (shouldCountTheBase) 
			{
				total ++;
			}
		}
		return (int) total/2;
	}
	
	//============================================================================================================================================//
	/*void OnGUI() 
    {
        float totalCurveLength = 0;

		ColorString[] curves = GameObject.FindObjectsOfType(typeof(ColorString)) as ColorString[];
		for (int i=0; i<curves.Length;i++) 
		{
			GUI.Label(new Rect(0, i * 40, 100, 100), "Curve Length is: " + curves[i].CurveLength);
            totalCurveLength += curves[i].CurveLength;
		}
		
		GUI.Label(new Rect(250, 0, 100, 100), "Play Time is: " + PlayTimeLabel);
		GUI.Label(new Rect(250, 40, 100, 100), "Pair to connect are: " + PairOfCurvesToConnect());
		
		if (HasLevelBeenCompleted) 
		{
			GUI.Label(new Rect(250, 80, 100, 100), "Level Finished");
		}
    }*/






    AsyncOperation Async;
    public int CurrentLevel;
    public int LastLevel;
    public List<string> LevelList;
    public List<int> LevelIndexList;
    public List<string> LevelIgnoreList;

    //=====================================================================================================================================//
    /*void OnGUI()
    {
        // Temp Game Buttons //
        if (CurrentScreen == GameScreen.Game)
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

            // Levels Level //
            if (GUI.Button(new Rect(Screen.width * 0.0f, Screen.height - height, Screen.width * 0.2f, height), "Levels"))
            {
                SetScreen(GameScreen.Menu); 
            }
        }
    } */

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
        StartTime = Time.time;

        LastLevel = CurrentLevel;
        CurrentLevel = index;
        if (CurrentLevel != LastLevel)
        {
            Async = Application.LoadLevelAdditiveAsync(LevelIndexList[index]);
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
            Game.Log("Level '" + level + "' Not Found");
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
    [UnityEditor.MenuItem("CONTEXT/Game/Update Level List")]
    static void UpdateLevelList(UnityEditor.MenuCommand command)
    {
        Game context = (Game)command.context;
        List<string> levels = ReadNames();
        List<string> gameLevels = new List<string>();
        List<int> gameIndexes = new List<int>();

        for (int i = 0; i < levels.Count; i++)
        {
            if (!context.LevelIgnoreList.Contains(levels[i]))
            {
                gameLevels.Add(levels[i]);
                gameIndexes.Add(i);
            }
        }

        context.LevelList = gameLevels;
        context.LevelIndexList = gameIndexes;
    }
#endif

    //=====================================================================================================================================//
    static public bool DebugMode = true;
    static public void Log(object obj)
    {
        if (DebugMode)
        {
            Debug.Log(obj.ToString());
        }
    }
}
