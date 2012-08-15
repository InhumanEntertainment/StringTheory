using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public partial class Game : MonoBehaviour
{
    static public Game Instance;
    public bool Logging = true;
    public int TargetFrameRate = 60;
    
    // Storage //
    public StringTheoryData Data;
    public string DataPath;
    public TextAsset DataFile;
     
    // Logic //
	float PlayTime;
	float StartTime;
	public bool Paused = false;

    // UI //
    public GameScreen CurrentScreen;
    public GameScreen LastScreen;
    public GameScreen[] Screens;
    public FXStars FX;

    public UILabel TimeLabel;
    public UILabel DistanceLabel;
    public UILabel LevelNameLabel;

    public UILabel PackNameLabel;
    public UILabel PackLevelsLabel;

    // Complete //
    public AudioClip CompletedSound;   
    public UILabel CompleteTimeLabel;
    public UILabel CompleteBestTimeLabel;
    public UILabel CompleteLengthLabel;
    public UILabel CompleteBestLengthLabel;
    public UILabel CompleteHighTimeLabel;
    public UILabel CompleteHighLengthLabel;
    public ColorBar ColorBar;

    // Packs //
    public StringTheoryPack CurrentPack;

    // Levels //
    AsyncOperation Async;
    public int CurrentLevel;
    public int LastLevel;
    public List<string> LevelList;
    public List<int> LevelIndexList;
    public List<string> LevelIgnoreList;

    public bool LevelIsTransitioning = false;
    public bool LevelHasCompleted = false;
    
    // Colors //
    public GameColor[] Colors;

    //============================================================================================================================================//
	void Awake() 
	{
        // Singleton //
        if (Game.Instance == null)
        {
            Game.Instance = this;
            DataPath = Application.persistentDataPath + "/StringTheory.xml";
            Data = StringTheoryData.Load(DataPath, DataFile);
            ReconnectBases();
            StartTime = Time.time;
            Application.targetFrameRate = TargetFrameRate;

            // Mute Music if Ipod is playing already //
            if (InhumanIOS.IsMusicPlaying())
                Audio.MusicMute = true;
        }
        else
        {
            Destroy(this.gameObject);
        }       
	}

    //============================================================================================================================================//
    void Start()
    {
        //CurrentScreen = Screens[0];

        SetScreen("Splash"); 
        SetScreen("Menu");
    }

    //============================================================================================================================================//
	void Update()
	{
        // Reload Data //
        if (Input.GetKeyDown(KeyCode.D))
        {
            Data = StringTheoryData.Load(DataPath);
            print("Data Reloaded");
        }

        DebugMode = Logging;
        if(Application.targetFrameRate != TargetFrameRate)
            Application.targetFrameRate = TargetFrameRate;

        // Handle Async Level Loading //
        if (Async != null)
        {
            if (Async.isDone)
            {
                // Delete Old Level Objects //
                if (LastLevel != -1 && CurrentLevel != LastLevel)
                {
                    DestroyLevel(LastLevel);
                }

                ReconnectBases();

                // Play Level Open //
                GameObject rootNew = GameObject.Find(LevelList[CurrentLevel]);
                Animation animNew = rootNew.transform.FindChild("Nodes").animation;
                animNew.PlayQueued("Level_Open", QueueMode.PlayNow);

                ColorBar bar = (ColorBar)GameObject.FindObjectOfType(typeof(ColorBar));
                bar.ResetColorBar();

                // Load in Level Name and Best Scores //
                LevelNameLabel.text = Data.Levels[CurrentLevel].Name;

                Async = null;
                LevelIsTransitioning = false;
            }
        }
        else
        {
            if (!Paused && !LevelHasCompleted && CurrentLevel > -1)
            {
                if (CurrentScreen.Name == "Game")
                {
                    if (PairOfCurvesToConnect() == 0)
                    {
                        LevelCompleted();
                        TimeLabel.gameObject.active = false;
                        DistanceLabel.gameObject.active = false;
                    }
                    else
                    {
                        UpdateHud();
                    }
                }
            }
        }
	}

    //============================================================================================================================================//
    void UpdateLevelCompleted()
    {
        // Count Up to current score, if best score then play sound and effect //
    }

    //============================================================================================================================================//
    void UpdateHud()
    {
        // Update Hud Labels //
        ColorString[] curves = GameObject.FindObjectsOfType(typeof(ColorString)) as ColorString[];
        float totalCurveLength = 0;
        for (int i = 0; i < curves.Length; i++)
        {
            totalCurveLength += curves[i].CurveLength;
        }

        if (TimeLabel != null)
        {
            PlayTime = Time.time - StartTime;

            int minutes = (int)PlayTime / 60;
            int seconds = (int)PlayTime % 60;
            int fraction = (int)(PlayTime * 100) % 100;

            bool minutePlus = minutes > 0;
            string time = (minutePlus ? minutes + ":" : "") + seconds.ToString(minutePlus ? "00" : "") + "." + fraction.ToString("00") + "sec";

            TimeLabel.text = time;
        }
    }

    //============================================================================================================================================//
    public void Pause()
    {
        Time.timeScale = 0;
        Paused = true;
    }

    //============================================================================================================================================//
    public void Resume()
    {
        Time.timeScale = 1;
        Paused = false;
    }

    //============================================================================================================================================//
    public void Reset()
    {
        // Are you sure dialog //
        Debug.Log("Reset");

        // Reload Base Scores but Not Game Settings //
        StringTheorySettings settings = Data.Settings;
        Data = StringTheoryData.Load(DataFile);
        Data.Settings = settings;
        Data.Save(DataPath);
    }

    //============================================================================================================================================//
    public void Retry()
    {
        CleanupScene();   
    }

    //============================================================================================================================================//
    public void PrevLevel()
    {
        CleanupScene();
        if (CurrentLevel > 0)
        {
            LoadLevel(CurrentLevel - 1);
        }
    }

    //============================================================================================================================================//
    public void NextLevel()
    {
        CleanupScene();
        if (CurrentLevel + 1 < LevelList.Count)
        {
            LoadLevel(CurrentLevel + 1);
        }
    }

    //============================================================================================================================================//
    // Spawn object and parent it under the current level so it will be destoryed on level exit //
    //============================================================================================================================================//
    public Object Spawn(Object original, Vector3 position, Quaternion rotation)
    {
        Object obj = (Object)Instantiate(original, position, rotation);
		
		if (CurrentLevel >= 0) 
		{
	        GameObject parent = GameObject.Find(LevelList[CurrentLevel]);

	        if (parent != null)
	        {
	            Transform xform = null;
	            if (obj is GameObject)
	                xform = ((GameObject)obj).transform;
	            else if (obj is Component)
	                xform = ((Component)obj).transform;
	
	            if(xform != null)
	                xform.parent = parent.transform;
	        }
		}

        return obj;
    }
    public Object Spawn(Object original)
    {
        return Spawn(original, Vector3.zero, Quaternion.identity);
    }

    //============================================================================================================================================//
    public void DestroyLevel(int levelIndex)
    {
        if (levelIndex != -1)
        {
            GameObject root = GameObject.Find(LevelList[levelIndex]);
            if (root != null)
            {                
                // Play Transition //
                Animation anim = root.transform.FindChild("Nodes").animation;
                root.transform.position = new Vector3(0, 0, -5);
                anim.PlayQueued("Level_Close", QueueMode.PlayNow);

                // Destroy All Objects //
                //root.SetActiveRecursively(false);     
            }

            CleanupScene();
        }                
    }

    //============================================================================================================================================//
    public void CleanupScene()
    {
        // Hide Completed //
        if (LevelHasCompleted)
        {
            CloseScreen("Completed");
        }

        // Destroy All Curves //
        ColorString[] curves = (ColorString[])GameObject.FindObjectsOfType(typeof(ColorString));
        for (int i = 0; i < curves.Length; i++)
        {
            Destroy(curves[i].gameObject);
        }

        // Remove Curve Refs on Bases //
        ColorBase[] bases = (ColorBase[])GameObject.FindObjectsOfType(typeof(ColorBase));
        for (int i = 0; i < bases.Length; i++)
        {
            bases[i].Curve = null;
        }

        // Reset Detector //
        GameObject curveManager = GameObject.FindGameObjectWithTag("CurveManager");
        CurveColliderDetector detector = curveManager.GetComponent<CurveColliderDetector>();
        detector.Curves.Clear();

        StartTime = Time.time;
        LevelHasCompleted = false;
        TimeLabel.gameObject.active = true;
        DistanceLabel.gameObject.active = true;
    }
	
    //============================================================================================================================================//
    // Transitions between screens //
    //============================================================================================================================================//
    public void SetScreen(string screen)
    {
        print("Set Screen: " + screen);

        for (int i = 0; i < Screens.Length; i++)
        {
            if (Screens[i].Name == screen && Screens[i].Name != CurrentScreen.Name)
            {
                CurrentScreen.Close(this);
                LastScreen = CurrentScreen;
                CurrentScreen = Screens[i];
                Screens[i].Open(this);

                if (screen == "Game")
                {
                    LastLevel = -1;
                    CurrentLevel = -1;
                }
            }
        } 
    }

    //============================================================================================================================================//
    public StringTheoryLevel FindLevel(string name)
    {
        for (int i = 0; i < Data.Levels.Count; i++)
        {
            string levelName = Path.GetFileNameWithoutExtension(Data.Levels[i].Scene);

            if (levelName == name)
                return Data.Levels[i];
        }

        return null;
    }

    //============================================================================================================================================//
    public void SetPack(string pack)
    {
        print("Set Pack: " + pack);

        // Setup Labels //
        for (int i = 0; i < Data.Packs.Count; i++)
        {            
            if (Data.Packs[i].Name == pack)
            {
                CurrentPack = Data.Packs[i];

                // Set Packs Label //
                PackNameLabel.text = CurrentPack.Name;

                // Set Levels Completed Total //
                int CompletedLevels = 0;
                for (int c = 0; c < CurrentPack.Levels.Count; c++)
			    {
                    StringTheoryLevel level = FindLevel(CurrentPack.Levels[c]);

                    if (level == null)
                        Debug.LogError("Missing Level: " + CurrentPack.Levels[c]);

                    if (level != null && level.Completed)
                        CompletedLevels++;
			    }

                PackLevelsLabel.text = CompletedLevels + "/" + CurrentPack.Levels.Count + " Completed";
            }
        }

        UpdateButtons();
    }

    //============================================================================================================================================//
    public void UpdateButtons()
    {
        LevelButton[] buttons = GameObject.FindObjectsOfType(typeof(LevelButton)) as LevelButton[];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].UpdateButton();
        }
    }

    //============================================================================================================================================//
    public void OpenScreen(string screen)
    {
        print("Open Screen: " + screen);

        for (int i = 0; i < Screens.Length; i++)
        {
            if (Screens[i].Name == screen)
            {
                Screens[i].Open(this);
            }
        }
    }

    //============================================================================================================================================//
    public void CloseScreen(string screen)
    {
        print("Close Screen: " + screen);

        for (int i = 0; i < Screens.Length; i++)
        {
            if (Screens[i].Name == screen)
            {
                Screens[i].Close(this);
            }
        }
    }

	//============================================================================================================================================//   
    void LevelCompleted()
	{
        Audio.Play(CompletedSound);

		LevelHasCompleted = true;      
        float finishTime = Time.timeSinceLevelLoad - StartTime;
        float finishLength = ColorBar.TotalLength;
        print(CurrentLevel);
        Data.Levels[CurrentLevel].Completed = true;

        // Best Time //
        CompleteTimeLabel.text = "Time: " + finishTime.ToString("N2") + "sec";
        if (Data.Levels[CurrentLevel].BestTime == 0)
        {
            CompleteBestTimeLabel.text = "Best: " + finishTime.ToString("N2") + "sec";        
            CompleteHighTimeLabel.text = "New Best Time!";
            Data.Levels[CurrentLevel].BestTime = finishTime;
        }
        else if (finishTime < Data.Levels[CurrentLevel].BestTime)
        {
            CompleteBestTimeLabel.text = "Previous Best: " + Data.Levels[CurrentLevel].BestTime.ToString("N2") + "sec";
            CompleteHighTimeLabel.text = "New Best Time!";
            Data.Levels[CurrentLevel].BestTime = finishTime;
        }
        else
        {
            CompleteBestTimeLabel.text = "Best: " + Data.Levels[CurrentLevel].BestTime.ToString("N2") + "sec";        
            CompleteHighTimeLabel.text = "";          
        }

        // Best Length //
        CompleteLengthLabel.text = "Length: " + finishLength.ToString("N2") + "m";
        if (Data.Levels[CurrentLevel].BestLength == 0)
        {
            CompleteBestLengthLabel.text = "Best: " + finishLength.ToString("N2") + "m";
            CompleteHighLengthLabel.text = "New Best Length!";
            Data.Levels[CurrentLevel].BestLength = finishLength;
        }
        else if (finishLength < Data.Levels[CurrentLevel].BestLength)
        {
            CompleteBestLengthLabel.text = "Previous Best: " + Data.Levels[CurrentLevel].BestLength.ToString("N2") + "m";
            CompleteHighLengthLabel.text = "New Best Length!";
            Data.Levels[CurrentLevel].BestLength = finishLength;
        }
        else
        {
            CompleteBestLengthLabel.text = "Best: " + Data.Levels[CurrentLevel].BestLength.ToString("N2") + "m";
            CompleteHighLengthLabel.text = "";
        }
        
        Data.Save(DataPath);

        // Hide Pause menu //
        // Explode Stars or border stars //

        // Open Level Completed Panel //
        OpenScreen("Completed");
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
			}
            else
            {
				GameObject expectedCurve = bases[i].ExpectedCurve;
				if (expectedCurve) 
                {
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

    //=====================================================================================================================================//
    public void LoadLevel(int index)
    {      
        if (!LevelIsTransitioning)
        {
            StartTime = Time.time;
            LastLevel = CurrentLevel;
            CurrentLevel = index;
            LevelIsTransitioning = true;   
        
            Async = Application.LoadLevelAdditiveAsync(LevelIndexList[index]);
        }
    }
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
            Debug.Log("Level '" + level + "' Not Found");
        }
    }

    //============================================================================================================================================//
    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            print("Paused");
            Data.Save(DataPath);
        }        
        else
            print("Unpaused");
    }

    //============================================================================================================================================//
    void OnApplicationQuit()
    {
        print("Quit");
        Data.Save(DataPath);
    }

//============================================================================================================================================//
// Editor Tools //
//============================================================================================================================================//
#region Editor

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

    /*static string DebugBuffer;
    static public void Store(object obj)
    {
        if (DebugMode)
        {
            //Debug.Log(obj.ToString());
            DebugBuffer += obj.ToString() + "\n";
        }
    }
    
    static public void PrintLog()
    {
        if (DebugMode)
        {
            Debug.Log(DebugBuffer);
            DebugBuffer = "\n";
        }
    }*/

#endregion
}

