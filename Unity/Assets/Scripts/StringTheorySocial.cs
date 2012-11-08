using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class StringTheorySocial
{
    //============================================================================================================================================//
    public void Awake()
    {
        // GameCenter //
        Social.localUser.Authenticate(AuthenticateCallback);
    }

    //============================================================================================================================================//
    public void Achievement()
    {
        IAchievement a1 = Social.CreateAchievement();
        a1.id = "firstplay";
        a1.percentCompleted = 100;
        a1.ReportProgress(ReportCallback);

        Social.ShowAchievementsUI();
    }

    //============================================================================================================================================//
    void AuthenticateCallback(bool success)
    {
        //InhumanIOS.Popup ("Authentication", success.ToString (), "Ok");

        if (success)
        {
            Debug.Log("Authentication Succesful");
            Social.LoadAchievements(AchievementsCallback);
        }
        else
        {
            Debug.Log("Authentication Failed");
        }
    }

    //============================================================================================================================================//
    void AchievementsCallback(IAchievement[] achievements)
    {
        //InhumanIOS.Popup ("Achievements", achievements.Length.ToString (), "Ok");		

        if (achievements.Length == 0)
        {
            Debug.Log("No Achievements Found");
        }
        else
        {
            Debug.Log(achievements.Length + " Achievements Found");
        }
    }

    //============================================================================================================================================//
    void ReportCallback(bool success)
    {
        //InhumanIOS.Popup ("Report", success.ToString (), "Ok");	
    }
}
