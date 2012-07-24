using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelNames : MonoBehaviour
{
    public string[] Levels;

#if UNITY_EDITOR

    //=====================================================================================================================================//
    static string[] ReadNames()
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
        return temp.ToArray();
    }

    //=====================================================================================================================================//
    [UnityEditor.MenuItem("CONTEXT/LevelNames/Update Names")]
    static void UpdateNames(UnityEditor.MenuCommand command)
    {
        LevelNames context = (LevelNames)command.context;
        context.Levels = ReadNames();
    }

    //=====================================================================================================================================//
    void Reset()
    {
        Levels = ReadNames();
    }

#endif
}
