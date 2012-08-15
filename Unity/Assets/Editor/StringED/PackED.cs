using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class PackED : EditorWindow
{
    public List<Object> Objects = new List<Object>();

    //============================================================================================================================================//
    [MenuItem("Inhuman/PackED")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(PackED));
    }

    //============================================================================================================================================//
    public void SetButtonIndexes()
    {

        for (int i = 0; i < Objects.Count; i++)
        {
            LevelButton button = (LevelButton)Objects[i];
            button.name = (i + 1).ToString();
            button.Index = i;
            button.Label.text = (i + 1).ToString();

            EditorUtility.SetDirty(button);
            EditorUtility.SetDirty(button.Label);

            Debug.Log(button.name + " : " + i);
        }
    }

    //============================================================================================================================================//
    public void RandomButtonIndexes()
    {
        // Create Random List //
        List<int> Indexes = new List<int>();
        List<int> RandomIndexes = new List<int>();
        
        for (int i = 0; i < Objects.Count; i++)
        {
            Indexes.Add(i);
        }

        for (int i = 0; i < Objects.Count; i++)
        {
            int rand = Random.Range(0, Indexes.Count);

            RandomIndexes.Add(Indexes[rand]);
            Indexes.RemoveAt(rand);          
        }

        for (int i = 0; i < Objects.Count; i++)
        {
            LevelButton button = (LevelButton)Objects[i];
            button.Index = RandomIndexes[i];
            button.name = (RandomIndexes[i] + 1).ToString();
            button.Label.text = (RandomIndexes[i] + 1).ToString();

            EditorUtility.SetDirty(button);
            EditorUtility.SetDirty(button.Label);

            Debug.Log(button.name + " : " + i);
        }
    }

    //============================================================================================================================================//
    void OnSelectionChange()
    {
        Object[] sel = Selection.GetFiltered(typeof(LevelButton), SelectionMode.Unfiltered);

        if (sel.Length > 0)
        {
            for (int i = 0; i < sel.Length; i++)
            {
                if (!Objects.Contains(sel[i]))
                {
                    Objects.Add(sel[i]);
                }
            }
        }
    }

    //============================================================================================================================================//
    void OnGUI()
    {
        GUI.backgroundColor = Color.grey;
        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("Reset"))
        {
            Objects.Clear();
        }

        for (int i = 0; i < Objects.Count; i++)
        {
            GUILayout.Label(Objects[i].name);
        }

        if (GUILayout.Button("Set Indexes"))
        {
            SetButtonIndexes();
        }

        if (GUILayout.Button("Random Indexes"))
        {
            RandomButtonIndexes();
        }

        EditorGUILayout.EndVertical();
    }

    //============================================================================================================================================//
    void OnInspectorUpdate()
    {
        this.Repaint();
    }
}